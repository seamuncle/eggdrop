using UnityEngine;
using System.Collections;

public class TrailTraversal : MonoBehaviour
{
	// Trail this traveral is following
	public Trail trail;

	// Number of seconds a trail traversal should take
	public float secondsDuration;

	// A value between 0 and the duration of the traversal, in seconds.
	public float secondsIntoTraversal = 0f;

	public bool isActive = true;

	private Transform[] points;

	// Use this for initialization
	void Start ()
	{
		if (trail == null || secondsDuration <= 0f) {
			Debug.LogError ("TrailTraversal misconfigured", gameObject);
		}
		iTween.Init (gameObject); 
		BuildPoints ();

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

	}

	// Update is called once per frame
	void Update ()
	{
		if (!isActive) {
			return;
		}
		secondsIntoTraversal += Time.deltaTime;

		if (secondsIntoTraversal > secondsDuration) {
			if (trail.loopType == iTween.LoopType.loop) {
				secondsIntoTraversal -= secondsDuration;
			} else {
				secondsIntoTraversal = secondsDuration;
				isActive = false;
			}
		} 

		float percentCompletion = secondsIntoTraversal / secondsDuration;
		if (trail.curveType == TrailCurveType.CATMULL_ROM) {
			iTween.PutOnPath (gameObject.transform, points, percentCompletion);
		} else if (trail.curveType == TrailCurveType.LINEAR) {
			float secondsPerPoint = secondsDuration / (points.Length - 1); // This can be Cached from Start
			int startingPointIndex = Mathf.FloorToInt ((points.Length - 1) * percentCompletion);
			float secondsBetweenPoints = secondsIntoTraversal % secondsPerPoint;
			float interpValue = secondsBetweenPoints / secondsPerPoint;
			transform.position = Vector3.Lerp (points [startingPointIndex].position, points [startingPointIndex + 1].position, interpValue);
		}

	}
}
