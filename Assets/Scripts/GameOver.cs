using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {
	
	public GUIStyle textStyle;
	public GUIStyle scoreStyle;

	// Use this for initialization
	void Start () 
	{
		//XmlDataWriter.startXML=false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Screen.showCursor = true;//show mouse cursor
		
		#if UNITY_STANDALONE
		//Pause/Resume Game
		if(Input.GetKeyDown(KeyCode.Escape))
	   	{
			Application.Quit();
	  	}
		#endif
	}
	
	void OnGUI () 
	{
		#if UNITY_STANDALONE
		GUI.Label(new Rect(20, 20, 150, 20), "Press 'esc' to Quit!");	
		GUI.Label(new Rect((Screen.width/2)-120, (Screen.height/2)-150, 150, 20), "Muito bem!", textStyle);
		#endif
		
		#if UNITY_WEBPLAYER
//		GUI.Label(new Rect((Screen.width/2)-80, (Screen.height/2)+20, 150, 20), "Final Score: " + GameScoring.GetKLT()+ " %", scoreStyle);//game score
		GUI.Label(new Rect((Screen.width/2)-120, (Screen.height/2)-150, 150, 20), "Well Done!", textStyle);		
		#endif
		
		GUI.Label(new Rect((Screen.width/2)-80, (Screen.height/2)+60, 150, 20), "Time: "+ GameScoring.time.ToString("00")+" sec", scoreStyle);//time taken in seconds
		
		
//		if(GUI.Button (new Rect (Screen.width-100, Screen.height-70, 80, 60), "Play Again"))
//		{
//			Application.LoadLevel(0);
//		}
		
		#if UNITY_ANDROID
		if(GUI.Button (new Rect (Screen.width-100, Screen.height-70, 80, 60), "Quit"))
		{
			Application.Quit();
		}
		#endif
		
		
	}
}
