using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System;
using System.Threading;

public class XmlDataWriter : MonoBehaviour {
	
	public GameObject IKTarget;
		
	static string timestamp = String.Empty;
	static string date = String.Empty;
	public float uptime;
		
	static string filepath = String.Empty;
	XmlDocument xmlDoc;
	private static XmlWriter writer;
	
	public static bool stopXML=false;
	
	public static bool isCalibrating = false;
	
	// Use this for initialization
	void Awake () 
	{	
		date = DateTime.Now.ToString("dd-MM-yyyy"); // get date
		
		if(InitArguments.arg!="nolog")
		{
			XmlInit();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	// 	timestamp = DateTime.Now.ToString("dd-MM-yyyy HH.mm.ss.ffff"); // get time
	//	timestamp = DateTime.Now.ToString("HHmmssffff"); // get time
		
		bool flag = stopXML;
		
		if(InitArguments.arg!="nolog")
		{	
			if(!flag || !GeneralSettings.isPaused)//write if not paused
			{
				uptime+= Time.deltaTime;
				xmlWrite();
			}
			else{endXML();}
		}
	}

	
	public static void XmlInit()
	{
		filepath = Application.dataPath + @"/Log/ID_"+Init.uid+"_"+ DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xml";
		XmlWriterSettings settings = new XmlWriterSettings();
		settings.Indent = true;
		settings.NewLineOnAttributes = true;
		writer = XmlWriter.Create(filepath,settings);
		
		writer.WriteStartDocument();
		writer.WriteComment("Data - " + date);	//comment
		writer.WriteStartElement(Init.uid);
	}
	
	//XML+UDP
	public void xmlWrite()
	{
		timestamp = DateTime.UtcNow.Minute.ToString("00")+DateTime.UtcNow.Second.ToString("00")+DateTime.Now.Millisecond.ToString("0000"); //time in min:sec:usec
		
		writer.WriteStartElement("Time"); 
		writer.WriteAttributeString("mmssuuuu", timestamp); UDPSend.sendString("[$]GameData,[$$]TPT-VR,[$$$]timestamp,mmssuuuu,"+timestamp+";");
		writer.WriteElementString("Uptime", uptime.ToString()); //UDPSend.sendString("Uptime");

		if (GeneralSettings.device=="RawMouse" || GeneralSettings.device=="Mouse")
		{
			writer.WriteStartElement("Mouse");
			writer.WriteElementString("x", Input.mousePosition.x.ToString());  
			writer.WriteElementString("y", Input.mousePosition.y.ToString());
			writer.WriteEndElement();//Mouse
		}
			
		writer.WriteStartElement("IK");
		writer.WriteElementString("x", IKTarget.transform.position.x.ToString());   
		writer.WriteElementString("y", IKTarget.transform.position.y.ToString());
		writer.WriteElementString("z", IKTarget.transform.position.z.ToString());
		writer.WriteEndElement();//IK Target
		
		//IK UDP
		UDPSend.sendString("[$]GameData,[$$]TPT-VR,[$$$]IK,position,"+IKTarget.transform.position.x.ToString()+","+IKTarget.transform.position.y.ToString()+","+IKTarget.transform.position.z.ToString()+","+"0"+";");
		
		writer.WriteStartElement("Clicks");
		writer.WriteElementString("Mouse", ClickSelection.MouseClick().ToString());
		writer.WriteElementString("Keyboard", ClickSelection.KeyboardClick().ToString());
		writer.WriteElementString("TimerOn", GeneralSettings.timerOn.ToString());
		writer.WriteEndElement();//Clicks
		
//		writer.WriteStartElement("ClickedTile");
//		writer.WriteElementString("Name", ClickSelection.tile);
//		writer.WriteElementString("Symbol", ClickSelection.symbol);
//		writer.WriteEndElement();//Tile
		
		writer.WriteStartElement("Score");
		writer.WriteElementString("Correct", GameScoring.GetCorrect().ToString());
		writer.WriteElementString("Wrong", GameScoring.GetError().ToString());
		writer.WriteElementString("KLM", GameScoring.GetKLT().ToString());
		writer.WriteEndElement();//Score
		
	if(ClickSelection.hover)
		{
			writer.WriteStartElement("HoverTile");
			writer.WriteElementString("Position", ClickSelection.tileposition);
			writer.WriteElementString("Name", ClickSelection.tile);
			if(ClickSelection.isSelected)
			{
				writer.WriteElementString("Symbol", ClickSelection.symbol);
			}
			writer.WriteEndElement();//HoverTile
		}
		
		
		if (isCalibrating == true)
		{
			writer.WriteElementString("Calibration", "1");//on
		}
		else
		{
			writer.WriteElementString("Calibration", "0");//off
		}
		
		
	if (GeneralSettings.device=="Tracking")
		{		
			writer.WriteStartElement(UDPReceive.selection);//selected joint
				writer.WriteStartElement("Position");//pos
					writer.WriteElementString("x", UDPReceive.udppos.x.ToString());
					writer.WriteElementString("y", UDPReceive.udppos.y.ToString());
					writer.WriteElementString("z", UDPReceive.udppos.z.ToString());
				writer.WriteEndElement();//end pos
				writer.WriteStartElement("Rotation");//rot
					writer.WriteElementString("x", UDPReceive.udprot.x.ToString());
					writer.WriteElementString("y", UDPReceive.udprot.y.ToString());
					writer.WriteElementString("z", UDPReceive.udprot.z.ToString());
					writer.WriteElementString("w", UDPReceive.udprot.w.ToString());
				writer.WriteEndElement();//end rot
			writer.WriteEndElement();//end joint
				

		}			
		
		
		writer.WriteEndElement();//timestamp
		
	}
	
	public static void endXML()
	{
		print("endXML");
				
		writer.WriteComment("Final KLM Score");	//comment
		writer.WriteStartElement("KLM");
		writer.WriteElementString("Final", GameScoring.GetKLT().ToString());   // <-- These are new
		writer.WriteElementString("Time", GameScoring.time.ToString("00"));
		writer.WriteElementString("CorrectSymbols", SpawnElements.selectionList[0].ToString()+","+SpawnElements.selectionList[1].ToString()+","+SpawnElements.selectionList[2].ToString());
		writer.WriteEndElement();//Score
		
		writer.WriteComment("Tile Grid");	//comment
		writer.WriteStartElement("Tiles");
		int j=0;
			foreach (Texture i in SpawnElements.randTextures)
			{
				writer.WriteElementString("Tile"+j.ToString(), SpawnElements.randTextures[j].ToString());
				j++;
			}
		writer.WriteEndElement();//Tiles
		
		writer.WriteComment("General Settings");//comment	
		writer.WriteStartElement("Settings");
		writer.WriteElementString("GameDesign", Init.dev); 
		writer.WriteElementString("Device", GeneralSettings.device.ToString()); 
		writer.WriteElementString("LeftHand", ikLimbLeft.IsEnabled.ToString());   // <-- These are new
		writer.WriteElementString("RightHand", ikLimbRight.IsEnabled.ToString());
		writer.WriteElementString("HandSpeed", GeneralSettings.HandSpeed.ToString());
		writer.WriteElementString("TileSpeed", GeneralSettings.TileSpeed.ToString());
		writer.WriteEndElement();//Settings	
		
		writer.WriteComment("Low Pass Filter");//Comment	
		writer.WriteStartElement("Filter");
		writer.WriteElementString("ALPHA", UDPReceive.ALPHA.ToString());
		writer.WriteElementString("Delta", UDPReceive.deltaon.ToString());
		writer.WriteEndElement();//Filter
		
		writer.WriteComment("Tracking Calibration");//Comment	
		writer.WriteStartElement("Max");
		writer.WriteElementString("x", UDPReceive.xmax.ToString());
		writer.WriteElementString("y", UDPReceive.ymax.ToString());
		writer.WriteElementString("z", UDPReceive.zmax.ToString());
		writer.WriteEndElement();//Max
		writer.WriteStartElement("Min");
		writer.WriteElementString("x", UDPReceive.xmin.ToString());
		writer.WriteElementString("y", UDPReceive.ymin.ToString());
		writer.WriteElementString("z", UDPReceive.zmin.ToString());
		writer.WriteEndElement();//Min
		
		writer.WriteEndElement();//UID
		
		writer.WriteEndDocument();
		writer.Flush();
		writer.Close();
	}
	
}
