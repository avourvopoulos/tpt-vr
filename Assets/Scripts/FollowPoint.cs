using UnityEngine;
using System.Collections;

public class FollowPoint : MonoBehaviour {
	
	// The target we are following
	public Transform target;
	// The distance in the x-z plane to the target
	public float distance = 10.0f;
	// the height we want the camera to be above the target
	public float height = 5.0f;
	public float width = 2.0f;//
	// How much we 
	public float widthDamping = 2.0f;
	public float heightDamping = 2.0f;
	public float rotationDamping = 3.0f;
	
	float wantedRotationAngle;
	float wantedHeight;
	float wantedWidth;
	float currentHeight;
	float  currentWidth;
		
	float currentRotationAngle;
		
	public static bool follow=false;
	
	void LateUpdate () 
	{
	// Early out if we don't have a target
	if (!target)
		return;
	
	//follow only when Line is activated, not grid
	else if(follow)
	{

	// Calculate the current rotation angles
	 wantedRotationAngle = target.eulerAngles.y;
	 wantedHeight = target.position.y + height;
	 wantedWidth = target.position.x + width;
		
//	 currentRotationAngle = transform.eulerAngles.y;
	 currentHeight = transform.position.y;
	 currentWidth = transform.position.x;
	
	// Damp the rotation around the y-axis
	 currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

	// Damp the height
	 currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);
	
	// Damp the width
	 currentWidth = Mathf.Lerp (currentWidth, wantedWidth, widthDamping * Time.deltaTime);

	// Convert the angle into a rotation
	 Quaternion currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);
//	Quaternion currentRotation = newOrientation();
//	Quaternion currentRotation = Quaternion.Euler (0, currentRotationAngle, newOrientation());
		
	
	// Set the position of the camera on the x-z plane to:
	// distance meters behind the target
	transform.position = target.position;
//	transform.position -= currentRotation * Vector3.forward * distance;
	transform.position -=  Vector3.forward * distance;

	// Set the height of the camera
//	transform.position.y = currentHeight;
	
	// Set the width of the camera
//	transform.position.x = currentWidth;
		
	transform.position = new Vector3 (currentWidth, currentHeight, transform.position.z);
	
	
	// Always look at the target
	transform.LookAt (target);
					
		}//if mouse
		
		
	}
	
		
	
}
