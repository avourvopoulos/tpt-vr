using UnityEngine;
using System.Collections;

public class SelectionTest : MonoBehaviour {
	
	float force = 10.0f;
	float range = 10.0f;
	GameObject selector;

	// Use this for initialization
	void Start () 
	{
		renderer.material.color = Color.grey;
		
		selector = GameObject.Find("Selector");
	}
	
	// Update is called once per frame
	void Update () 
	{
	 
	}
	
	void FixedUpdate ()
	{

   		if(selector) 
		{
      	 selector.rigidbody.AddExplosionForce (force, transform.position, range);
		}
    }
	
	
	 void OnTriggerEnter(Collider other) 
	{
       // GameObject.Find("Selector").rigidbody.AddForce(Vector3.zero); 
    }
	
	void OnCollisionEnter(Collision collision)	
	{
	//	Debug.Log("Ouch!");
		renderer.material.color = Color.red;
	} 
	
	 void OnCollisionExit(Collision collisionInfo)
	{
     //   print("No longer in contact with " + collisionInfo.transform.name);
		renderer.material.color = Color.grey;
    }
}
