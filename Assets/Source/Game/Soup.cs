﻿using UnityEngine;
using System.Collections;

public class Soup : MonoBehaviour
{

	public UIWinText winDelegate;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnCollisionEnter (Collision collision)
	{
		Destroy (gameObject);

		SendMessageUpwards ("OnLevelComplete");
	}

}
