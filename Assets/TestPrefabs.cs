using UnityEngine;
using System.Collections;

public class TestPrefabs : MonoBehaviour {
	
	public GameObject element;
	public float gridX = 5f;
	public float gridY = 5f;
	public float spacing = 2f;
	
	public int numberOfObjects = 25;
	public float radius = 5f;

	// Use this for initialization
	void Start () 
	{
		GridTiles();
	//	CircleTiles();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	
	void GridTiles()
	{
	for (int y = 0; y < gridY; y++) 
		{
		for (int x = 0; x < gridX; x++) 
			{
			Vector3 pos = new Vector3(x, y, 0) * spacing;
			Instantiate(element, pos, Quaternion.identity);
			}
		}
	}
	
	
	void CircleTiles() 
	{
	for (int i = 0; i < numberOfObjects; i++) 
		{
		float angle = i * Mathf.PI * 2 / numberOfObjects;
		Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
		Instantiate(element, pos, Quaternion.identity);
		}
	}
	
	
}
