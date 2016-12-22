using UnityEngine;
using System.Collections;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;

public class UDPSend : MonoBehaviour {
	
	public GameObject IKTarget;

	private static int localPort;

    // prefs 
    public static string IP ;  // define in init
    public static int port ;  // define in init
	
	private string ipField = "127.0.0.1";
	private string portField = "1205";
	
	public static bool flag=false;
		
	// "connection" 
    public static IPEndPoint remoteEndPoint;
    public static UdpClient client;
	
	
	// Use this for initialization
	void Start ()
	{
		IP = "127.0.0.1";
		port = 1205;
        init();	
	}
	
	
//	void OnGUI () 
//	{	
//		//Game Settings
//		if(GeneralSettings.isPaused && !Init.mainscreen)
//		{		
//				
//			//Network Group
//			GUI.BeginGroup (new Rect (Screen.width - 212, Screen.height/2 - 200, 200, 120));
//			GUI.color = Color.yellow;
//			GUI.Box (new Rect (0,0,200,120), "Send Data");
//			GUI.color = Color.white;
//			
//	GUI.enabled = !flag;
//			
//			GUI.Label(new Rect(10, 60, 100, 20), "Address:");
//			GUI.Label(new Rect(10, 90, 100, 20), "Port:");
//			ipField = GUI.TextField (new Rect (65, 60, 100, 20), ipField);
//			portField = GUI.TextField (new Rect (65, 90, 50, 20), portField);
//			
//			//Start sending UDP
//			if (GUI.Button (new Rect (10, 25, 90, 30), "Send")) 
//			{
//				IP = ipField;
//				port = int.Parse(portField);
//				
//				init();
//				flag=true;
//				InvokeRepeating("GameData", 0.1F, 0.05F);//Send Data 20Hz
//				Debug.Log("Start UDP");
//			}	
//	GUI.enabled = true;	
//			
//	GUI.enabled = flag;		
//			//Stop sending UDP
//			if (GUI.Button (new Rect (100, 25, 90, 30), "Disconnect")) 
//			{		
//				flag=false;
//				 CancelInvoke("GameData");//stop data loop
//				client.Close();
//				Debug.Log("Stop UDP");
//			}
//	 GUI.enabled = true;
//			
//			GUI.EndGroup (); // end network group			
//			
//		}//if paused
//		 			
//	}

	
	
	public static void init()
    {       			

        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);

        client = new UdpClient();  

        // status
        print("Sending to "+IP+" : "+port);
    }
	
	
//	void GameData()
//	{
//		
//		//Inverse Kinematics (IK) data
//		//[$]GameData , [$$]TPT-VR , [$$$]IK , x , y, z
//		sendString("[$]GameData,[$$]TPT-VR,[$$$]IK,position,"+IKTarget.transform.position.x.ToString()+","+IKTarget.transform.position.y.ToString()+","+IKTarget.transform.position.z.ToString()+","+"0"+";");
//		
//		//if calibrating
//		//[$]GameData , [$$]TPT-VR , [$$$]Calibrating , true/false
//		if (XmlDataWriter.isCalibrating)
//		{
//			sendString("[$]GameData,[$$]TPT-VR,[$$$]Calibrating,isCalibrating;");
//		}
//		
//		//clicks
//		//[$]GameData , [$$]TPT-VR , [$$$]Clicks , mouse/keyboard/timer
//		if(ClickSelection.MouseClick())
//		{
//			sendString("[$]GameData,[$$]TPT-VR,[$$$]Clicks,Mouse;");
//		}
//		if(ClickSelection.KeyboardClick())
//		{
//			sendString("[$]GameData,[$$]TPT-VR,[$$$]Clicks,Keyboard;");
//		}
//		if(GeneralSettings.timerOn)
//		{
//			sendString("[$]GameData,[$$]TPT-VR,[$$$]Clicks,Timer;");
//		}
//		
//		//selected tile
//		//[$]GameData , [$$]TPT-VR , [$$$]Tile , name , symbol
//		sendString("[$]GameData,[$$]TPT-VR,[$$$]Tile,"+ClickSelection.tile+","+ClickSelection.symbol+";");
//		
//
//		//correct choices 
//		//[$]GameData , [$$]TPT-VR , [$$$]Choice , right/wrong
//		sendString("[$]GameData,[$$]TPT-VR,[$$$]Choices,"+GameScoring.GetCorrect()+","+GameScoring.GetError()+";");
////		sendString("[$]GameData,[$$]TPT-VR,[$$$]WrongChoices,"+GameScoring.GetError()+";");
//
//
////==========================================================
//
////	[$]GameData , [$$]TPT-VR , [$$$]KLM , score , time
//
////	[$]GameData , [$$]TPT-VR , [$$$]CorrectSymbols , 1 , 2, 3
//
////	[$]GameData , [$$]TPT-VR , [$$$]KLM , score , time
//
////	[$]GameData , [$$]TPT-VR , [$$$]TileGrid , Tile0 , TP6
//		
//		
//	}
	
	
	 // sendData
    public static void sendString(string message)
    {
        try 
        {
               if (message != "") 
                {
                    // UTF8 encoding to binary format.
                     byte[] data = Encoding.UTF8.GetBytes(message);
			
					//byte[] data = Encoding.ASCII.GetBytes(message); asci

                    // Send the message to the remote client.
                   client.Send(data, data.Length, remoteEndPoint);		
                }
        }

        catch (Exception err)
        {
          Debug.Log(err.ToString());
        }
    }//end of sendString
	
	
	
	 void OnApplicationQuit() 
		{
         	 if(flag)
			{
         	 client.Close();
			}
		}	
	
	
	
}
