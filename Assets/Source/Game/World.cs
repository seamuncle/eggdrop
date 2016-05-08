using UnityEngine;
using System.Collections;

public class World : MonoBehaviour
{
	public Level prevLevel;

	public Level currLevel;

	public Level nextLevel;

	public string resourceFolder;

	public TrailTraversal bird;

	/**
	 * This should be one less than the level name
	 */
	public int levelIndex = 1;

	public float levelTransitionTime = 0.8f;


	public float levelTransitionDistance = 30f;


	// Use this for initialization
	void Start ()
	{
		if (nextLevel == null) {
			nextLevel = GetComponentInChildren<Level> ();
			// Assume positioned to origin

		}

		if (bird == null) {
			bird = GetComponentInChildren<TrailTraversal> ();
		}
		LoadNextLevel ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
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

	void OnLevelComplete ()
	{
		LoadNextLevel ();

		// TODO create path for bird given current curve points and starting curve points on next level

		// TODO tween bird

		// tween nextLevel

		var hash = iTween.Hash (
			           "oncomplete", "OnLevelMoveComplete",
			           "oncompletetarget", gameObject,
			           "oncompleteparams", prevLevel,
			           "time", levelTransitionTime,
			           "position", Vector3.left * levelTransitionDistance

		           );
		iTween.MoveTo (prevLevel.gameObject, hash);

	}

	void OnLevelMoveComplete (Level prevLevel)
	{
		currLevel.transform.parent = null;
		Destroy (prevLevel);
	}
}
