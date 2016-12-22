using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {
	
	public GameObject EndU, EndD, EndL, EndR;
	
	float horizontal = 0f;
	float vertical = 0f;
	
	float speed;//tile speed
	
	public Camera minimap;
	
	float xmin,xmax,ymin,ymax = 0.0f;//x,y borders

	// Use this for initialization
	void Start () 
	{
	 transform.position = new Vector3 (0, 1.5f, -2);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Get Tile Speed from the Seettings/Pause Menu
		speed = GeneralSettings.TileSpeed;
		
		//for horizontal movement
		if(ArrowBehaviour.arrow=="LEFT")
		{
		  	horizontal = -speed;
		//	Debug.Log(ArrowBehaviour.arrow);
		}
		else if(ArrowBehaviour.arrow=="RIGHT")
		{
			horizontal = speed;
		//	Debug.Log(ArrowBehaviour.arrow);
		}
		else{horizontal = 0f;}
		
		//for vertical movement
		if(ArrowBehaviour.arrow=="UP")
		{
			vertical = speed;
		//	Debug.Log(ArrowBehaviour.arrow);
		}
		else if(ArrowBehaviour.arrow=="DOWN")
		{
			vertical = -speed;
		//	Debug.Log(ArrowBehaviour.arrow);
		}
		else{vertical = 0f;}
		
		//Apply movement
//		transform.Translate(horizontal * Time.deltaTime, vertical * Time.deltaTime, 0);
		
		xmin=(SpawnElements.tiles[0].transform.position.x);
		xmax=(SpawnElements.tiles[SpawnElements.tiles.Count-1].transform.position.x);
		
		ymin=(SpawnElements.tiles[0].transform.position.y);
		ymax=(SpawnElements.tiles[SpawnElements.tiles.Count-1].transform.position.y);
		
		//Apply horizontal movement based on the tiles range
		if (transform.position.x > xmax)
		{
			transform.position = new Vector3(xmax, transform.position.y, transform.position.z);
		}
		
		if (transform.position.x < xmin)
		{
			transform.position = new Vector3(xmin, transform.position.y, transform.position.z);
		}
		
		//Apply vertical movement based on the tiles range
		if (transform.position.y >  ymax)
		{
			transform.position = new Vector3(transform.position.x, ymax, transform.position.z);
		}
		
		if (transform.position.y < ymin)
		{
			transform.position = new Vector3(transform.position.x, ymin, transform.position.z);
		}

		else
		{
			transform.Translate(horizontal * Time.deltaTime, vertical * Time.deltaTime, 0);
		}
		
			showEndButtons();
	}
	
	void showEndButtons()
	{
				//Apply horizontal movement based on the tiles range
		if (transform.position.x >= xmax)
		{
			GameObject.FindGameObjectWithTag("ArrowR").renderer.enabled=false;
		}
		else{GameObject.FindGameObjectWithTag("ArrowR").renderer.enabled=true;}
		
		if (transform.position.x <= xmin)
		{
			GameObject.FindGameObjectWithTag("ArrowL").renderer.enabled=false;
		}
		else{GameObject.FindGameObjectWithTag("ArrowL").renderer.enabled=true;}
		
		//Apply vertical movement based on the tiles range
		if (transform.position.y >=  ymax)
		{
			GameObject.FindGameObjectWithTag("ArrowU").renderer.enabled=false;
		}
		else{GameObject.FindGameObjectWithTag("ArrowU").renderer.enabled=true;}
		
		if (transform.position.y <= ymin)
		{
			GameObject.FindGameObjectWithTag("ArrowD").renderer.enabled=false;
		}
		else{GameObject.FindGameObjectWithTag("ArrowD").renderer.enabled=true;}
		
//		//horizontal 
//		if (transform.position.x < xmax)
//		{
//			EndR.SetActive(true);
//		}
//		if (transform.position.x > xmin)
//		{
//			EndL.SetActive(true);
//		}
//		//vertical
//		if (transform.position.y >  ymax)
//		{ 
//			EndU.SetActive(true);
//		}
//		if (transform.position.y < ymin)
//		{
//			EndD.SetActive(true);
//		}
//		else
//		{
//			EndL.SetActive(false);
//			EndR.SetActive(false);
//			EndU.SetActive(false);
//			EndD.SetActive(false);
//		}
	}
	
	
/*	
	void OnGUI() 
	{
		
		//ToDo: END button apearing in the end of the grid/series 
		if(endXmin==true)
		{
			GUI.Button(new Rect(SpawnElements.tiles[0].transform.position.x, transform.position.y, 50, 30), "END");
			EndL.SetActive(true);
		}
		if(endXmax==true)
		{
			GUI.Button(new Rect(SpawnElements.tiles[SpawnElements.tiles.Count-1].transform.position.x, transform.position.y, 50, 30), "END");
			EndR.SetActive(true);
		}
		if(endYmin==true)
		{
			GUI.Button(new Rect(transform.position.x, SpawnElements.tiles[0].transform.position.y, 50, 30), "END");
			EndU.SetActive(true);
			
		}
		if(endYmax==true)
		{
			GUI.Button(new Rect(transform.position.x, SpawnElements.tiles[SpawnElements.tiles.Count-1].transform.position.y, 50, 30), "END");
			EndD.SetActive(true);
		}
	
	}
	
*/	
	
	
}
