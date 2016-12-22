using UnityEngine;
using System.Collections;

public class ClickSelection : MonoBehaviour {
	
	public static bool isColided=false;
	public static bool isSelected=false;
	
//	public GameObject TickObject;
//	public GameObject UntickObject;
	public GameObject CrossOut;
	public GameObject Timer;
	
	public GameObject Frame;
	
	public static string tile = string.Empty;//tile name
	public static string symbol = string.Empty;//texture name
	public static string tileposition = string.Empty;//texture name
	
	public static bool hover = false;
	
	public static bool grasp = false;
	
	void Awake()
	{
		//Disabling tick/untick objects
//		TickObject.active = false;
//		UntickObject.active = false;
		Frame.SetActive(false);
		
		CrossOut.SetActive(false);
		Timer.SetActive(false);//Disabling Timer
	}
	
	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{

		
	}
	
	public static bool MouseClick()
	{
		if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) && (GeneralSettings.device=="Mouse" ||GeneralSettings.device=="RawMouse") && !GeneralSettings.timerOn)
		{
			return true;
		}
	 return false;
	}
	
	public static bool KeyboardClick()
	{
		if ((Input.GetKeyDown(KeyCode.Space)||
			Input.GetKeyDown(KeyCode.F5) || 
			Input.GetKeyDown(KeyCode.F6) || 
			Input.GetKeyDown(KeyCode.F7) || 
			Input.GetKeyDown(KeyCode.F8)) )//&& (GeneralSettings.device=="Keyboard" || GeneralSettings.device=="Tracking") && !GeneralSettings.timerOn)
		{
			return true;
		}
	 return false;
	}
	
	void OnCollisionStay(Collision collision) 
	{
		//Debug.Log(name);
		hover = true;
		tileposition = transform.position.ToString();
		tile = name;
		symbol = renderer.material.mainTexture.name;
		
//		Debug.Log("Name: "+tile+" Texture: "+symbol+" Pos: "+tileposition);
		
		//if the Tile is both Colided and Clicked = selected
		if ((MouseClick()==true || KeyboardClick()==true) || TimerCount.timeout==true)
		{
			isSelected=true;
			
//			tile = name;
//			symbol = renderer.material.mainTexture.name;

//			Debug.Log("Name: "+tile+" Texture: "+symbol);
			
				//send scoring
				if(renderer.material.mainTexture == SpawnElements.selectionList[0] || 
					renderer.material.mainTexture == SpawnElements.selectionList[1] ||
						renderer.material.mainTexture == SpawnElements.selectionList[2])
				{
					
/*
					//click/unclick the tick
					if(TickObject.active == false)
					{
						TickObject.active = true;//Enabling tick
						UntickObject.active = false;//Disabling unttick
						GameScoring.SetCorrect(1);
					}
					else 
					{
						TickObject.active = false;//Disabling tick
						GameScoring.DeleteCorrect(1);
					}
*/
					//click/unclick the tick
//					if(CrossOut.active == false)
					if(renderer.material.color.Equals(Color.white))
					{
					//	CrossOut.SetActive(true);//Enabling tick
					renderer.material.SetColor("_Color",Color.green);
						GameScoring.SetCorrect(1);				
						TimerCount.timeout=false;//reset timer
					}
					else 
					{
					//	CrossOut.SetActive(false);//Disabling tick
					renderer.material.SetColor("_Color",Color.white);
						GameScoring.DeleteCorrect(1);
					TimerCount.timeout=false;//reset timer
					}
				
						
				}
				else //if wrong choice
				{
				//	GameScoring.SetError(1);
				//	renderer.material.color = Color.red;
				
				//	TickObject.active = false;//Enabling tick
				//	UntickObject.active = true;//Disabling unttick
/*				
					//click/unclick the tick
					if(UntickObject.active == false)
					{
						TickObject.active = false;//Enabling tick
						UntickObject.active = true;//Disabling unttick
						GameScoring.SetError(1);
					}
					else 
					{
						UntickObject.active = false;//Disabling tick
						GameScoring.DeleteError(1);
					}
*/				
					//click/unclick the tick
			//		if(CrossOut.active == false)
					if(renderer.material.color.Equals(Color.white))
					{
					//	CrossOut.SetActive(true);//Enabling unttick
					renderer.material.SetColor("_Color",Color.green);
						GameScoring.SetError(1);
						TimerCount.timeout=false;//reset timer
					}
					else 
					{
					//	CrossOut.SetActive(false);//Disabling tick
					renderer.material.SetColor("_Color",Color.white);
						GameScoring.DeleteError(1);
						TimerCount.timeout=false;//reset timer
					}
				
				}
			
		}//if Mouse/Keyboard click
		
		else
		{
			isSelected=false;
			tile = string.Empty;
			symbol = string.Empty;
		}
//		TimerCount.timeout = false;
		
			
	}
	
	void OnCollisionEnter(Collision collision) 
	{
		isColided=true;
	//	renderer.material.color = Color.gray;
		Frame.SetActive(true);
		
		if(GeneralSettings.timerOn)
		{
		Timer.active = true;//Enabling Timer
		}
	//	Debug.Log("Enter/Timer.active: "+Timer.active);
	//	Debug.Log("Name: "+name+" Texture: "+renderer.material.mainTexture.name);
		
		grasp = true;
	}
	
	void OnCollisionExit(Collision collisionInfo) 
	{
		isColided=false;
	//	renderer.material.color = Color.white;
		Frame.SetActive(false);
	
		Timer.active = false;//Disabling Timer
	//	Debug.Log("Exit/Timer.active: "+Timer.active);
		TimerCount.myTimer = TimerCount.startTime;
		
		grasp = false;
	}
	
	
}
