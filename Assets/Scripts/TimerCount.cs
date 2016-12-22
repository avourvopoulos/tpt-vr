using UnityEngine;
using System.Collections;

public class TimerCount : MonoBehaviour {
	
    public static float myTimer = 0.0f;
	public static float startTime = 6.0f;
	
	public static bool timeout = false;

	// Use this for initialization
	void Start () 
	{
		myTimer = startTime;
	}
	
	// Update is called once per frame
	void Update () 
	{
		myTimer -= Time.deltaTime;
			
		if(myTimer <= 0 && myTimer > -1)
			{
				timeout = true;
			//	myTimer = startTime;
				myTimer = -1;
			}
		
		renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(0, startTime, myTimer));
	}
		
	
}
