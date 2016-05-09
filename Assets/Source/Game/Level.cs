using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour
{

	public Trail birdTrail;

	// Use this for initialization
	void Start ()
	{
		if (birdTrail == null) {
			Trail[] trails = GetComponentsInChildren<Trail> ();
			foreach (Trail trail in trails) {
				if (trail.name == "BirdTrail") {
					birdTrail = trail;
					break;

				}
			}
		}


		if (birdTrail == null) {
			Debug.LogError ("Misconfigured Level has no birdTrail", this);
		}
	}
}
