using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System;

public class SaveConfig : MonoBehaviour {
	
	public static string timestamp = "";
	public static string savetime = "";

	bool LeftHandXML;
	bool RightHandXML;
		
	float HandSpeedXML; 
	float TileSpeedXML; 
	bool TimerOnXML; 

	// Use this for initialization
	void Start () 
	{
		#if !UNITY_WEBPLAYER
		LoadFromXml();
		#endif
	}
	
	// Update is called once per frame
	void Update () 
	{
	//	timestamp = DateTime.Now.ToString("dd-MM-yyyy HH.mm.ss"); // get time	
	}
	
	void OnGUI()
    {
/*		
		GUI.BeginGroup (new Rect (Screen.width / 2 - 500-MainGuiControls.gone, (Screen.height / 2 - 120)*MainGuiControls.optionsmenu, 200, 100));
		GUI.color = Color.yellow;
		GUI.Box (new Rect (0,0,200,100), "Configuration");
		GUI.color = Color.white;
		
		if (GUI.Button(new Rect(40, 30, 50, 20), "Save"))
        {
			WriteToXml();
			Debug.Log("Saved...");
		}
		
		if (GUI.Button(new Rect(110, 30, 50, 20), "Load"))
        {
			LoadFromXml();
			Debug.Log("Loaded...");
		}
		GUI.Label(new Rect(10, 60, 400, 20), "Last Save: "+savetime);
		
		GUI.EndGroup ();
*/		
	}
	
	 public static void WriteToXml()
 	{
		timestamp = DateTime.Now.ToString("dd-MM-yyyy HH.mm.ss");
  	//	savetime = timestamp;
		
	  string filepath = Application.dataPath + @"/SaveConfig.xml";
	  XmlDocument xmlDoc = new XmlDocument();
	  
	  if(File.Exists (filepath))
	  {
	   xmlDoc.Load(filepath);
	    
	   XmlElement elmRoot = xmlDoc.DocumentElement;
	    
	    elmRoot.RemoveAll(); // remove all inside the transforms node.
	    
	    XmlElement elm1 = xmlDoc.CreateElement("Settings"); // create the node.
	    
	     XmlElement TimeXML = xmlDoc.CreateElement("Time"); 
	     TimeXML.InnerText = timestamp; //time 
			
		 XmlElement LeftHandXML = xmlDoc.CreateElement("LeftHand"); 
	     LeftHandXML.InnerText = ikLimbLeft.IsEnabled.ToString(); //bool 
	    
	     XmlElement RightHandXML = xmlDoc.CreateElement("RightHand");
	     RightHandXML.InnerText = ikLimbRight.IsEnabled.ToString(); //bool
	    
	     XmlElement HandSpeedXML = xmlDoc.CreateElement("HandSpeed"); 
	     HandSpeedXML.InnerText = GeneralSettings.HandSpeed.ToString(); // float
			
		 XmlElement TileSpeedXML = xmlDoc.CreateElement("TileSpeed");
	     TileSpeedXML.InnerText = GeneralSettings.TileSpeed.ToString(); //float
			
		 XmlElement TimerOnXML = xmlDoc.CreateElement("TimerOn");
	     TimerOnXML.InnerText = GeneralSettings.timerOn.ToString();//bool
			
		   
		
	   elm1.AppendChild(TimeXML);	
	   elm1.AppendChild(LeftHandXML);			
	   elm1.AppendChild(RightHandXML); 		
	   elm1.AppendChild(HandSpeedXML);			
	   elm1.AppendChild(TileSpeedXML);	
	   elm1.AppendChild(TimerOnXML);

				
	   elmRoot.AppendChild(elm1); // make the transform node the parent.			
	    
	   xmlDoc.Save(filepath); // save file.
	   Debug.Log("Saved...");
	  }
	 }
	
  
	 public static void LoadFromXml()
	 {
	  string filepath = Application.dataPath + @"/SaveConfig.xml";
	  XmlDocument xmlDoc = new XmlDocument();
	  
	  if(File.Exists (filepath))
	  {
	   xmlDoc.Load(filepath);
	    Debug.Log("Loaded...");
			
	   XmlNodeList transformList = xmlDoc.GetElementsByTagName("Settings");
	  
	   foreach (XmlNode transformInfo in transformList)
	   {
	    XmlNodeList xmlcontent = transformInfo.ChildNodes;
	    
	    foreach (XmlNode xmlsettings in xmlcontent)
	    {
	     if(xmlsettings.Name == "Time")
	     {
	      savetime = xmlsettings.InnerText; 
	     }
	     if(xmlsettings.Name == "LeftHand")
	     {
	      ikLimbLeft.IsEnabled = bool.Parse(xmlsettings.InnerText); 
	     }
	     if(xmlsettings.Name == "RightHand")
	     {
	      ikLimbRight.IsEnabled = bool.Parse(xmlsettings.InnerText); 
	     }
	     if(xmlsettings.Name == "HandSpeed")
	     {
	      GeneralSettings.HandSpeed = float.Parse(xmlsettings.InnerText);
	     }
		 if(xmlsettings.Name == "TileSpeed")
	     {
	      GeneralSettings.TileSpeed = float.Parse(xmlsettings.InnerText);
		 }
		 if(xmlsettings.Name == "TimerOn")
	     {
	      GeneralSettings.timerOn = bool.Parse(xmlsettings.InnerText);					
		 }
	      
	    }
	   }
	  }
	  
	 }
	
	
}
