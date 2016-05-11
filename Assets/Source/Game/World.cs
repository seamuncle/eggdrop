﻿using UnityEngine;
using System.Collections;

public class World : MonoBehaviour
{
	public Level prevLevel;

	public Level currLevel;

	public Level nextLevel;

	public string resourceFolder;

	public TrailTraversal birdTrailTraversal;

	public Trail levelTransitionTrail;

	/**
	 * This should be one less than the level name
	 */
	public int levelIndex = 1;

	public float levelTransitionDistance = 30f;


	// Use this for initialization
	void Start ()
	{
		if (nextLevel == null) {
			nextLevel = GetComponentInChildren<Level> ();
			// Assume positioned to origin

		}

		if (birdTrailTraversal == null) {
			birdTrailTraversal = GetComponentInChildren<TrailTraversal> ();
			// TODO check name
		}
		LoadNextLevel ();

		birdTrailTraversal.Begin( currLevel.birdTrail );
	}

	void LoadNextLevel ()
	{
		if (prevLevel != null) {
			DestroyImmediate (prevLevel);
		}

		prevLevel = currLevel;
		currLevel = nextLevel;

		levelIndex++;
		string fileName = resourceFolder + "/Level" + levelIndex;
		GameObject nextLevelObject = Resources.Load (fileName, typeof(GameObject)) as GameObject;

		if (nextLevelObject == null) {
			// current level is now the last--next could be a finish level
			return;
		}

		nextLevel = Instantiate (nextLevelObject).GetComponent<Level> ();

		nextLevel.transform.parent = currLevel.transform;
		nextLevel.transform.localPosition = Vector3.right * levelTransitionDistance;
	}

	void TransitionBird ()
	{
		iTween.Stop (birdTrailTraversal.gameObject);

		levelTransitionTrail.points [0].transform.position = birdTrailTraversal.GetPrevTrailPosition ();
		levelTransitionTrail.points [1].transform.position = birdTrailTraversal.GetCurrTrailPosition ();
		levelTransitionTrail.points [2].transform.position = (Vector3.left * levelTransitionDistance + currLevel.birdTrail.GetClampedPointFor (currLevel.birdTrail.points.Count - 1));
		levelTransitionTrail.points [3].transform.position = (Vector3.left * levelTransitionDistance + currLevel.birdTrail.GetClampedPointFor (0));

		birdTrailTraversal.Begin ( levelTransitionTrail );
	}


	void OnLevelComplete ()
	{
		LoadNextLevel ();

		TransitionBird ();

		var hash = iTween.Hash (
			           "oncomplete", "OnLevelMoveComplete",
			           "oncompletetarget", gameObject,
			           "oncompleteparams", prevLevel,
			           "time", levelTransitionTrail.secondsDuration, // Use time from allocated transition trail's duration
			           "position", Vector3.left * levelTransitionDistance

		           );
		iTween.MoveTo (prevLevel.gameObject, hash);

	}

	void OnLevelMoveComplete (Level prevLevel)
	{
		currLevel.transform.parent = null;
		Destroy (prevLevel);

		// This should happen when bird is done traversal, but for now, abuse the fact level tween is the same duration
		iTween.Stop (birdTrailTraversal.gameObject);
		birdTrailTraversal.Begin ( currLevel.birdTrail );
	}
}
