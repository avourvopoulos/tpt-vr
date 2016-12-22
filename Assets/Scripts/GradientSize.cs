using UnityEngine;
using System.Collections;

public class GradientSize : MonoBehaviour {
	
	public static bool change=false;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
/*		
		if(change)//if follow point is enabled (on Serial game design only)
		{
		//resize based on lline-of-objects lenghth
	 	transform.localScale = new Vector3((SpawnElements.tiles[SpawnElements.tiles.Count-1].transform.position.x)+5, transform.localScale.y, transform.localScale.z);
		
		//position the gradientbox in the begining (first tile) of the row
		transform.position = new Vector3 ((SpawnElements.tiles[0].transform.position.x)+(transform.localScale.x/2)-3.5f, transform.position.y, transform.position.z);
		
		//Minimap to follow the pointmap
		FollowPoint.follow = true;
			
		change = false;//stop from updating	
*/		
			
		if(GeneralSettings.arch=="Horizontal")//if follow point is enabled (on Serial game design only)
		{
		//resize based on lline-of-objects lenghth
	 	transform.localScale = new Vector3((SpawnElements.tiles[SpawnElements.tiles.Count-1].transform.position.x)+5, transform.localScale.y, transform.localScale.z);	
		//position the gradientbox in the begining (first tile) of the row
		transform.position = new Vector3 ((SpawnElements.tiles[0].transform.position.x)+(transform.localScale.x/2)-3.5f, transform.position.y, transform.position.z);
		//Minimap to follow the pointmap
		FollowPoint.follow = true;	
		}//end if
		
		if(GeneralSettings.arch=="Vertical")//if follow point is enabled (on Serial game design only)
		{
		//resize based on lline-of-objects lenghth
	 	transform.localScale = new Vector3(transform.localScale.x, (SpawnElements.tiles[SpawnElements.tiles.Count-1].transform.position.y)+5, transform.localScale.z);	
		//position the gradientbox in the begining (first tile) of the row
		transform.position = new Vector3 (transform.position.x, (SpawnElements.tiles[0].transform.position.y)+(transform.localScale.y/2)-3.5f, transform.position.z);
		//Minimap to follow the pointmap
		FollowPoint.follow = true;	
		}//end if
		
	}
	
}
