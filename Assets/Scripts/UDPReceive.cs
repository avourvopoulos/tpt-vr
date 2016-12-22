using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
#if UNITY_STANDALONE
using System.Diagnostics;
#endif

public class UDPReceive : MonoBehaviour {
	
 // receiving Thread
    Thread receiveThread; 
 // udpclient object
    UdpClient client; 
	
	public GUIStyle calbText;

    public static List<string> datatypelst = new List<string>();
	public static List<string> devicelst = new List<string>();
	public static List<string> jointslst = new List<string>();
	public static List<string> transformTypelst = new List<string>();
	public static List<string> emulst = new List<string>();
	
	public static string datatype;
	public static string device;
	public static string joint;
	public static string transformationtype;
	public static float udpx, udpy, udpz, udpw;
//	public static float x, y, z, w = 0.0f;
 
    // public
    public int port; // define > init
	private string portField = "1202";
	public static bool isConnected=false;
	
//	private string text = String.Empty;
	
	public static string selection = "n/a";
	public static string udpDev = "n/a";
	public static bool tracking = false;	
	
//	public static float xmax = Screen.currentResolution.width/2;
//	public static float	xmin = Screen.currentResolution.width/2;
//	public static float ymax = Screen.currentResolution.height/2;
//	public static float ymin = Screen.currentResolution.height/2;
	public static float xmax, xmin, ymax, ymin, zmax, zmin;
	
	private float axisH = 0.0f;
	private float axisV = 0.0f;
	
	public static float dirH;
	public static float dirV;
	
	public static Vector3 udppos;
	public static Quaternion udprot;
	public GameObject target;//IK target
	
	public static List<float> xvalues = new List<float>();
	public static List<float> yvalues = new List<float>();
	public static List<float> zvalues = new List<float>();
	public static float scalex, scaley, scalez = 0.0f;
	public static bool calibrate = false;
	
	public GUIStyle txtstyle, bgstyle;
	
	public static float ALPHA = 0.90f;
	//float[] xin, xout = new float[10];//x in/out
	//float[] yin, yout = new float[10];//y in/out
	public float[] n_input = new float[2];//xy in/out
	public float[] n_output = new float[2];//xy in/out
	
//	public Transform start;
	Vector3 FilteredPosition;
	Vector3 NewPosition;
//	public float smooth = 5.0f;//smoothing for lerp
	public static float delta;
	public static bool deltaon = true;
	
	bool yon = true;
	bool zon = false;
	
	public static string rawdata;
	
	public static bool emulate = false;
	public Vector2 scrollPositiontrc = Vector2.zero;
	public Vector2 scrollPositionbtn = Vector2.zero;
	
	//calibration window
	public static bool activeWin = false;
	public static bool activeNetWin = false;
	public static bool activeDevWin = false;
	public Rect windowRect0 = new Rect(30, 150, 290, 400);//posx, posy, width, height - filtering
	public Rect windowRect1 = new Rect(30, 150, 200, 320);//posx, posy, width, height - networking
	public Rect windowRect2 = new Rect(30, 150, 240, 300);//posx, posy, width, height - devs
	bool activewin=false;
	
	public static int calibrationTime = 10;
	
	public static float rangemaxx, rangemaxy, rangeminx, rangeminy = 0.0f;
	
	bool MouseSet=false;
	
	bool resetLists = false;
	
	int windowHeight = 0;
	
	public static string borderHit=" ";
	
	float timer = 5;
	
	bool eyetrack = false;
	
	void Awake()
	{
//		rangemaxx = GameObject.FindGameObjectWithTag("ArrowR").transform.position.x;
//		rangemaxy = GameObject.FindGameObjectWithTag("ArrowU").transform.position.y;
//		rangeminx = GameObject.FindGameObjectWithTag("ArrowL").transform.position.x;
//		rangeminy = GameObject.FindGameObjectWithTag("ArrowD").transform.position.y;		
	}
			
    void Start()
    {		
		init();		
		isConnected = true;
    }
	
	void FixedUpdate()
	{
		if(Input.GetKeyDown(KeyCode.F))
		{
			isConnected = false;
			receiveThread.Abort();
			client.Close();
			clearLists();
			print("Stop UDP");
			tracking = false;
			calibrate = false;
		}
	}
	
	void Update () 
	{
		
		//update available data list
//		if(GeneralSettings.isPaused)
//		{
//			timer -= Time.deltaTime;
//			if(timer <= 0)
//			{
//			  devicelst.Clear();
//			  timer = 5;
//			Debug.Log(" devicelst.Clear()");
//			}
////		}
		
		if(Input.GetKeyDown(KeyCode.C))
		{calibrate=false; Movement.calibLoop = false;}
		
		if(Input.GetKeyDown(KeyCode.L))
		{clearLists();}
		
			//apply Time.deltaTime to the lowpass filter
			if(deltaon)
			{
				delta = Time.deltaTime;
			}
			else
			{
				delta = 1.0f;
			}
			//use y or z coordinates for the y axis
			if(yon)
			{
				zon=false;
			}
			else
			{
				yon=false;
				zon=true;
			}
		
		if(tracking)
	    {
				
			if(calibrate)	
			{
				#if UNITY_STANDALONE
				//mouse settings
				if(!MouseSet)
				{
					MouseSettings.MoveMouse(Screen.currentResolution.width/2, Screen.currentResolution.height/2);
					MouseSettings.SetMouseSpeed(1);
					MouseSet=true;
					print("Mouse to slow speed");
					
					resetLists = true;
				}
				#endif
				
				//XYZ MIN/MAX
				if(udppos.x !=0)//x
				{
					xvalues.Add(-udppos.x);
				
					xvalues.Sort();
					xmin = xvalues[0];
					xmax = xvalues[xvalues.Count-1];
				}
				
				if(udppos.y !=0)//y
				{
					yvalues.Add(udppos.y);
				
					yvalues.Sort();
					ymin = yvalues[0];
					ymax = yvalues[yvalues.Count-1];
				}	
				
				if(udppos.z !=0)//z
				{
					zvalues.Add(-udppos.z);
				
					zvalues.Sort();
					zmin = zvalues[0];
					zmax = zvalues[zvalues.Count-1];
				}
				
			//	calstatus = calibrate;
				
				
				if(resetLists)
				{
				//	Thread.Sleep(2000);
					clearLists();
					print("clearLists");
					resetLists = false;
				}
				
				
			}//end calibrate
			
			#if UNITY_STANDALONE
			else if (!GeneralSettings.isPaused)
			{
				MouseSettings.SetMouseSpeed(1);
			}
			
			else 
				{
					MouseSettings.SetMouseSpeed(MouseSettings.InitSpeed());
					MouseSet=false;
//					print("Mouse back to Init Speed");
				}
			#endif
			
		rangemaxx = GameObject.FindGameObjectWithTag("ArrowR").transform.position.x;
		rangemaxy = GameObject.FindGameObjectWithTag("ArrowU").transform.position.y;
		rangeminx = GameObject.FindGameObjectWithTag("ArrowL").transform.position.x;
		rangeminy = GameObject.FindGameObjectWithTag("ArrowD").transform.position.y;
			
			
//	if(!GeneralSettings.isPaused || !calibrate)
//			{
//			//constrain mouse cursor
//			if(-udppos.x > xmax){MouseSettings.MoveMouse((int)xmax, (int)udppos.y);}
//			else if(-udppos.x < xmin){MouseSettings.MoveMouse((int)xmin, (int)udppos.y);}
//			else{udppos.x = udppos.x;}
//				
//			if(udppos.y > ymax){MouseSettings.MoveMouse((int)-udppos.x, (int)ymax);}
//			else if(udppos.y < ymin){MouseSettings.MoveMouse((int)-udppos.x, (int)ymin);}
//			else{udppos.y = udppos.y;}
//			}
			
			//scale raw data between -1 to 1
			//new_value = ( (old_value - old_min) / (old_max - old_min) ) * (new_max - new_min) + new_min
			scalex = ((-udppos.x - xmin)/(xmax-xmin)) * (rangemaxx - (rangeminx)) + (rangeminx);
			scaley = ((udppos.y - ymin)/(ymax-ymin)) * (rangemaxy - (rangeminy)) + (rangeminy);
			scalez = ((-udppos.z - zmin)/(zmax-zmin)) * (rangemaxy - (rangeminy)) + (rangeminy);
			
			//borders x
			if(scalex >= rangemaxx)
			{
				scalex=rangemaxx;
				borderHit="ArrowR"; //UnityEngine.Debug.Log(borderHit);
				if(!GeneralSettings.isPaused || !calibrate)
				{
				//	MouseSettings.MoveMouse(xmax, udppos.y);
				}
			}
			else if(scalex <= rangeminx)
			{
				scalex=rangeminx;
				borderHit="ArrowL"; //UnityEngine.Debug.Log(borderHit);
				if(!GeneralSettings.isPaused || !calibrate)
				{
				//	MouseSettings.MoveMouse(xmin, udppos.y);
				}
			}
			else{scalex=scalex;}
			
			//borders y
			if(scaley >= rangemaxy)
			{
				scaley=rangemaxy;
				borderHit="ArrowU"; //UnityEngine.Debug.Log(borderHit);
				if(!GeneralSettings.isPaused || !calibrate)
				{
				//	MouseSettings.MoveMouse(udppos.x, ymax);
				}
			}
			else if(scaley <= rangeminy)
			{
				scaley=rangeminy;
				borderHit="ArrowD"; //UnityEngine.Debug.Log(borderHit);
				if(!GeneralSettings.isPaused || !calibrate)
				{				
				//	MouseSettings.MoveMouse(udppos.x, ymin);
				}
			}
			else{scaley=scaley;}
			
			//borders z
			if(scalez >= rangemaxy){scalez=rangemaxy;}
			else if(scalez <= rangeminy){scalez=rangeminy;}
			else{scalez=scalez;}
			
			//put last x,y coordinates to an array in order to feed the tracking to the low pass filter
			n_input[0]=scalex;
			//option for y or z axis to act like y.
			if(yon)
			{n_input[1]=scaley;}//y
			else
			{n_input[1]=scalez;}//z
			
			n_output[0] = lowPass(n_input, n_output)[0];
			n_output[1] = lowPass(n_input, n_output)[1];
			
			//lowPass(n_input, n_output); //[0]for x, [1]for y
			FilteredPosition = new Vector3(lowPass(n_input, n_output)[0], lowPass(n_input, n_output)[1], 0);
	//		print("FilteredPosition: "+FilteredPosition);
				
			NewPosition = new Vector3(FilteredPosition.x, FilteredPosition.y, target.transform.position.z);
						
			//apply position to target
		if (float.IsNaN(NewPosition.x)==false && float.IsNaN(NewPosition.y)==false){	
			target.transform.position =	NewPosition;	
			}

		}//end tracking
		if(eyetrack)
		{
			target.transform.position =	new Vector3(-udppos.x, udppos.y, target.transform.position.z);	
		//	print("raw: "+udppos.x + udppos.y);
		}
		
	}
	
	//low pass filter x,y
	float[] lowPass( float[] input, float[] output ) 
	{
     for(int i=0; i<input.Length; i++ ) 
		{
			 if( output == null || output.Length == 0 || float.IsNaN(output[i]) || float.IsInfinity(output[i]) || output[i] == float.NaN)
			 {
			//	return input;
				output[i] = input[i];
			 }
			 else
			{
				output[i] = output[i] + ALPHA * (input[i] - output[i]) * delta;
			//	output[i] = (1-ALPHA)* input[i] + (ALPHA*output[i]); //ToDo: include a deltatime relative to ALPHA
				
			}
    	}
    		return output;
	}

	
//	public float InputGetAxis(string axis)
//	{
//	    var v = Input.GetAxis(axis);
//	    if (Mathf.Abs(v) > 0.005)return v;
//	    if (axis=="Horizontal")return axisH;
//	    if (axis=="Vertical") return axisV;
//		else return 0;
//	}
	
	
    public void init()
    {
		
        // Local endpoint define (where messages are received).
        // Create a new thread to receive incoming messages.
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;	
        receiveThread.Start();
    }
 

    // receive thread 
    public void ReceiveData() 
    {	
		port = int.Parse(portField);
		
		client = new UdpClient(port);
		print("Started UDP - port: " +port);
		
        while (isConnected)
        {
            try 
            { 
              	 // receive Bytes from 127.0.0.1
					IPEndPoint IP = new IPEndPoint(IPAddress.Loopback, 0);

        	        byte[] udpdata = client.Receive(ref IP);
				
                //  UTF8 encoding in the text format.
					string data = Encoding.UTF8.GetString(udpdata);	
				
			
					//PROTOCOL//
					if(data!=String.Empty)
					{
						TranslateData(data);
					//	print(data);
					}
					else
					{
						clearLists();//if no data clear lists
					}
		
				
            }//try
            catch (Exception err) 
            {
                print(err.ToString());
            }
			
		//	clearLists();//clear lists
			
        }//while true		
	 
    }//ReceiveData
	
	
	void TranslateData(string n_data)
	{
		//	[$]<data  type> , [$$]<device> , [$$$]<joint> , <transformation> , <param_1> , <param_2> , .... , <param_N>
		
		// Decompose incoming data based on the protocol rules
		string[] separators = {"[$]","[$$]","[$$$]",",",";"," "};
			      
		string[] words = n_data.Split(separators, StringSplitOptions.RemoveEmptyEntries);
			 	         					
		datatype = words[0];
//		datatype = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(words[0].ToUpper());
		device = words[1];
		joint = words[2];
		transformationtype = words[3];
		
		string emustring = device+"_"+joint;
		
//		udpx = float.Parse(words[4]);
//		udpy = float.Parse(words[5]);
//		udpz = float.Parse(words[6]);
//		udpw = float.Parse(words[7]);	
		

		//populate lists categorizing the segmeneted data
		if(!emulst.Contains(emustring)&& emustring!=String.Empty)
		{	
			emulst.Add(emustring);//emulation
		}	
		if(!datatypelst.Contains(datatype)&& datatype!= String.Empty)
		{
			datatypelst.Add(datatype);//add to datatype list
		}
		if(!devicelst.Contains(device)&& device!=String.Empty)
		{
			devicelst.Add(device);//add to device list
		}
		if(!jointslst.Contains(joint)&& joint!=String.Empty)
		{
			jointslst.Add(joint);//add to joint list	
		//	emulst.Add(emustring);//emulation
		}	
		if(!transformTypelst.Contains(transformationtype)&& transformationtype!=String.Empty)
		{
			transformTypelst.Add(transformationtype);//add to transformaton type list
		}
		
		
		//apply tacking
		if(selection==device)	
		{
			if(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(selection.ToUpper())=="KINECT" && System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(joint.ToUpper())=="LEFTWRIST" && Init.Hand=="Left")
			{
				if(transformationtype=="position")
				{
					udppos = new Vector3(float.Parse(words[4]),float.Parse(words[5]),float.Parse(words[6]));
				}
				else if (transformationtype=="rotation")
				{
					udprot = new Quaternion(float.Parse(words[4]),float.Parse(words[5]),float.Parse(words[6]),float.Parse(words[7]));
				}				
				//print("kinect;;;leftwrist");
			}
			if(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(selection.ToUpper())=="KINECT" && System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(joint.ToUpper())=="RIGHTWRIST" && Init.Hand=="Right")
			{
				if(transformationtype=="position")
				{
					udppos = new Vector3(float.Parse(words[4]),float.Parse(words[5]),float.Parse(words[6]));
				}
				else if (transformationtype=="rotation")
				{
					udprot = new Quaternion(float.Parse(words[4]),float.Parse(words[5]),float.Parse(words[6]),float.Parse(words[7]));
				}					
				//print("kinect;;;rightwrist");
			}
			if(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(selection.ToUpper())=="ANTS" && System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(joint.ToUpper())=="GLOVE")
			{
				if(transformationtype=="position")
				{
					udppos = new Vector3(float.Parse(words[4]),float.Parse(words[5]),float.Parse(words[6]));
				}
				else if (transformationtype=="rotation")
				{
					udprot = new Quaternion(float.Parse(words[4]),float.Parse(words[5]),float.Parse(words[6]),float.Parse(words[7]));
				}
				//print("ants;;;glove");
			}
			if(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(selection.ToUpper())=="ANDROID" && System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(joint.ToUpper())=="GYRO")
			{
			//	if(transformationtype=="position")
			//	{
					udppos = new Vector3(float.Parse(words[4]),float.Parse(words[5]),float.Parse(words[6]));
			//	}
				//print("android;;;gyro");				
			}
			if(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(selection.ToUpper())=="WII" && System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(joint.ToUpper())=="ACCELEROMETER")
			{
				if(transformationtype=="rotation")
				{
					udppos = new Vector3(float.Parse(words[4]),float.Parse(words[5]),float.Parse(words[6]));
				}				
			}			
//			else{
//				udppos = new Vector3(float.Parse(words[4]),float.Parse(words[5]),float.Parse(words[6]));
//			}
			if(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(selection.ToUpper())=="TOBII" && System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(joint.ToUpper())=="CENTERGAZEPOINT")
			{
				if(transformationtype=="position")
				{
					udppos = new Vector3(float.Parse(words[4]),float.Parse(words[5]),0);
				}				
			}			
//			else{
//				udppos = new Vector3(float.Parse(words[4]),float.Parse(words[5]),0);
//			}			

		}
		
		if(selection==emustring)
		{
			if(transformationtype=="position")
			{
				udppos = new Vector3(float.Parse(words[4]),float.Parse(words[5]),float.Parse(words[6]));
			}			
		}
	

	}//end of TranslateData()
	
	
    public static void clearLists()
	{	   
	  datatypelst.Clear();
	  devicelst.Clear();
	  jointslst.Clear();
	  transformTypelst.Clear();
	  emulst.Clear();
		
	  xvalues.Clear();
	  yvalues.Clear();
	  zvalues.Clear();
	  xmax= 0; xmin = 0;
	  ymax= 0; ymin = 0;
	  zmax= 0; zmin = 0;
		
	  scalex= 0;
	  scaley= 0;
	  scalez= 0;
	}
	
	
	void OnGUI () 
	{
		//Game Settings
		if(GeneralSettings.isPaused && !Init.mainscreen)
		{
					
			//Network Group
//			GUI.BeginGroup (new Rect (Screen.width-250, 10, 150, 200));
//			GUI.BeginGroup (new Rect (Screen.width/2, 10, 150, 200));
//			GUI.color = Color.yellow;
//			GUI.Box (new Rect (0,0,150,200), "Receive Data");
//			GUI.color = Color.white;
//			
//			GUI.Label(new Rect(30, 100, 100, 20), "Port:");
//			portField = GUI.TextField (new Rect (65, 100, 50, 20), portField);
//			
//			GUI.enabled = !isConnected;				
//			//Start sending UDP
//			if (GUI.Button (new Rect (30, 30, 90, 30), "Start")) 
//			{		
//				port = int.Parse(portField);			
//				init();		
//				isConnected = true;
//			}	
//			GUI.enabled = true;	
//				
//			GUI.enabled = isConnected;		
//			//Stop sending UDP
//			if (GUI.Button (new Rect (30, 60, 90, 30), "Stop")) 
//			{		
//				isConnected = false;
//				receiveThread.Abort();
//				client.Close();
//				clearLists();
//				print("Stop UDP");
//				tracking = false;
//				calibrate = false;
//			}
//	 		GUI.enabled = true;
//			
//			
//#if UNITY_STANDALONE
//			//display local ip address
//			GUI.color = Color.grey;
//			GUI.Label(new Rect(20, 130, 200, 20), "Local IP Address: ");
//			GUI.Label(new Rect(30, 150, 200, 20), LocalIPAddress());
//			GUI.color = Color.white;
//#if !UNITY_STANDALONE_LINUX					
//			//copy address to clipboard
//			if (GUI.Button (new Rect (15,175,120,20), "Copy to Clipboard"))
//			{
//				ClipboardHelper.clipBoard = LocalIPAddress();
//			}
//#endif
//#endif
//			GUI.EndGroup (); // end network group
			
				
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////				
			
		//Select device: mouse, keyboard, tracking
		GUI.BeginGroup(new Rect( Screen.width-210, 10, 160, 250+windowHeight));
		GUI.color = Color.yellow;
        GUI.Box(new Rect(0, 0, 140, 250+windowHeight), "Available Interfaces");
		GUI.color = Color.white;
			
			
       if (GUI.Button(new Rect(25, 40, 85, 80), "KEYBOARD"))
		{
			GeneralSettings.device = "Keyboard";
			GeneralSettings.TileSpeed=0.25f;
            UnityEngine.Debug.Log("Keyboard");
		}
			
				
		if (GUI.Button(new Rect(30, 140, 80, 80), "MOUSE"))
		{
			#if UNITY_WEBPLAYER
			GeneralSettings.device = "RawMouse";
			#elif UNITY_STANDALONE	
			GeneralSettings.device = "Mouse";	
			UDPReceive.udppos = Vector3.zero;
			clearLists();//clear lists
			Movement.calibLoop = true;//calibration
					selection="mouse";
			//Adjust filter
			UDPReceive.ALPHA = 1.0f;
			UDPReceive.deltaon = false;
			#endif 
            UnityEngine.Debug.Log("Mouse");
		}
        		
//		if (GUI.Button(new Rect(40, 110, 80, 30), "Raw Mouse"))
//		{
//			GeneralSettings.device = "RawMouse";
//            Debug.Log("Raw Mouse");	
//		}
			
			
	float yOffset = 0.0f;
	foreach(string emu in devicelst)
		{
		   if(GUI.Button (new Rect (30, 240+ yOffset, 80, 80), System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(emu.ToUpper())))
			{
					
						clearLists();//clear lists before using other joint for tracking
//						calibrate = true;		
						selection = emu;//fix to point into a joint
						tracking=true;
						print("Selected: " + emu);
						GeneralSettings.device="Tracking";				
					
					
				string tempString = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(emu.ToUpper());	
				//aply filter presets
				if(tempString.Contains("KINECT"))
				{
					calibrate = true;
					ALPHA=0.65f;
					deltaon=true;
				}
				if(tempString.Contains("ANTS"))
				{
					calibrate = true;
					ALPHA=0.20f;
					deltaon=false;
				}
				if(tempString.Contains("ANDROID"))
				{
					calibrate = true;	
					ALPHA=0.25f;
					deltaon=false;			
				}
				if(tempString.Contains("TOBII"))
				{
					eyetrack = true;
						xmax = Screen.currentResolution.width;
						xmin = 0;
						ymax = Screen.currentResolution.height;
						ymin = 0;
									//print(xmax+","+ymax);
					ALPHA=0.15f;
					deltaon=false;			
				}					
				else{
						ALPHA=0.85f;
						deltaon=true;
					}		
					
									
		   	}			
		        yOffset += 100;
				
				windowHeight+=60; //UnityEngine.Debug.Log("devicelst: "+devicelst.Capacity);
						
		}
			
		GUI.EndGroup();	
				
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			
			
			//list buttons of available joints dynamically
//			GUI.color = Color.yellow;
//			GUI.Label(new Rect(Screen.width-230, Screen.height/2-60, 130, 200), "Available Tracking:");
//			GUI.color = Color.white;
//			
//		float yOffset = 0.0f;			
//		scrollPositiontrc = GUI.BeginScrollView(new Rect(Screen.width-280, Screen.height/2-50, 220, 150), scrollPositiontrc, new Rect(0, 0, 320, 220+(emulst.Count*20)));
//		if (emulst.Count!=0)
//			{	
//				foreach(string emu in emulst)
//		        {
//		           if(GUI.Button (new Rect (5, 20+ yOffset, 10+(emu.Length*10), 20), System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(emu.ToUpper())))
//					{
//						clearLists();//clear lists before using other joint for tracking
//						calibrate = true;
//						
//						selection = emu;//fix to point into a joint
//						tracking=true;
//						print("Selected: " + emu);
//						GeneralSettings.device="Tracking";
//					//	StartCoroutine(Calibrate());//start calibration
//		           	}			
//		          yOffset += 25;
//		         }
//			}
//        GUI.EndScrollView();
//			
			

			
			
		//callibratoin pop-up window
		if (activeWin)
		{
	       windowRect0 = GUI.Window(0, windowRect0, CalibWindow, "Calibration");
		}
			
		if (activeNetWin)
		{
	       windowRect1 = GUI.Window(1, windowRect1, NetWindow, "Network");
		}

		if (activeDevWin)
		{
	       windowRect2 = GUI.Window(2, windowRect2, DevWindow, "Network Devices");
		}
			
//		GUI.enabled = tracking;
		
					//callibratoin pop-up window
		  if (GUI.Button(new Rect(55, Screen.height-105, 100, 20), "Network"))
			{activeNetWin = true;}
			
		//callibratoin pop-up window
		  if (GUI.Button(new Rect(55, Screen.height-75, 100, 20), "Filtering"))
			{activeWin = true;}
		//	GUI.Label(new Rect(Screen.width-240, Screen.height/2+160, 200, 100), "Press 'q' to stop emulation");
			
		  if (GUI.Button(new Rect(55, Screen.height-45, 100, 20), "Devices"))
			{activeDevWin = true;}			
		
	if(calibrate)
		{
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), " ", bgstyle);

			GUI.Label(new Rect(20, 20, 200, 100), "Press C to stop calibration", calbText);
				
//			if (GUI.Button(new Rect(Screen.width/2-50, Screen.height/2, 110, 80), "Stop Calibration"))
//			{
//					calibrate = false; 
//					Movement.calibLoop = false;
//			}
		}
		
		//display calibration dialog
		if(calibrate && tracking)
		{
		#if UNITY_STANDALONE		
		XmlDataWriter.isCalibrating = true;
		#endif
		GUI.color = Color.yellow;	
		GUI.Label(new Rect(50, Screen.height-25, 200, 100), "Calibrating...");
		GUI.color = Color.white;	
		}
		else if (!calibrate && tracking)
		{
		#if UNITY_STANDALONE
		XmlDataWriter.isCalibrating = false;
		#endif
		GUI.color = Color.green;	
		GUI.Label(new Rect(50, Screen.height-25, 200, 100), "Calibration finished!");
		GUI.color = Color.white;
		}
		else{GUI.Label(new Rect(50, Screen.height-25, 200, 100), " ");}
		
//		GUI.enabled = true;	
			
			
			//tracking
//		float yOffset = 0.0f;			
//		scrollPositiontrc = GUI.BeginScrollView(new Rect(Screen.width/2-40, 350, 220, 100), scrollPositiontrc, new Rect(0, 0, 320, 220+(emutracklst.Count*20)));
//			foreach(string emu in emutracklst)
//	        {
//	           if(GUI.Button (new Rect (5, 20+ yOffset, 10+(emu.Length*10), 20), System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(emu.ToUpper())))
//				{
//					selection = emu;//fix to point into a joint
//					tracking=true;
//					print("Selected: " + emu);
//	           	}			
//	          yOffset += 25;
//	         }	
//        GUI.EndScrollView();
		
		//button
//		float yOffset3 = 0.0f;			
//		scrollPositionbtn = GUI.BeginScrollView(new Rect(Screen.width/2+240, 350, 220, 100), scrollPositionbtn, new Rect(0, 0, 320, 220+(emubuttonlst.Count*20)));
//			foreach(string emu in emubuttonlst)
//	        {
//	           if(GUI.Button (new Rect (5, 20+ yOffset3, 10+(emu.Length*10), 20), System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(emu.ToUpper())))
//				{
//					selection = emu;//fix to point into a joint
//					tracking=true;
//					print("Selected: " + emu);
//	           	}			
//	          yOffset3 += 25;
//	         }	
//        GUI.EndScrollView();
			
		}//on pause
		
	}//onGUI
	
	
//	void incomingData()
//	{
//			if (devicelst.Count!=0)
//			{	
//				foreach(string dev in devicelst)
//		        {
//		           if(dev == "Ants")
//					{
//						GeneralSettings.device = "Ants";
//					
////						clearLists();//clear lists before using other joint for tracking
////						calibrate = true;						
////						selection = emu;//fix to point into a joint
////						tracking=true;
////						print("Selected: " + emu);
////						GeneralSettings.device="Tracking";
//					
//		           	}
//					if(dev == "Kinect")
//					{
//						//todo
//						GeneralSettings.device = "Kinect";
//					}
//					if(dev == "Android")
//					{
//						//todo
//						GeneralSettings.device = "Android";
//					}		         
//		         }
//			}
//	}
	
	
  IEnumerator Calibrate()
    {
			print("Calibrating...");
		XmlDataWriter.isCalibrating = true;
            yield return new WaitForSeconds(calibrationTime);
			calibrate=false;//stop calibration
		XmlDataWriter.isCalibrating = false;
			print("Calibration finished!");
		Time.timeScale = 0.0f;
    }
	
	
		void CalibWindow(int windowID)
		{
			//Filtering & calibration
			GUI.BeginGroup (new Rect (30, 30, 230, 320));
			GUI.Box (new Rect (0,0,230,320), "Tracking Calibration");

			GUI.Label(new Rect(20, 40, 220, 20), "max: "+xmax.ToString("0.00")+","+ymax.ToString("0.00")+","+zmax.ToString("0.00"));
			GUI.Label(new Rect(20, 60, 220, 20), "min: "+xmin.ToString("0.00")+","+ymin.ToString("0.00")+","+zmin.ToString("0.00"));
			GUI.Label(new Rect(20, 90, 220, 20), "Scale: "+scalex.ToString("0.00")+","+scaley.ToString("0.00")+","+scalez.ToString("0.00"));
			
			yon = GUI.Toggle(new Rect(150, 120, 80, 20), yon, "use y");
			zon = GUI.Toggle(new Rect(150, 140, 80, 20), zon, "use z");
			
			//calibrate = GUI.Toggle(new Rect(30, 160, 100, 30), calibrate, "Calibrate");
			
			//filter
			GUI.Label(new Rect(20, 190, 200, 20), "Filtered: "+FilteredPosition.x.ToString("0.00")+","+FilteredPosition.y.ToString("0.00"));
			ALPHA = GUI.HorizontalSlider(new Rect(30, 250, 100, 30), ALPHA, 0.0F, 1.0F);
			GUI.Label(new Rect(50, 270, 200, 100), "Alpha: "+ALPHA.ToString("0.00"));
			deltaon = GUI.Toggle(new Rect(160, 250, 100, 30), deltaon, "Delta");
			GUI.EndGroup ();
		
			//close button
	        if (GUI.Button(new Rect((windowRect0.width/2)-50, windowRect0.height-30, 100, 20), "Close"))
			{
				activeWin = false;
	            print("closing calibration window");
			}
			
			//drag window
	         GUI.DragWindow(new Rect(0, 0, 10000, 20));
    	}
	
	
	
		void NetWindow(int windowID)
		{
			//Filtering & calibration
			GUI.BeginGroup (new Rect (10, 20, 180, 300));
			GUI.Box (new Rect (0,0,160,280), "");

			GUI.Label(new Rect(30, 100, 100, 20), "Port:");
			portField = GUI.TextField (new Rect (65, 100, 50, 20), portField);
			
			GUI.enabled = !isConnected;				
			//Start sending UDP
			if (GUI.Button (new Rect (30, 30, 90, 30), "Start")) 
			{		
				port = int.Parse(portField);			
				init();		
				isConnected = true;
			}	
			GUI.enabled = true;	
				
			GUI.enabled = isConnected;		
			//Stop sending UDP
			if (GUI.Button (new Rect (30, 60, 90, 30), "Stop")) 
			{		
				isConnected = false;
				receiveThread.Abort();
				client.Close();
				clearLists();
				print("Stop UDP");
				tracking = false;
				calibrate = false;
			}
	 		GUI.enabled = true;
			
			
#if UNITY_STANDALONE
			//display local ip address
			GUI.color = Color.grey;
			GUI.Label(new Rect(20, 130, 200, 20), "Local IP Address: ");
			GUI.Label(new Rect(30, 150, 200, 20), LocalIPAddress());
			GUI.color = Color.white;
#if !UNITY_STANDALONE_LINUX					
			//copy address to clipboard
			if (GUI.Button (new Rect (15,175,120,20), "Copy to Clipboard"))
			{
				ClipboardHelper.clipBoard = LocalIPAddress();
			}
#endif
#endif	

			GUI.EndGroup ();
		
			//close button
	        if (GUI.Button(new Rect((windowRect1.width/2)-50, windowRect1.height-60, 100, 20), "Close"))
			{
				activeNetWin = false;
	            print("closing net window");
			}
			
			//drag window
	         GUI.DragWindow(new Rect(0, 0, 10000, 20));
    	}
	
	
		void DevWindow(int windowID)
		{
			GUI.Box (new Rect (10,20,220,240), "");
			float yOffset = 0.0f;			
			scrollPositiontrc = GUI.BeginScrollView(new Rect(10, 50, 220, 220), scrollPositiontrc, new Rect(0, 0, 320, 270+(emulst.Count*20)));
			if (emulst.Count!=0)
				{	
					foreach(string emu in emulst)
			        {
			           if(GUI.Button (new Rect (5, 20+ yOffset, 10+(emu.Length*10), 20), System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(emu.ToUpper())))
						{
							clearLists();//clear lists before using other joint for tracking
							calibrate = true;		
							selection = emu;//fix to point into a joint
							tracking=true;
							print("Selected: " + emu);
							GeneralSettings.device="Tracking";	
			           	}			
			          yOffset += 25;
			         }
				}
	        GUI.EndScrollView();	

			//close button
	        if (GUI.Button(new Rect((windowRect2.width/2)-50, windowRect2.height-30, 100, 20), "Close"))
			{
				activeDevWin = false;
			}
			
			//drag window
	         GUI.DragWindow(new Rect(0, 0, 10000, 20));
		
		}
	

#if UNITY_STANDALONE
	public string LocalIPAddress()
	 {
		if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
    	{
        return null;
    	}
		
	   IPHostEntry host;
	   string localIP = "";
	   host = Dns.GetHostEntry(Dns.GetHostName());
	   foreach (IPAddress ip in host.AddressList)
	   {
	     if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork )
	     {
	       localIP = ip.ToString();
	     }
	   }
	   return localIP;
	 }
#endif		

	
	void OnDisable() 
	{ 
		if(isConnected)
    	{
		 receiveThread.Abort(); 
		 client.Close(); 
		}
	} 	
	
	void OnApplicationQuit () 
	{
		if(isConnected)
	    {
		  receiveThread.Abort();
          client.Close();
		}
    }
	

}