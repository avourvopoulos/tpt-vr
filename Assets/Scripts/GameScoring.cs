using UnityEngine;
using System.Collections;
/*
 * Formula:
 * KLT% = C-(E+O)*100/Symbols
 * */

public class GameScoring : MonoBehaviour {

	static int correct;//C
	static int error;//E
	static int omissions;//O
	static int symbols;//No. of TP symbols
	static int KLT;//final score in %
	
	public static float time;
	
	public GUIStyle textStyle;
	
	
	// Use this for initialization
	void Start () 
	{
	//	omissions = SpawnElements.linesize;
	//	omissions = SpawnElements.CorrectChoices()-correct;
	//	Debug.Log("getCorrectChoices: "+TextureShuffle.correctCount);
	}
	
	// Update is called once per frame
	void Update () 
	{
		time = Time.time;
		
		omissions = SpawnElements.CorrectChoices()-correct;
/*		
		if(correct == SpawnElements.CorrectChoices())
		{
			print("GAME OVER!");
		}
*/		
		

	}
	
	
	void OnGUI () 
	{
		
/*		if(GeneralSettings.isPaused)
	    {
			GUI.BeginGroup (new Rect (Screen.width/2 - 50, (Screen.height / 2 + 130), 150, 130));
			GUI.color = Color.yellow;
			GUI.Box (new Rect (0,0,150,130), "KLM");
			GUI.color = Color.white;
			
			GUI.Label(new Rect(10, 40, 80, 25), "Correct: " + GetCorrect());
			GUI.Label(new Rect(10, 60, 80, 25), "Wrong: " + GetError());
			GUI.Label(new Rect(10, 100, 80, 55), "Final: " + GetKLT()+ " %");
			
			GUI.EndGroup ();
		}
*/		
		
//		GUI.Label(new Rect((Screen.width/2)-100, Screen.height-70, 80, 55),  (SpawnElements.CorrectChoices()-correct)+ "", textStyle);
		
	}
	
	//Correct Answers
	public static int GetCorrect(){return correct;}
    public static void SetCorrect(int n_correct){correct = correct+n_correct;}
	
	public static void DeleteCorrect(int del_correct){correct = correct-del_correct;}
		
	//Error Answers
	public static int GetError(){return error;}
    public static void SetError(int n_error){error = error+n_error;}
	
	public static void DeleteError(int del_error){error = error-del_error;}
		
	//Omissioned Answers
	public static int GetOmissions(){return omissions;}
    public static void SetOmissions(int n_omissions){omissions = omissions+n_omissions;}
	
	public static void DelOmission(int n_omissions){omissions = omissions-n_omissions;}
	
	//Number of Symbols
	public static int GetSymbols(){return symbols;}
    public static void SetSymbols(int n_symbols){symbols = n_symbols;}
	
	//Final KLT
	public static int GetKLT()
	{
		if(SpawnElements.CorrectChoices()!=0)
		{
		KLT = (correct - (error + omissions))*100/SpawnElements.CorrectChoices();
		return KLT;
		}
		else
		{
			return 0;
		}
	}
	
}
