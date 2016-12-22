using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestCubeNav : MonoBehaviour {
	
	public GameObject ControlCube;
	
	public Camera camera;
	
	public float minY = 0.0f, maxY = 0.0f, minX = 0.0f, maxX = 0.0f;// X,Y cursor/target limits
	
	List<float> widthlimit = new List<float>();
	List<Vector3> poslimit = new List<Vector3>();
	
//	public float ratio = 0.001f;

	// Use this for initialization
	void Start () 
	{
		maxY = Screen.height;
		minY = Screen.height-(Screen.height);
		
		maxX = 100;//Screen.width-10;
		minX = 0;//Screen.width-(Screen.width+10);
	}
	
	// Update is called once per frame
	void Update () 
	{
			
/*		
		Vector3 screenPos = camera.WorldToScreenPoint(new Vector3(ControlCube.transform.rotation.eulerAngles.y, //x
																	0, //y
																		ControlCube.transform.rotation.eulerAngles.x)); //z
*/		
		 
		Vector3 screenPos = camera.WorldToScreenPoint(new Vector3(ControlCube.transform.rotation.eulerAngles.y, //x
																	0, //y
																		ControlCube.transform.rotation.eulerAngles.x)); //z
		
		
//		screenPos.x = Mathf.Clamp( screenPos.x, minX, maxX);
//	    screenPos.y = Mathf.Clamp( screenPos.y, minY, maxY);
		
		//Min: 475.3177 Max: 32162.13
//		transform.position = screenPos*0.001f;
		

		if (screenPos.x*0.001f <= 0.5)
		{
       	 transform.position = new Vector3(0.5f, 0, screenPos.z);
		}
   	    if (screenPos.x*0.001f >=32.0)
		{
         transform.position = new Vector3(23.0f, 0, screenPos.z);
		}
		else
		{
			transform.position = new Vector3(screenPos.x*0.001f, 0, screenPos.z);
		}
		
		
//		transform.position = new Vector3(screenPos.x*0.001f, 0, screenPos.z); 

/*		
		poslimit.Add(transform.position);//populate list
//		poslimit.Sort();
 		Vector3 min = poslimit[0];               // min, or
 		Vector3 max = poslimit[poslimit.Count - 1];  // max
		
		Debug.Log("Min: "+ min + " Max: " + max);
*/		
	//	Debug.Log("screenPos "+screenPos*0.001f);
	//	Debug.Log("transform.position "+transform.position);
	//	Debug.Log("screenPos.x "+screenPos.x);
	//	Debug.Log("screenPos.y "+screenPos.y);
	//	Debug.Log("screenPos.z "+screenPos.z);
	}
}
