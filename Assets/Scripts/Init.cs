using UnityEngine;
using System.Collections;

public class Init : MonoBehaviour {
	
	public GameObject FB;
	
	public static bool isGrid = false;
	public static bool isHSerial = false;
	public static bool isVSerial = false;
	
//	public GameObject LeftShoulder;
//	public GameObject RightShoulder;
	
	public Texture LHand;//left hand texture
	public Texture RHand;//right hand texture
	public Texture Logo1;//left logo texture
	public Texture Logo2;//right logo texture
	public Texture MouseIcon;
	public Texture KeybIcon;
	public Texture GridIcon;
	public Texture HorzIcon;
	public Texture arrowgradient;
	public Texture tptLogo;
	
	public GUIStyle handStyle;//hands style
	public GUIStyle textStyle;//text style
	
	//GUI enable/disable flags
	public bool gamegui = true;
    public bool devicegui = false;//change to false if the device gui is appearing
    public bool handgui = true;
	
	public static string dev = "";//type of device for logging
	
	public static string uid {get; set;}
	
	public GameObject Args;
	
	public static bool mainscreen = true;
	
	public static string Hand;
	
	void Awake()
	{
		#if UNITY_STANDALONE
		FB.SetActive(false);
		DontDestroyOnLoad(Args);
		#endif
	
		#if UNITY_WEBPLAYER
		Args.SetActive(false);
		DontDestroyOnLoad(FB);
		#endif
	}

	// Use this for initialization
	void Start () 
	{
		uid = "";
		gamegui=false;
		

	}
	
	// Update is called once per frame
	void Update () 
	{
		#if UNITY_STANDALONE
		if(Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();	
		}
		#endif
		
	}
	
	//enable play button when all options are set.
	bool enablePlay()
	{
	//	if (handgui || devicegui || gamegui || uid=="")
		#if UNITY_STANDALONE
		if (handgui || gamegui || uid=="")
		#elif UNITY_WEBPLAYER	
		if (handgui || !InteractiveConsole.loggedin)
    	#endif
			return false;
		else 
			return true;
	}
	
	void OnGUI () 
	{
		
	if(mainscreen)
		{	
		//TPT Logo
		#if UNITY_STANDALONE
//     	GUI.Label (new Rect ((Screen.width/2) - tptLogo.width/2, 20, 200, 200), tptLogo, handStyle);
    	#endif

		
		#if UNITY_WEBPLAYER
//      	GUI.Label (new Rect ((Screen.width/2)-60 , 20, 100, 100), tptLogo, handStyle);
    	#endif
		
		//Arrow gradient
//		GUI.Label (new Rect ((Screen.width / 2) - 350,(Screen.height/2)-220, 220, 550), arrowgradient, handStyle);
		
		//neurorehablab logo
		GUI.Label (new Rect (0, (Screen.height)-Logo1.height+60, 200, 200), Logo1, handStyle);
		GUI.Label (new Rect ((Screen.width)-Logo2.width+110, (Screen.height)-Logo2.height+60, 400, 200), Logo2, handStyle);
		
		#if UNITY_STANDALONE
		//Patient ID field
		GUI.Label (new Rect ((Screen.width / 2) - 30, (Screen.height/2)-220, 100, 30), "ID", textStyle);
		uid = GUI.TextField(new Rect((Screen.width / 2) - 120, (Screen.height/2)-180, 200, 20), uid, 25);
		#endif
		
////////////////////////////////////////////////////////////////////////////////////////////////////		
		//Select Game Design: Grid vs Serial
/*			
		GUI.BeginGroup(new Rect((Screen.width / 2) - 90, (Screen.height/2)-100, 150, 90));
//		GUI.BeginGroup(new Rect((Screen.width / 2) - 90, (Screen.height/2)-110, 150, 90));
        GUI.Box(new Rect(0, 0, 150, 90), "2. Select Design", textStyle);
		GUI.enabled = gamegui;//enable/disable GUI
		if (GUI.Button(new Rect(10, 30, 60, 50), GridIcon, handStyle))
		{
			isGrid = true;
            Debug.Log("Grid");
			dev = "Grid";//type of device for logging
			gamegui=false;//disable GUI after selection
		}
        
        if (GUI.Button(new Rect(80, 30, 60, 50), HorzIcon, handStyle))
		{
			isHSerial = true;
            Debug.Log("Horizontal Serial");
			dev = "Horizontal";//type of device for logging
			gamegui=false;//disable GUI after selection
		}
		
		 if (GUI.Button(new Rect(80, 60, 60, 20), "Vertical"))
		{
			isVSerial = true;
            Debug.Log("Vertical Serial");
			dev = "Vertical";//type of device for logging
			gamegui=false;//disable GUI after selection
		}
		
		GUI.enabled = true;
		GUI.EndGroup();	
*/
			
////////////////////////////////////////////////////////////////////////////////////////////////////		
//		//Select device: mouse, keyboard, tracking
//		GUI.BeginGroup(new Rect((Screen.width / 2) - 90, (Screen.height/2)+20, 150, 90));
//        GUI.Box(new Rect(10, 0, 150, 90), "3. Select Device", textStyle);
//		GUI.enabled = devicegui;//enable/disable GUI
//		if (GUI.Button(new Rect(10, 30, 60, 50), MouseIcon, handStyle))
//		{
//		//	Movement.device = "Mouse";
//			GeneralSettings.device = "Mouse";
//            Debug.Log("Mouse");
//			
//			devicegui=false;//disable GUI after selection
//		}
//        
//        if (GUI.Button(new Rect(80, 30, 60, 50), KeybIcon, handStyle))
//		{
//		//	Movement.device = "Keyboard";
//			GeneralSettings.device = "Keyboard";
//            Debug.Log("Keyboard");
//			
//			devicegui=false;//disable GUI after selection
//		}
//		GUI.enabled = true;
//		GUI.EndGroup();
		
////////////////////////////////////////////////////////////////////////////////////////////////////		
		//Hand Selection
		GUI.BeginGroup(new Rect((Screen.width / 2) - 100, (Screen.height/2)-100, 150, 100));
	//	GUI.BeginGroup(new Rect((Screen.width / 2) - 100, (Screen.height/2)+160, 150, 90));
        GUI.Box(new Rect(30, 0, 150, 100), "Select Hand", textStyle);
		//enable left arm IK
		GUI.enabled = handgui;//enable/disable GUI
		if(GUI.Button (new Rect (10, 40, 60, 60), LHand, handStyle))
		{
			ikLimbLeft.IsEnabled = true;
			ikLimbRight.IsEnabled = false;
				Hand="Left";
		//	RightShoulder.transform.Rotate(new Vector3(0,-180,0)); //return arm to initial position and out of the camera view
			Debug.Log("Left Hand");
			
			handgui=false;//disable GUI after selection
		}
		//enable right arm IK
		if(GUI.Button (new Rect (100, 40, 60, 60), RHand, handStyle))
		{
			ikLimbRight.IsEnabled = true;
			ikLimbLeft.IsEnabled = false;
				Hand="Right";
		//	LeftShoulder.transform.Rotate(new Vector3(0,-180,0)); //return arm to initial position and out of the camera view
			Debug.Log("Right Hand");
			
			handgui=false;//disable GUI after selection
		}
		GUI.enabled = true;
		GUI.EndGroup();
		
////////////////////////////////////////////////////////////////////////////////////////////////////		
		//Play Game!
		GUI.enabled = enablePlay();
//		if (GUI.Button(new Rect((Screen.width / 2) - 65, (Screen.height/2)+310, 100, 80), "PLAY!"))
		if (GUI.Button(new Rect((Screen.width / 2) - 65, (Screen.height/2)+100, 100, 80), "PLAY!"))	
		{
			GeneralSettings.isPaused=true;//start game paused
			GeneralSettings.device = "n/a";
				
			isGrid = true;
			dev = "Grid";//type of device for logging
				
			Application.LoadLevel(1);
			mainscreen=false;
				
			#if UNITY_WEBPLAYER && !UNITY_EDITOR
			Screen.fullScreen = true;
			#endif	

		//	SaveConfig.LoadFromXml();//load last saved settings
		}
		GUI.enabled = true;
	}
		
	}//if mainscreen	
	
}
