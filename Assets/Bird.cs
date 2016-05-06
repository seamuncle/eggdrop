using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour
{
	/** 
	 * Spped egg is released relative to speed of bird 
	 */
	public float eggVelocityMultiplier = 1.3f;

	/**
	 * Resource pointing to egg game object to be inserted
	 */
	public GameObject eggPrefab;

	public Vector3 lastPosition;
	public Vector3 velocity;


	void Start ()
	{
		eggPrefab = Resources.Load ("Egg", typeof(GameObject)) as GameObject;
		lastPosition = transform.position;
	}

	void FixedUpdate ()
	{
		velocity = (transform.position - lastPosition) / Time.fixedDeltaTime;
		lastPosition = transform.position;
	}

	void OnDrawGizmos ()
	{
		Gizmos.DrawRay (transform.position, velocity);
	}


	void OnMouseOver ()
	{
		if (Input.GetMouseButtonDown (0)) {
			GameObject newEgg = Instantiate (eggPrefab);
			Rigidbody eggPhysics = newEgg.GetComponent<Rigidbody> ();
			eggPhysics.velocity = eggVelocityMultiplier * velocity;
			eggPhysics.position = transform.position;
		}
	}
}
