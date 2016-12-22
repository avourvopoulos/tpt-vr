using UnityEngine;
using System.Collections;

public class FrameBorder : MonoBehaviour {
	
	public GameObject frame;

	// Use this for initialization
	void Start () 
	{
		frame.transform.position = new Vector3(Screen.currentResolution.width,Screen.currentResolution.height,-0.05f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
