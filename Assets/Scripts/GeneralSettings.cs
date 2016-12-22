using UnityEngine;
using System.Collections;
using System.Threading;

public class GeneralSettings : MonoBehaviour {
	
	public static string device {get; set;}//Gets/Sets the type of device (e.g. Mouse, Keyboard...etc.)
//	public static bool Vertical {get; set;}

	public static float Orient {get; set;}//Series orientation
	
	//L/R Shoulder to apply transformations
	public GameObject LeftShoulder;
	public GameObject RightShoulder;
	
	public GUIStyle pauseBg;
	
	//L/R hand textures
	public Texture LHand;
	public Texture RHand;
	public Texture timericon;
	public Texture saveicon;
	public Texture mouseicon;
	public Texture keybicon;
	public GUIStyle handStyle;
	
	public static bool timerOn=true;//selection timer
	
	public static bool cursorVis = false;//cursor visibility
	
	public static string arch = "";//type of architecture (e.g. Grid or Series)
	
	public static bool isPaused = false; //Pause screen
	public static bool settings = false; //Pause screen
	
	public GUIStyle guiStyle;
	
	public static bool horizontalBool = true;
	public static bool verticalBool = true;
	
	public static float HandSpeed = 0.01f;
	public string stringHandSpeed = " ";
	
	public string stringTileSpeed = " ";
	public static float TileSpeed = 0.5f;
	
	public static bool MouseSet = false;
	
	//public static bool writexml = false;
	
	
	// Use this for initialization
	void Start () 
	{
		stringHandSpeed = HandSpeed.ToString(); 
	    stringTileSpeed = TileSpeed.ToString();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Screen.showCursor = cursorVis; //cursor visibility
		
//		if(UDPReceive.borderHit == GameObject.FindGameObjectWithTag(UDPReceive.borderHit).name)
//		{
//			GameObject.FindGameObjectWithTag(UDPReceive.borderHit).SetActive(false);
//		}
//		else
//		{
//			GameObject.FindGameObjectWithTag(UDPReceive.borderHit).SetActive(true);
//		}
		
//		if(UDPReceive.calibrate && !MouseSet)
//		{
//			MouseSettings.MoveMouse(Screen.width/2, Screen.height/2);
//			MouseSettings.SetMouseSpeed(1);
//			MouseSet=true;
//		}
//		if(!UDPReceive.calibrate && MouseSet)
//		{
//			MouseSettings.SetMouseSpeed(MouseSettings.InitSpeed());
//			MouseSet=false;
//		}
//		
//		if(!isPaused || !settings)
//		{
//			MouseSettings.SetMouseSpeed(1);
//		}
//		else
//		{
//			MouseSettings.SetMouseSpeed(MouseSettings.InitSpeed());
//		}
		
		
		//show cursor while pressing C
		if(Input.GetKey(KeyCode.C))
		{
			cursorVis=true;
		}
		else{cursorVis=false;}
		
		
		//End Game
		if(Input.GetKeyDown(KeyCode.F))
	   	{
			if(InitArguments.arg!="nolog")
			{	
				#if UNITY_STANDALONE
				XmlDataWriter.stopXML=true;
				XmlDataWriter.endXML();
				#endif
			}
			
			#if UNITY_STANDALONE
			MouseSettings.SetMouseSpeed(MouseSettings.InitSpeed());
			#endif
			
			Thread.Sleep(100);
	     	Application.LoadLevel(2);
	  	}
		
		//Pause/Resume Game
		if(Input.GetKeyDown(KeyCode.S) && !isPaused)
	   	{
	      print("Paused");
	      Time.timeScale = 0.0f;
	      isPaused = true;
	  	 }
	   else if(Input.GetKeyDown(KeyCode.S) && isPaused)
	  	 {
	      print("Resumed");
	      Time.timeScale = 1.0f;
	      isPaused = false; 
		
		#if UNITY_STANDALONE	
		  //center mouse cursor
		  MouseSettings.MoveMouse(Screen.currentResolution.width/2, Screen.currentResolution.height/2);
		#endif	
		  //center IK target
		float xDiff = (GameObject.FindGameObjectWithTag("ArrowR").transform.position.x)-(GameObject.FindGameObjectWithTag("ArrowL").transform.position.x);
		float yDiff = (GameObject.FindGameObjectWithTag("ArrowU").transform.position.y)-(GameObject.FindGameObjectWithTag("ArrowD").transform.position.y);
		  GameObject.FindGameObjectWithTag("IK").transform.position = new Vector3 (xDiff, yDiff, GameObject.FindGameObjectWithTag("IK").transform.position.z);
	  	 }
		
		//reset cursor and hand position
		if(Input.GetKeyDown(KeyCode.R))
	   	{
			#if UNITY_STANDALONE
			//center mouse cursor
		 	MouseSettings.MoveMouse(Screen.currentResolution.width/2, Screen.currentResolution.height/2);
			#endif	
		 	//center IK target
			float xDiff = (GameObject.FindGameObjectWithTag("ArrowR").transform.position.x)-(GameObject.FindGameObjectWithTag("ArrowL").transform.position.x);
			float yDiff = (GameObject.FindGameObjectWithTag("ArrowU").transform.position.y)-(GameObject.FindGameObjectWithTag("ArrowD").transform.position.y);
		 	GameObject.FindGameObjectWithTag("IK").transform.position = new Vector3 (xDiff, yDiff, GameObject.FindGameObjectWithTag("IK").transform.position.z);
		}
		
		//write xml data toggle button
		//if(writexml){XmlDataWriter.XmlInit();}
		
		
	}
	

	
	void OnGUI () 
	{
		
	if(!Init.mainscreen)	
		{
			
		#if !UNITY_ANDROID
		GUI.Label(new Rect(10, 10, 250, 20), "Press 's' for settings", guiStyle);
		GUI.Label(new Rect(10, 40, 150, 20), "Press 'f' to finish...", guiStyle);
		#endif
		
		#if UNITY_ANDROID
		if (GUI.Button(new Rect(20, 20, 60, 60), "Settings")&& !isPaused)
		{
		  print("Paused");
	      Time.timeScale = 0.0f;
	      isPaused = true;
		}
		#endif
		

		//Game settings
		if(isPaused)
		{
			Time.timeScale = 0.0f;
	    	GUI.Box(new Rect(0, 0, Screen.width, Screen.height), " ", pauseBg);
//			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), " ");
//			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), " ");
//			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), " ");
			
			cursorVis=true;//cursor gets visible
					
			//KLP Scoring board
//			GUI.BeginGroup (new Rect (Screen.width/2-10 , Screen.height-150, 120, 120));
//			GUI.color = Color.blue;
//			GUI.Box (new Rect (0,0,120,120), "KLM Score");
//			GUI.color = Color.white;
//			GUI.Label(new Rect(10, 20, 80, 25), "Correct: " + GameScoring.GetCorrect());
//			GUI.Label(new Rect(10, 40, 80, 25), "Wrong: " + GameScoring.GetError());
//			GUI.Label(new Rect(10, 60, 80, 25), "Omisions: " + GameScoring.GetOmissions());
//			GUI.Label(new Rect(10, 90, 80, 55), "Final: " + GameScoring.GetKLT()+ " %");
//			GUI.EndGroup ();
		
		//resume game
		GUI.color = Color.green;		
		if (GUI.Button(new Rect(Screen.width/2-40, Screen.height/2-80, 80, 80), "Play"))
		{
	      print("Resumed");
	      Time.timeScale = 1.0f;
	      isPaused = false; 
			
		#if UNITY_STANDALONE			
		  //center mouse cursor
		  MouseSettings.MoveMouse(Screen.currentResolution.width/2, Screen.currentResolution.height/2);	
		#endif
		  //center IK target
		float xDiff = (GameObject.FindGameObjectWithTag("ArrowR").transform.position.x)-(GameObject.FindGameObjectWithTag("ArrowL").transform.position.x);
		float yDiff = (GameObject.FindGameObjectWithTag("ArrowU").transform.position.y)-(GameObject.FindGameObjectWithTag("ArrowD").transform.position.y);
		  GameObject.FindGameObjectWithTag("IK").transform.position = new Vector3 (xDiff, yDiff, GameObject.FindGameObjectWithTag("IK").transform.position.z);
	  						
		}	
		GUI.color = Color.white;	
			
			//enable/disable timer
			GUI.color = Color.yellow;
			GUI.Label (new Rect (250, 320, 120, 80), "Timer Activation");
			GUI.color = Color.white;
		    timerOn = GUI.Toggle(new Rect(260, 350, 60, 60), timerOn, timericon);
			

			//Hand Movement Orientation
			GUI.color = Color.yellow;
			GUI.Label (new Rect (50, 320, 120, 80), "Enabled Axis");
			GUI.color = Color.white;
			horizontalBool = GUI.Toggle (new Rect (55, 355, 80, 30), horizontalBool, "Horizontal");//Horizontal slider and gain adjustment	
			verticalBool = GUI.Toggle (new Rect (55, 395, 80, 30), verticalBool, "Vertical");//Vertical slider and gain adjustment
			
			//Hand Speed (IK Target)
			GUI.color = Color.yellow;
			GUI.Label (new Rect (50, 20, 120, 80), "Hand Speed");
			GUI.color = Color.white;
			//TextField
		//	stringHandSpeed = GUI.TextField(new Rect(180, 50, 40, 20), stringHandSpeed, 25);
			//Slider
		//	HandSpeed = GUI.VerticalSlider (new Rect (120, 30, 100, 60), float.Parse(stringHandSpeed), 0.1f, 0.001f);
			GUI.Label (new Rect (65, 40, 120, 80), HandSpeed.ToString("0.000"));
			
			GUI.color = Color.yellow;
			GUI.Label (new Rect (200, 20, 120, 80), "Hand Speed Presets");
			GUI.color = Color.red;
			if(GUI.Button (new Rect (150, 50, 30, 20), new GUIContent("<|||", "Very Slow"))){HandSpeed=0.001f;}//very slow
			if(GUI.Button (new Rect (200, 50, 30, 20), new GUIContent("<||", "Slow"))){HandSpeed=0.002f;}//slow
			if(GUI.Button (new Rect (250, 50, 30, 20), new GUIContent("|", "Medium"))){HandSpeed=0.01f;}
			if(GUI.Button (new Rect (300, 50, 30, 20), new GUIContent("||>", "Fast"))){HandSpeed=0.02f;}//fast
			if(GUI.Button (new Rect (350, 50, 30, 20), new GUIContent("|||>", "Very Fast"))){HandSpeed=0.03f;}//very fast
			GUI.color = Color.white;
			GUI.Label(new Rect(240, 100, 120, 40), GUI.tooltip);
			//ToDo: Add min/max labels + horizontal sliders
			
			
			//Tile Speed
			GUI.color = Color.yellow;
			GUI.Label (new Rect (50, 160, 120, 80), "Tile Speed");
			GUI.color = Color.white;
			//TextField
	//		stringTileSpeed = GUI.TextField(new Rect(180, 150, 40, 20), stringTileSpeed, 25);
			//Slider
	//		TileSpeed = GUI.VerticalSlider (new Rect (120, 130, 100, 60), float.Parse(stringTileSpeed), 10.0f, 0.1f);
			GUI.Label (new Rect (60, 190, 120, 80), TileSpeed.ToString("0.000"));
			
			GUI.color = Color.yellow;
			GUI.Label (new Rect (200, 160, 180, 80), "Tile Speed Presets");
			GUI.color = Color.red;
			if(GUI.Button (new Rect (150, 200, 30, 20), new GUIContent("<|||", "Very Slow"))){TileSpeed=0.1f;}//very slow
			if(GUI.Button (new Rect (200, 200, 30, 20), new GUIContent("<||", "Slow"))){TileSpeed=0.25f;}//slow
			if(GUI.Button (new Rect (250, 200, 30, 20), new GUIContent("|", "Medium"))){TileSpeed=0.5f;}
			if(GUI.Button (new Rect (300, 200, 30, 20), new GUIContent("||>", "Fast"))){TileSpeed=0.75f;}//fast
			if(GUI.Button (new Rect (350, 200, 30, 20), new GUIContent("|||>", "Very Fast"))){TileSpeed=0.90f;}//very fast
			GUI.color = Color.white;
			GUI.Label(new Rect(240, 100, 250, 40), GUI.tooltip);
				

//		//Select device: mouse, keyboard, tracking
//		GUI.BeginGroup(new Rect( Screen.width/2, 10, Screen.width-10, Screen.height-10));
//		GUI.color = Color.yellow;
//        GUI.Box(new Rect(0, 0, 150, 150), "Choose Device");
//		GUI.color = Color.white;
//				
//		if (GUI.Button(new Rect(10, 30, 60, 60), mouseicon))
//		{
//			GeneralSettings.device = "RawMouse";
//				UDPReceive.udppos = Vector3.zero;
//				clearLists();//clear lists			
//			  Movement.calibLoop = true;//calibration
//					
//			//Adjust filter
//			UDPReceive.ALPHA = 0.30f;
//			UDPReceive.deltaon = false;
			  
//            Debug.Log("Mouse");
//		}
//        
//        if (GUI.Button(new Rect(80, 30, 60, 60), keybicon))
//		{
//			GeneralSettings.device = "Keyboard";
//            Debug.Log("Keyboard");
//		}
			
//		if (GUI.Button(new Rect(40, 110, 80, 30), "Raw Mouse"))
//		{
//			GeneralSettings.device = "RawMouse";
//            Debug.Log("Raw Mouse");	
//		}
			
//		GUI.EndGroup();				

				}//if settings	


			
		#if UNITY_ANDROID
		if (GUI.Button(new Rect(100, Screen.height-80, 60, 60), "Play")&& isPaused)
		{
	      print("Resumed");
	      Time.timeScale = 1.0f;
	      isPaused = false; 
		}
		if (GUI.Button(new Rect(Screen.width-100, Screen.height-80, 60, 60), "Finish"))
		{
			Thread.Sleep(100);
	     	Application.LoadLevel(2);
		}
		#endif			
			
			//Type of Device
//			GUI.Label (new Rect ((Screen.width)-100, 30, 150, 80), DevIcon, DeviceStyle);
		
		
			
			
		//Log XML data toggle button
		//writexml = GUI.Toggle (new Rect (Screen.width-450, Screen.height/2+10, 80, 30), writexml, "Log Data");
	
			
			
//		GUI.BeginGroup(new Rect( 130, (Screen.height/2)+100, 150, 90));
//		GUI.color = Color.yellow;
//        GUI.Box(new Rect(0, 0, 150, 90), "Switch Hands");
//		GUI.color = Color.white;
//		//enable left arm IK
//		if(GUI.Button (new Rect (10, 30, 60, 60), LHand, handStyle))
//		{
//			ikLimbLeft.IsEnabled = true;
//			ikLimbRight.IsEnabled = false;
//			RightShoulder.transform.Rotate(new Vector3(0,-180,0)); //return arm to initial position and out of the camera view
//		}
//		//enable right arm IK
//		if(GUI.Button (new Rect (80, 30, 80, 60), RHand, handStyle))
//		{
//			ikLimbRight.IsEnabled = true;
//			ikLimbLeft.IsEnabled = false;
//			LeftShoulder.transform.Rotate(new Vector3(0,-180,0)); //return arm to initial position and out of the camera view
//		}	
//		GUI.EndGroup();	
			
			
//		#if UNITY_STANDALONE	
//		//SAVE/LOAD Settings
//			if (GUI.Button(new Rect((Screen.width/2)-10, Screen.height/2-120, 60, 60), saveicon))
//	        {
//				SaveConfig.WriteToXml();
//			//	Debug.Log("Saved...");
//			}
//			
//			//last save date label
//			GUI.Label(new Rect((Screen.width/2)-60, (Screen.height/2)-50, 400, 20), "Last Save: "+SaveConfig.savetime);
//			
//				
//			if (GUI.Button(new Rect((Screen.width/2)-10, (Screen.height/2)+80, 50, 30), "Load"))
//	        {
//				SaveConfig.LoadFromXml();
//			//	Debug.Log("Loaded...");
//			}
//						
//			
//			
//			
//			//Stop XML Logging
//			if(GUI.Button (new Rect ((Screen.width/2)-60, (Screen.height/2)-100, 60, 60), "Stop Logging"))
//			{
//				XmlDataWriter.stopXML=true;
//			}
//			
//				
//		#endif	
			
		}//end if paused
			
					
	}
	
	 void clearLists()
	{	   
	  UDPReceive.xvalues.Clear();
	  UDPReceive.yvalues.Clear();
	  UDPReceive.zvalues.Clear();
	  UDPReceive.xmax= 0; UDPReceive.xmin = 0;
	  UDPReceive.ymax= 0; UDPReceive.ymin = 0;
	  UDPReceive.zmax= 0; UDPReceive.zmin = 0;
	}
	
}
