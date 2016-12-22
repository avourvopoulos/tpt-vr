using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

public class MouseSettings : MonoBehaviour {
	
	public const uint SPI_SETMOUSESPEED = 0x0071;
	public const uint SPI_GETMOUSESPEED = 0x0070;
	public const uint SPI_SETMOUSE = 0x0004;
	public const uint SPI_GETMOUSE = 0x0003;
	public const int SPIF_UPDATEINIFILE = 0x01;
	public const int SPIF_SENDCHANGE = 0x02;
	
	[DllImport("User32.dll")]
	static extern bool SystemParametersInfo(uint uiAction, uint uiParam, uint pvParam, uint fWinIni);
	
	[DllImport("User32.dll")]
	static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref uint pvParam, uint fWinIni);
	
	[DllImport("user32.dll")]
    static extern bool SetCursorPos(int X, int Y);
	
	static uint initSpeed = 0;
	static uint currSpeed = 0;
	
	
	void Awake () 
	{
		#if UNITY_STANDALONE
		//place cursor in the middle of the screen
//		MoveMouse(Screen.width/2, Screen.height/2);
		
		//get initial speed
		SystemParametersInfo(SPI_GETMOUSESPEED, 0, ref initSpeed, 0);
		
		//set Enhance pointer precision OFF
//		SetMouseAcceleration(0);
		#endif	
	}

	void Update()
	{
		#if UNITY_STANDALONE
		if (Input.GetKeyUp(KeyCode.A))
		{
		//set mouse speed to 1
		SystemParametersInfo( SPI_SETMOUSESPEED, 0, 1, 0);
		}
		
		if (Input.GetKeyUp(KeyCode.Z))
		{
		//set mouse speed to 20
		SystemParametersInfo(SPI_SETMOUSESPEED, 0, 20, 0);
		}
		
		if (Input.GetKeyUp(KeyCode.C))
		{
		//get current mouse speed
		SystemParametersInfo(SPI_GETMOUSESPEED, 0, ref currSpeed, 0);
		print(currSpeed);
		}
		
		if (Input.GetKeyUp(KeyCode.I))
		{print("Init Speed: "+InitSpeed());}
		
//		if (Input.GetKeyUp(KeyCode.I))
//		{
//		//set mouse accel
//		SystemParametersInfo(SPI_SETMOUSESPEED, 0, initSpeed, 0);
//		}
		
		#endif	
	}
	
	
	public static void SetMouseSpeed(uint speed)
	{
		//speed from 1 to 20
		SystemParametersInfo(SPI_SETMOUSESPEED, 0, speed, 0);
	}
	
	public static uint GetCurrentSpeed()
	{
		//get current mouse speed
		SystemParametersInfo(SPI_GETMOUSESPEED, 0, ref currSpeed, 0);
		return currSpeed;		
	}
	
	public static uint InitSpeed()
	{
		//get initial mouse speed
		return initSpeed;		
	}
	
	public static void MoveMouse(int x, int y)
    {
		SetCursorPos(x,y);	
    }
	
	// Turns mouse acceleration on/off by calling the SystemParametersInfo function.
	// When mouseAccel is TRUE, mouse acceleration is turned on; FALSE for off.
	void SetMouseAcceleration(uint mouseAccel)
	{
	    
		uint mouseParams = 0; 
		
	    // Get the current values.
	    SystemParametersInfo(SPI_GETMOUSE, 0, mouseParams, 0);
	
	    // Modify the acceleration value as directed.
	    mouseParams = mouseAccel;
	
	    // Update the system setting.
	    SystemParametersInfo(SPI_SETMOUSE, 0, mouseParams, SPIF_SENDCHANGE);
	}
	
	//return everything to default values
	void OnApplicationQuit() 
	{
		#if UNITY_STANDALONE
		//set the initial speed back
		SetMouseSpeed(InitSpeed());
		
		//set Enhance pointer precision back ON
//		SetMouseAcceleration(1);
		
		#endif	
	}
	
	
}
