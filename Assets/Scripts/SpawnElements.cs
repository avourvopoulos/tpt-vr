using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class SpawnElements : MonoBehaviour {
	
	//Toulouse Pieron Textures
	public Texture tp1,tp2,tp3,tp4,tp5,tp6,tp7,tp8;
	
	//Navigation Arrows
	public GameObject arrowUp,arrowDown,arrowLeft,arrowRight;
	
	//Main Tile Element
	public Transform element;
	private float spacing = 0.35f; //space between the tiles
	
	//list for all the spawned elements
	public static List<GameObject> tiles = new List<GameObject>();
	
	//lists for textures
	public static List<Texture> textureList = new List<Texture>();
	public static List<Texture> selectionList = new List<Texture>();
	public static List<Texture> randTextures = new List<Texture>();
	
    public static int columns = 5;
	public static int rows = 5;
	public static int linesize = columns*rows;
	public static int currentIndex;
	
//	public static int correctCount;
	
	void Awake () 
	{
///*		//FOR TEST ONLY:
//		Initxt();
//		GridObjects();
//		GeneralSettings.isPaused=false;
//		VLineObjects();
//		LineObjects();
//		GeneralSettings.device = "Mouse";
//		GeneralSettings.arch="Grid";
//		GeneralSettings.device = "Tracking";
//		GeneralSettings.device = "Keyboard";
//		ikLimbRight.IsEnabled = true;
//*/		////////////////////§/////////

	}
	
	// Use this for initialization
	void Start () 
	{
	//	getCorrectChoices();
	//	Debug.Log("getCorrectChoices: "+CorrectChoices());
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Game Design Selection
		if (Init.isGrid)
		{
			Initxt();//initialise textures
			GridObjects();
			Init.isGrid=false;
			
			GeneralSettings.arch="Grid";
			
			//create 4 arrows
			arrowLeft.SetActive(true);
			arrowRight.SetActive(true);
			arrowUp.SetActive(true);
			arrowDown.SetActive(true);
			
			Debug.Log("getCorrectChoices: "+CorrectChoices());
		}
		if (Init.isHSerial)
		{
			Initxt();//initialise textures
			LineObjects();
			Init.isHSerial=false;
			
			GeneralSettings.arch="Horizontal";
			
			//create 2 arrows (L/R)
			arrowLeft.SetActive(true);
			arrowRight.SetActive(true);
			arrowUp.SetActive(false);
			arrowDown.SetActive(false);
			
			Debug.Log("getCorrectChoices: "+CorrectChoices());
		}
		if (Init.isVSerial)
		{
			Initxt();//initialise textures
			VLineObjects();
			Init.isVSerial=false;
			
			GeneralSettings.arch="Vertical";
			
			//create 2 arrows (U/D)
			arrowLeft.SetActive(false);
			arrowRight.SetActive(false);
			arrowUp.SetActive(true);
			arrowDown.SetActive(true);
			
			Debug.Log("getCorrectChoices: "+CorrectChoices());
		}
		
		//Apply Horizontal and Vertical movement from tracking devices
	//	transform.Translate(Movement.h, Movement.v, 0);
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
		if(!GeneralSettings.isPaused)
	    {
		//Correct Choices Group
		GUI.BeginGroup(new Rect((Screen.width/6), 10, 280, 110));
        GUI.Box(new Rect(0, 0, 280, 110), " ");
//		GUI.Box(new Rect(0, 0, 230, 90), " ");
		//three random selection textures
		GUI.color = Color.green;	
		GUI.DrawTexture(new Rect(10, 15, 80, 80), selectionList[0]);// Debug.Log("+ selectionList[0]: "+selectionList[0]);
		GUI.DrawTexture(new Rect(100, 15, 80, 80), selectionList[1]);// Debug.Log("+ selectionList[1]: "+selectionList[1]);	(Screen.height/2)-220
		GUI.DrawTexture(new Rect(190, 15, 80, 80), selectionList[2]);// Debug.Log("+ selectionList[2]: "+selectionList[2]);
		
		GUI.EndGroup();	
		}
	}
	
	//return a random toulouse pieron(tp) texture
	public static Texture randTexture()
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
	public static int CorrectChoices()
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
	
	//Create Horizontal Line of Objects
	void LineObjects()
	{
		for (int i = 0; i < linesize; i++) { //rows
				
			Transform t = Instantiate(element, new Vector3((i+i)*spacing,1.5f, 0), Quaternion.identity) as Transform;//objects to transform
			GameObject go = t.gameObject; //convert transforms into gameobjects
			go.name= "Tile"+i;//set the name of the object
			go.renderer.material.mainTexture = randTexture(); //assign random texture
			go.renderer.material.mainTextureScale = new Vector2(-1, -1);//flip the texture
			go.renderer.material.SetColor("_Color",Color.white);//change tile color to white
			tiles.Add(go);//stored in the list
			
			randTextures.Add(go.renderer.material.mainTexture);//add the allocated (random) texture on a list
		}
	 	element.transform.position = new Vector3(0,2000,0);
		
		//Change gradient size flag
		GradientSize.change=true;
		
	}//HlineObjects()
	
	
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
			go.renderer.material.SetColor("_Color",Color.white);//change tile color to white
			tiles.Add(go);//stored in the list
				
			randTextures.Add(go.renderer.material.mainTexture);//add the allocated (random) texture on a list
			}
		}
	 	element.transform.position = new Vector3(0,2000,0);
	}//createObjects()
	
	
	//Create Vertical Line of Objects
	void VLineObjects()
	{
		for (int i = 0; i < linesize; i++) { //rows
				
			Transform t = Instantiate(element, new Vector3(1.5f,(i+i)*spacing, 0), Quaternion.identity) as Transform;//objects to transform
			GameObject go = t.gameObject; //convert transforms into gameobjects
			go.name= "Tile"+i;//set the name of the object
			go.renderer.material.mainTexture = randTexture(); //assign random texture
			go.renderer.material.mainTextureScale = new Vector2(-1, -1);//flip the texture
			tiles.Add(go);//stored in the list
			
			randTextures.Add(go.renderer.material.mainTexture);//add the allocated (random) texture on a list
		}
	 	element.transform.position = new Vector3(0,2000,0);
		
		//Change gradient size flag
		GradientSize.change=true;
		
	}//VlineObjects()
	
	
	
}
