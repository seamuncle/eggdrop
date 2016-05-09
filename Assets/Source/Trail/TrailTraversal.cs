﻿using UnityEngine;
using System.Collections;

public class TrailTraversal : MonoBehaviour
{
	// Trail this traveral is following
	public Trail trail;

	// A value between 0 and the duration of the traversal, in seconds.
	public float secondsIntoTraversal = 0f;

	public bool isActive = true;

	private Transform[] points;

	private float secondsPerPoint;

	// Use this for initialization
	public void Start ()
	{
		if (trail == null || trail.secondsDuration <= 0f) {
			Debug.LogError ("Trail/Traversal misconfigured", gameObject);
		}
		iTween.Init (gameObject); 
		BuildPoints ();
		secondsIntoTraversal = 0f;

	}

	private void BuildPoints ()
	{
		bool isLooping = trail.loopType == iTween.LoopType.loop;
		int pointsLength = trail.points.Count + (isLooping ? 1 : 0);

		points = new Transform[pointsLength];
		trail.points.CopyTo (points);

		if (isLooping) {
			points [pointsLength - 1] = points [0];
		}
		secondsPerPoint = trail.secondsDuration / (pointsLength - 1);
	}

	void FixedUpdate ()
	{
		if (!isActive) {
			return;
		}
		secondsIntoTraversal += Time.fixedDeltaTime;
		startingPointIndex = Mathf.FloorToInt ((points.Length - 1) * GetPercentCompletion ());

		if (secondsIntoTraversal > trail.secondsDuration) {
			if (trail.loopType == iTween.LoopType.loop) {
				secondsIntoTraversal -= trail.secondsDuration;
			} else {
				secondsIntoTraversal = trail.secondsDuration;
				isActive = false;
			}
		} 
		transform.position = GetCurrTrailPosition ();


	}

	private int startingPointIndex = 0;

	public float GetPercentCompletion ()
	{
		return secondsIntoTraversal / trail.secondsDuration;
	}

	public int GetStartingPointIndex ()
	{
		return startingPointIndex;
	}

	public Vector3 GetPrevTrailPosition ()
	{
		return trail.GetClampedPointFor (startingPointIndex);
	}

	public Vector3 GetNextTrailPosition ()
	{
		return trail.GetClampedPointFor (startingPointIndex + 1);
	}

	public Vector3 GetCurrTrailPosition ()
	{
				
		float secondsBetweenPoints = secondsIntoTraversal % secondsPerPoint;
		float interpValue = secondsBetweenPoints / secondsPerPoint;
		if (trail.curveType == TrailCurveType.CATMULL_ROM) {
			//Clamp to allow looping
			Vector3 p0 = trail.GetClampedPointFor (startingPointIndex - 1);
			Vector3 p1 = GetPrevTrailPosition ();
			Vector3 p2 = GetNextTrailPosition ();
			Vector3 p3 = trail.GetClampedPointFor (startingPointIndex + 2);


			//Find the coordinates between the control points with a Catmull-Rom spline
			return CatmullRomPresenter.InterpolateCatmullRom (interpValue, p0, p1, p2, p3);

		} else if (trail.curveType == TrailCurveType.LINEAR) {
			return Vector3.Lerp (GetPrevTrailPosition (), GetNextTrailPosition (), interpValue);
		} else {
			return Vector3.zero;
		}
	}

}
