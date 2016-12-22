using UnityEngine;
using System.Collections;

public class KeyboardMapping : MonoBehaviour {
	
	private float axisH = 0.0f;
	private float axisV = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		axisV = axisH = 0;
	//    if ( ) axisV = 1; 
	//    if ( ) axisV = -1; 
	    if (Input.GetKey(KeyCode.F9) || 
			Input.GetKey(KeyCode.F10) || //move left
			Input.GetKey(KeyCode.F11)|| 
			Input.GetKey(KeyCode.F12)) axisH = -1;
		
	    if (Input.GetKey(KeyCode.F1) || 
			Input.GetKey(KeyCode.F2) || //move right
			Input.GetKey(KeyCode.F3)|| 
			Input.GetKey(KeyCode.F4)) axisH = 1;
		
		
		float dirH = GeneralSettings.HandSpeed * InputGetAxis("Horizontal");
		float dirV = GeneralSettings.HandSpeed * InputGetAxis("Vertical");
		
		if (GeneralSettings.device=="Keyboard")
		{
//			transform.Translate(dirH, dirV, 0);
			
			if(GeneralSettings.horizontalBool)
			{
				transform.Translate(dirH, 0, 0);
			}
			if(GeneralSettings.verticalBool)
			{
				transform.Translate(0, dirV, 0);
			}
			if(GeneralSettings.verticalBool && GeneralSettings.horizontalBool)
			{
				transform.Translate(dirH, dirV, 0);
			}
			else{transform.Translate(0, 0, 0);}
		}

	}
	
	public float InputGetAxis(string axis)
	{
	    var v = Input.GetAxis(axis);
	    if (Mathf.Abs(v) > 0.005)return v;
	    if (axis=="Horizontal")return axisH;
	    if (axis=="Vertical") return axisV;
		else return 0;
	}
	
}
