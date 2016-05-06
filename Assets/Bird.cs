using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour
{
	public float percentVelocityRelease = 0.7f;
	public GameObject eggPrefab;

	public Vector3 lastPosition;
	public Vector3 velocity;


	void Start() {
		eggPrefab = Resources.Load ("Egg", typeof(GameObject)) as GameObject;
		lastPosition = transform.position;
	}

	void FixedUpdate() {
		velocity =   (transform.position - lastPosition) / Time.fixedDeltaTime;
		lastPosition = transform.position;
	}

	void OnDrawGizmos() {
		Gizmos.DrawRay (transform.position, velocity);
	}


	void OnMouseOver ()
	{
		if (Input.GetMouseButtonDown (0)) {
			Debug.Log ("Click");
			GameObject newEgg = Instantiate (eggPrefab);
			Rigidbody eggPhysics = newEgg.GetComponent<Rigidbody> ();
			eggPhysics.velocity = percentVelocityRelease * velocity;
			eggPhysics.position = transform.position;
		}
	}
}
