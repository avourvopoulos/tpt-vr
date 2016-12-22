using UnityEngine;
using System.Collections;

public class PlayClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseDown()
	{
		
	}

	void OnMouseUp()
	{ 
		 animation.Play("clickAnimTest");
	}
}
