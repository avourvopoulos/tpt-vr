using UnityEngine;
using System.Collections;

public class ArrowBehaviour : MonoBehaviour {

	BoxCollider col;
	
	public static string arrow="n/a";

	// Use this for initialization
	void Start () 
	{
		renderer.material.color = Color.grey;
		
	//	collider = this.collider as BoxCollider;
		col = (BoxCollider)collider;
		
	}
	
	// Update is called once per frame
	void Update () 
	{
//		if(UDPReceive.borderHit==arrow)
//		{
//			gameObject.SetActive(false);
//		}
//		else
//		{
//			gameObject.SetActive(true);
//		}
		
		
//		if(UDPReceive.selection.ToUpper() == "TOBII")
//		{
//			col.size = new Vector3(60,60,30);
//		}
//		else{
//			col.size = new Vector3(60,1240,30);
//		}
	}
	
	void OnCollisionStay(Collision collision) 
	{ 
       if(gameObject.name=="ArrowL")
		{
			arrow="LEFT";
		//	Debug.Log(arrow);
		}
		if(gameObject.name=="ArrowR")
		{	
			arrow="RIGHT";
		//	Debug.Log(arrow);
		}
		if(gameObject.name=="ArrowU")
		{
			arrow="UP";
		//	Debug.Log(arrow);
		}
		if(gameObject.name=="ArrowD")
		{	
			arrow="DOWN";
		//	Debug.Log(arrow);
		}	
		
        renderer.material.color = Color.green;
		
//		if(gameObject.name==UDPReceive.borderHit)
//		{
//			gameObject.SetActive(false);
//		}
//		else
//		{
//			gameObject.SetActive(true);
//		}
    }
	
	
	void OnCollisionExit(Collision collisionInfo) 
	{
        if(gameObject.name=="ArrowL")
		{
			arrow="";
		//	Debug.Log("/LEFT");
		}
		if(gameObject.name=="ArrowR")
		{	
			arrow="";
		//	Debug.Log("/RIGHT");
		}
		      if(gameObject.name=="ArrowU")
		{
			arrow="";
		//	Debug.Log("/UP");
		}
		if(gameObject.name=="ArrowD")
		{	
			arrow="";
		//	Debug.Log("/DOWN");
		}	
		
		renderer.material.color = Color.grey;
    }
	
	
}
