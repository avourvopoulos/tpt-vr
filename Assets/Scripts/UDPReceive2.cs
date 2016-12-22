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

public class UDPReceive2 : MonoBehaviour {
	
 // receiving Thread
    Thread receiveThread; 
 // udpclient object
    UdpClient client; 

    List<string> datatypelst = new List<string>();
	public static List<string> devicelst = new List<string>();
	public static List<string> jointslst = new List<string>();
	public static List<string> transformTypelst = new List<string>();
	List<string> emulst = new List<string>();
	List<string> emutracklst = new List<string>();
	List<string> emubuttonlst = new List<string>();
//	public static float[] param = new float[4]; 
	
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
	public static float scalex, scaley, scalez;
	public static bool calibrate = true;
	public GUIStyle txtstyle;
	
	public static float ALPHA = 0.55f;
	//float[] xin, xout = new float[10];//x in/out
	//float[] yin, yout = new float[10];//y in/out
	public float[] n_input = new float[2];//xy in/out
	public float[] n_output = new float[2];//xy in/out
	
//	public Transform start;
	Vector3 FilteredPosition;
//	public float smooth = 5.0f;//smoothing for lerp
	public static float delta;
	public static bool deltaon = true;
	
	bool yon = true;
	bool zon = false;
	
	public static string rawdata;
	
	public static bool emulate = false;
	public Vector2 scrollPositiontrc = Vector2.zero;
	public Vector2 scrollPositionbtn = Vector2.zero;
	//GUIStyle guistyle;
		
    public void Start()
    {
    //    init(); 
	//	camera.nearClipPlane = 11.0F;
		
    }
	
	
	void Update () 
	{	
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
	//		while(calstatus)	
			{
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
			}//end calibrate
			

			//scale raw data between -1 to 1
			scalex = (2 * (-udppos.x - xmin)/( xmax - xmin) - 1);
			scaley = (2 * (udppos.y - ymin)/( ymax - ymin) - 1); 		//	scaley = (2 * (udppos.y - ymin)/( ymax - ymin) - 0.5f);
			scalez = (2 * (-udppos.z - zmin)/( zmax - zmin) - 1);
			
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
			FilteredPosition = new Vector3(lowPass(n_input, n_output)[0], lowPass(n_input, n_output)[1], target.transform.position.z);	

				if(emulate)
				{
					//add mouse cursor code
					//MoveMouse(FilteredPosition.x, FilteredPosition.y):
					target.transform.position = new Vector3(FilteredPosition.x, FilteredPosition.y, target.transform.position.z);//low pass xy		
				}
				else{}

			
		}//end tracking
		
		
	}
	
	//low pass filter x,y
	float[] lowPass( float[] input, float[] output ) 
	{
     for(int i=0; i<input.Length; i++ ) 
		{
			 if( output == null || output.Length == 0 || float.IsNaN(output[0]) || float.IsNaN(output[1]) || float.IsInfinity(output[0]) || float.IsInfinity(output[1]))
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

	
	public float InputGetAxis(string axis)
	{
	    var v = Input.GetAxis(axis);
	    if (Mathf.Abs(v) > 0.005)return v;
	    if (axis=="Horizontal")return axisH;
	    if (axis=="Vertical") return axisV;
		else return 0;
	}
	
	
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
					//	rawdata = data; //print(rawdata);
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
		
		string emustring = device+"::"+joint;
		
//		udpx = float.Parse(words[4]);
//		udpy = float.Parse(words[5]);
//		udpz = float.Parse(words[6]);
//		udpw = float.Parse(words[7]);	
			
		
//		if(selection==joint)
		if(selection==emustring)	
		{
			if(transformationtype=="position")
			{
				udppos = new Vector3(float.Parse(words[4]),float.Parse(words[5]),float.Parse(words[6]));
			}
			else if (transformationtype=="rotation")
			{
				udprot = new Quaternion(float.Parse(words[4]),float.Parse(words[5]),float.Parse(words[6]),float.Parse(words[7]));
			}
		}
		else{}
			
		
		if(!datatypelst.Contains(datatype)&& datatype =="tracking")
		{
			emutracklst.Add(emustring);
		}
		//populate lists categorizing the segmeneted data
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
			emulst.Add(emustring);//emulation
		}	
		if(!transformTypelst.Contains(transformationtype)&& transformationtype!=String.Empty)
		{
			transformTypelst.Add(transformationtype);//add to transformaton type list
		}
		

	}//end of TranslateData()
	
	
	void clearLists()
	{	   
	  datatypelst.Clear();
	  devicelst.Clear();
	  jointslst.Clear();
	  transformTypelst.Clear();
	  	emubuttonlst.Clear();
		emutracklst.Clear();
		emulst.Clear();
		
	  xvalues.Clear();
	  yvalues.Clear();
	  zvalues.Clear();
	  xmax= 0; xmin = 0;
	  ymax= 0; ymin = 0;
	  zmax= 0; zmin = 0;
	}
	
	
	void OnGUI () 
	{
		//Game Settings
		if(GeneralSettings.isPaused)
		{
			
//			GUI.BeginGroup (new Rect (Screen.width-300, (Screen.height/2-80), 250, 320));
//			GUI.Box (new Rect (0,0,250,320), "Tracking Calibration");
//			GUI.Label(new Rect(20, 20, 200, 20), "x max: "+xmax+"   x min: "+xmin, txtstyle);
//			GUI.Label(new Rect(20, 40, 200, 20), "y max: "+ymax+"   y min: "+ymin, txtstyle);
//			GUI.Label(new Rect(20, 60, 200, 20), "z max: "+zmax+"   z min: "+zmin, txtstyle);
//			GUI.Label(new Rect(20, 90, 200, 20), "Scale x: "+scalex, txtstyle);
//			GUI.Label(new Rect(20, 110, 200, 20), "Scale y: "+scaley, txtstyle);
//			GUI.Label(new Rect(20, 130, 200, 20), "Scale z: "+scalez, txtstyle);
//			
//			yon = GUI.Toggle(new Rect(150, 110, 80, 20), yon, "use y");
//			zon = GUI.Toggle(new Rect(150, 130, 80, 20), zon, "use z");
//			
//			calibrate = GUI.Toggle(new Rect(30, 160, 100, 30), calibrate, "Calibrate");
//			
//			//filter
//			GUI.Label(new Rect(20, 190, 200, 20), "Filtered x: "+FilteredPosition.x, txtstyle);
//			GUI.Label(new Rect(20, 210, 200, 20), "Filtered y: "+FilteredPosition.y, txtstyle);
//			ALPHA = GUI.HorizontalSlider(new Rect(35, 250, 100, 30), ALPHA, 0.0F, 1.0F);
//			GUI.Label(new Rect(60, 270, 200, 100), "Alpha: "+ALPHA.ToString("0.00"));
//			deltaon = GUI.Toggle(new Rect(30, 300, 100, 30), deltaon, "Delta");
//			GUI.EndGroup ();
//			
			
			//Network Group
			GUI.BeginGroup (new Rect (Screen.width-250, 10, 150, 180));
			GUI.color = Color.yellow;
			GUI.Box (new Rect (0,0,150,180), "Receive Data");
			GUI.color = Color.white;
			
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
			if (GUI.Button (new Rect (30,165,120,20), "Copy to Clipboard"))
			{
				ClipboardHelper.clipBoard = LocalIPAddress();
			}
#endif
#endif
			GUI.EndGroup (); // end network group
				
				
			
			//list available devices dynamically
//			GUI.color = Color.yellow;
//			GUI.Label(new Rect(Screen.width/2-20, 30, 130, 200), "Available Devices:");
//			GUI.color = Color.white;
//	
//			float yOffset2 = 0.0f;
//			foreach(string dev in devicelst)
//	        {
//				GUI.Label (new Rect (Screen.width/2-10, 60+ yOffset2, 85, 20), System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(dev.ToUpper()));//display device name in Uppercase
//				udpDev = dev;
//				yOffset2 += 25;
//			}
				
			
			//list buttons of available joints dynamically
			GUI.color = Color.yellow;
			GUI.Label(new Rect(Screen.width-230, Screen.height/2-20, 130, 200), "Available Tracking:");
			GUI.color = Color.white;
		float yOffset = 0.0f;			
		scrollPositiontrc = GUI.BeginScrollView(new Rect(Screen.width/2+180, Screen.height/2, 220, 100), scrollPositiontrc, new Rect(0, 0, 320, 220+(emulst.Count*20)));
			foreach(string emu in emulst)
	        {
	           if(GUI.Button (new Rect (5, 20+ yOffset, 10+(emu.Length*10), 20), System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(emu.ToUpper())))
				{
					selection = emu;//fix to point into a joint
					tracking=true;
					print("Selected: " + emu);
	           	}			
	          yOffset += 25;
	         }	
        GUI.EndScrollView();
			
			
		if (GUI.Button(new Rect(Screen.width-150, Screen.height-130, 80, 20), "Calibration"))
		{
			//open window with filtering options
		}
			
			
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
		if(tracking)
    	{
		 receiveThread.Abort(); 
		 client.Close(); 
		}
	} 	
	
	void OnApplicationQuit () 
	{
		if(tracking)
	    {
		  receiveThread.Abort();
          client.Close();
		}
    }
	

}