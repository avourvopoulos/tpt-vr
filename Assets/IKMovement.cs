using UnityEngine;
using System.Collections;

public class IKMovement : MonoBehaviour {
	
	float horizontalAxis = 0.0f;
	float verticalAxis = 0.0f;
	
	string device = "mouse";
		
	// Use this for initialization
	void Start () 
	{
		ikLimbLeft.IsEnabled = true;
		ikLimbRight.IsEnabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//switch device
		if(Input.GetKey(KeyCode.M))
		{device = "mouse";}
		if(Input.GetKey(KeyCode.K))
		{device = "keyboard";}
		
		//switch hands
		if(Input.GetKey(KeyCode.L))//left
		{
			ikLimbLeft.IsEnabled = true;
			ikLimbRight.IsEnabled = false;
			//hide/rotate arm
			GameObject.Find("RightShoulder").transform.Rotate(0,180,0);
			GameObject.Find("LeftShoulder").transform.Rotate(0,0,0);
		}
		if(Input.GetKey(KeyCode.R))//right
		{
			ikLimbLeft.IsEnabled = false;
			ikLimbRight.IsEnabled = true;
			//hide/rotate arm
		 	GameObject.Find("LeftShoulder").transform.Rotate(0,180,0);
			GameObject.Find("RightShoulder").transform.Rotate(0,0,0);
		}		
		
		//choose device and apply movement
		if(device == "mouse")
		{
			horizontalAxis = Input.GetAxis("Mouse X");
        	verticalAxis = Input.GetAxis("Mouse Y");

		}
		if (device=="keyboard")
		{
			horizontalAxis = Input.GetAxis("Horizontal");
        	verticalAxis = Input.GetAxis("Vertical");
		}
		
		float h = Time.deltaTime * horizontalAxis;
        float v = Time.deltaTime * verticalAxis;
		
		transform.Translate(h, v, 0);//apply movement to IK target
	}
	
	void OnGUI () 
	{
		GUI.Label (new Rect (10, 10, 200, 200), "Press:\n'K' for keyboard\n'M' for mouse\n'L' for left hand\n'R' for right hand");
	}
	
}
