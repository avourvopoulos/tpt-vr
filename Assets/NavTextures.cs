using UnityEngine;
using System.Collections;

public class NavTextures : MonoBehaviour {
	
	public Texture leftbar;
	public Texture rightbar;
	
	public GUIStyle navstyle;
	
	public GameObject LeftBorder;
	public GameObject RightBorder;
	
	public static bool goLeft = false;
	public static bool goRight = false;
	
	
	// Use this for initialization
	void Start () 
	{
	//	LeftBorder.transform.position = new Vector3(Screen.width-(Screen.width+10), Screen.height, 2);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	
	void OnGUI () 
	{
		if(GUI.Button(new Rect(Screen.width-(Screen.width+100),0,40,Screen.height), leftbar)) //0
		{
			goLeft = true;
		}
		else
		{
			goLeft = false;
		}
		
		if(GUI.Button(new Rect(Screen.width,00,40,Screen.height), rightbar)) //-40
		{
			goRight = true;
		}
		else
		{
			goRight = false;
		}
	}
}
