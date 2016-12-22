using UnityEngine;
using System.Collections;

public class LightAdjust : MonoBehaviour {
	
	public Light tilelight;

	// Use this for initialization
	void Start () 
	{
		 if(GeneralSettings.arch == "Grid")
		{
			tilelight.intensity = 1;
			tilelight.range = 1;
		//	Debug.Log("Grid Light");
		}
		else
		{
			tilelight.intensity = 1;
			tilelight.range = 2;
		//	Debug.Log("Serial Light");
		}
		
	}
	
	// Update is called once per frame
	void Update () 
	{
	 
	}
}
