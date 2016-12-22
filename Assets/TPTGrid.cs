using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class TPTGrid : MonoBehaviour {
	
	//Toulouse Pieron Textures
	public Texture tp1,tp2,tp3,tp4,tp5,tp6,tp7,tp8;
	
	//Main Tile Element
	public Transform element;
	private float spacing = 1.2f; //space between the tiles
	
	//list for all the spawned elements
	public static List<GameObject> tiles = new List<GameObject>();
	
	//lists for textures
	List<Texture> textureList = new List<Texture>();
	List<Texture> selectionList = new List<Texture>();
	List<Texture> randTextures = new List<Texture>();
	
	public static int columns = 5;
	public static int rows = 5;
	int linesize = columns*rows;
	int currentIndex;

	// Use this for initialization
	void Awake () 
	{
		Screen.SetResolution(900, 640, false);
		
		Initxt();
		GridObjects();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKey(KeyCode.R))
		{
			print("reload!");
			Application.LoadLevel(0);
		}
		
		if(Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();
		}
		
	}
	
	//initialise textures
	void Initxt () 
	{
    	//populating list with the Toulouse Pieron textures
		textureList.Add(tp1);
		textureList.Add(tp2);
		textureList.Add(tp3);
		textureList.Add(tp4);	
		textureList.Add(tp5);
		textureList.Add(tp6);
		textureList.Add(tp7);
		textureList.Add(tp8);
		
		//3 random textures
		initTexture();
	}
	
	void OnGUI () 
	{
		//Correct Choices Group
		GUI.BeginGroup(new Rect((Screen.width/2)-170, 10, 260, 80));
        GUI.Box(new Rect(0, 0, 260, 80), " ");
//		GUI.Box(new Rect(0, 0, 230, 90), " ");
		//three random selection textures
		GUI.DrawTexture(new Rect(10, 15, 60, 60), selectionList[0]);// Debug.Log("+ selectionList[0]: "+selectionList[0]);
		GUI.DrawTexture(new Rect(100, 15, 60, 60), selectionList[1]);// Debug.Log("+ selectionList[1]: "+selectionList[1]);	(Screen.height/2)-220
		GUI.DrawTexture(new Rect(190, 15, 60, 60), selectionList[2]);// Debug.Log("+ selectionList[2]: "+selectionList[2]);
		GUI.EndGroup();
		
		
	}
	
	//return a random toulouse pieron(tp) texture
	Texture randTexture()
	{
		int rand = UnityEngine.Random.Range(0, textureList.Capacity);
		Texture tp = textureList[rand];
		
		return tp;
	}
	
	
	//populate the list with 3 random textures to display
	void initTexture()
	{
		do 
		{
			Texture randtxt;
			if(!selectionList.Contains(randtxt = randTexture()))// Debug.Log("* Contains: "+randtxt);
			{
			//	Thread.Sleep(100);
				selectionList.Add(randtxt);// Debug.Log("* selectionList.Add "+randtxt);
			}
		} while (selectionList.Count < 3);
	}
	
	
	//finds how many symbols are the same with the ones 
	int CorrectChoices()
	{
		int correctCount=0;
		for(int j=0; j<selectionList.Count; j++)
        {
			foreach (Texture i in randTextures)
			{	
				if(i == selectionList[j])
				{
					 correctCount++; 
				}//if
				
			}//foreach
        }//for
		 return correctCount;
	}
	
	//Create Grid
	void GridObjects()
	{
		int i=1;//increment for name
		for (int y = 0; y < rows; y++) { //rows
			for (int x = 0; x < columns; x++) { //columns	
	 
			Transform t = Instantiate(element, new Vector3((x+x)*spacing, (y+y)*spacing, 0), Quaternion.identity) as Transform;//objects to transform
			GameObject go = t.gameObject; //convert transforms into gameobjects
			//	go.transform.parent = element;
			go.name= "Tile"+i++;//set the name of the object
			go.renderer.material.mainTexture = randTexture(); //assign random texture
			go.renderer.material.mainTextureScale = new Vector2(-1, -1);//flip the texture
			tiles.Add(go);//stored in the list
				
			randTextures.Add(go.renderer.material.mainTexture);//add the allocated (random) texture on a list
			}
		}
	 	element.transform.position = new Vector3(0,2000,0);
	}//createObjects()
	
}
