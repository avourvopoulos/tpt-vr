using UnityEngine;
using System.Collections;

public class FingerAnim : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
			
		if (ClickSelection.grasp)	
		{
	       	animation.Play();
			ClickSelection.grasp = false;
			
			animation.Rewind();
		}

	}
	
	
}
