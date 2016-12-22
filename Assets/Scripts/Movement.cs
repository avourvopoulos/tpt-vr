using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Movement : MonoBehaviour {
	
	public GameObject BorderU, BorderD, BorderL, BorderR;
	float xmin,xmax,ymin,ymax;//x,y borders
	
    float Speed;//hand speed

	float horizontalAxis = 0.0f;
	float verticalAxis = 0.0f;
	
	public static bool isClicked;
	
	public Texture2D MouseIcon;
	public Texture2D KeybIcon;
	public Texture2D TrackIcon;
	Texture2D DevIcon;
	public GUIStyle DeviceStyle;
	
	//for mouse calibration
	List<float> xvalues = new List<float>();
	List<float> yvalues = new List<float>();
	float scalex, scaley;
	public static bool calibLoop = false;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		//Get Tile Speed from the Seettings/Pause Menu
		Speed = GeneralSettings.HandSpeed;
		
		
		if (GeneralSettings.device=="Mouse")
		{
			
			var mousePos = Input.mousePosition;
//			mousePos.x -= Screen.currentResolution.width/2;
//			mousePos.y -= Screen.currentResolution.height/2;
			
			UDPReceive.udppos = new Vector3 (-mousePos.x, mousePos.y, 0);
			
			if(calibLoop)
			{
				UDPReceive.calibrate = true;
				UDPReceive.tracking=true;
//				StartCoroutine(CalibrateMouse());//start calibration
			}
			
			
			DevIcon = MouseIcon;
		}
		
		if(GeneralSettings.device == "RawMouse")
		{
			horizontalAxis = Input.GetAxis("Mouse X");
        	verticalAxis = Input.GetAxis("Mouse Y");
			
			UDPReceive.tracking=false;//stop UDP tracking
			DevIcon = MouseIcon;
		}
		
		if (GeneralSettings.device=="Keyboard")
		{
			horizontalAxis = Input.GetAxis("Horizontal");
        	verticalAxis = Input.GetAxis("Vertical");
			UDPReceive.tracking=false;//stop UDP tracking
			DevIcon = KeybIcon;
			
		}
		//UDP tracking
		if (GeneralSettings.device=="Tracking")
		{		
//			horizontalAxis = UDPReceive.dirH;
//			verticalAxis = UDPReceive.dirV;
					
			DevIcon = TrackIcon;
		}
			
		
		float h = Speed * horizontalAxis;
        float v = Speed * verticalAxis;

		
		//Move Hand if it's not Paused
		if(!GeneralSettings.isPaused)
		{
			if(GeneralSettings.horizontalBool)
			{
				transform.Translate(h, 0, 0);
			}
			if(GeneralSettings.verticalBool)
			{
				transform.Translate(0, v, 0);
			}
			if(GeneralSettings.verticalBool && GeneralSettings.horizontalBool)
			{
				transform.Translate(h, v, 0);
			//	print("horizontal x: "+h+" vertical y: "+v);
			}
			else{transform.Translate(0, 0, 0);}
		}
		
		//hand motion range
		xmin = BorderL.transform.position.x; 
		xmax = BorderR.transform.position.x;	
		ymin=BorderD.transform.position.y;
		ymax=BorderU.transform.position.y;	
		//horizontal
		if(transform.position.x > xmax)//right
		{
			transform.position = new Vector3(xmax, transform.position.y, transform.position.z);
		}

		if (transform.position.x < xmin)//left
		{
			transform.position = new Vector3(xmin, transform.position.y, transform.position.z);
		}
		//Apply vertical movement based on the tiles range
		if (transform.position.y >  ymax)//up
		{
			transform.position = new Vector3(transform.position.x, ymax, transform.position.z);
		}
		if (transform.position.y < ymin)//down
		{
			transform.position = new Vector3(transform.position.x, ymin, transform.position.z);
		}
		
	}
	
	  IEnumerator CalibrateMouse()
    	{
			calibLoop = false;
			print("Calibrating...");
		XmlDataWriter.isCalibrating = true;
            yield return new WaitForSeconds(10);
			UDPReceive.calibrate=false;//stop calibration
		XmlDataWriter.isCalibrating = false;
			print("Calibration finished!");
		//	Time.timeScale = 0.0f;
   	 	}

	
	
	void OnGUI () 
	{
	//	GUI.color = Color.yellow;
	//	GUI.Label (new Rect ((Screen.width/2)-50, 20, 120, 80), "Device: ");
		GUI.Label (new Rect ((Screen.width)-100, 30, 150, 80), DevIcon, DeviceStyle);
		
//		//display on game pause
//		if(GeneralSettings.isPaused)
//		{
//		GUI.BeginGroup (new Rect (Screen.width/2 - 200, (Screen.height/2)+50, 250, 200));
//		GUI.Box (new Rect (0,0,250,200), "Mouse Calibration");
//		GUI.Label(new Rect(20, 20, 200, 20), "x max: "+xmax+"   x min: "+xmin);
//		GUI.Label(new Rect(20, 40, 200, 20), "y max: "+ymax+"   y min: "+ymin);
//		GUI.Label(new Rect(20, 90, 200, 20), "Scale x: "+scalex);
//		GUI.Label(new Rect(20, 110, 200, 20), "Scale y: "+scaley);
//			calibrate = GUI.Toggle(new Rect(30, 160, 100, 30), calibrate, "Calibrate");
//		GUI.EndGroup ();
//		}
		
	}
	
	
}
