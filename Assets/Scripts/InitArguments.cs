using UnityEngine;
using System;
using System.Collections;

public class InitArguments : MonoBehaviour {
	
	public static string arg = "";
	
	// Use this for initialization
	void Awake () 
	{
#if UNITY_STANDALONE
		#if !UNITY_EDITOR
		string[] arguments = Environment.GetCommandLineArgs();
		
//	       foreach(string arg in arguments)
//	       {
//	         cmdInfo += arg.ToString() + "@"+"\n ";
//	       }
		
		arg = arguments[1];
	#endif	
#endif		
		
		#if UNITY_EDITOR
		arg = "nolog";
		#endif	
	}
	
	
}