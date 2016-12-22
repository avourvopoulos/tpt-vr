using UnityEngine;
using System.Collections;
using System.Threading;

public class FinishButton : MonoBehaviour {
	
//	private bool isColided=false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionStay(Collision collision) 
	{

		if ((MouseClick()==true || KeyboardClick()==true))
		{
			XmlDataWriter.stopXML=true;
			XmlDataWriter.endXML();
			Thread.Sleep(100);
	     	Application.LoadLevel(2);
		}
	}
	
	
	bool MouseClick()
	{
		if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) && GeneralSettings.device=="Mouse")
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	
	bool KeyboardClick()
	{
		if (Input.GetKey(KeyCode.Space) && GeneralSettings.device=="Keyboard")
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	
	void OnCollisionEnter(Collision collision) 
	{
	//	isColided=true;
		renderer.material.color = Color.gray;
	}
	
	void OnCollisionExit(Collision collisionInfo) 
	{
	//	isColided=false;
		renderer.material.color = Color.white;	
	}
	
	
	
}
