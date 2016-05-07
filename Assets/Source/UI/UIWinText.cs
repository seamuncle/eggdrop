using UnityEngine;
using System.Collections;

public class UIWinText : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void StartWin ()
	{
		gameObject.SetActive (true);
	}
}
