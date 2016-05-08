using UnityEngine;
using System.Collections;


public class Egg : MonoBehaviour
{
	void OnCollisionEnter (Collision collision)
	{
		// Debug-draw all contact points and normals
		foreach (ContactPoint contact in collision.contacts) {
			Debug.DrawRay (contact.point, contact.normal, Color.red);
		}

		Destroy (gameObject);
	}
}


