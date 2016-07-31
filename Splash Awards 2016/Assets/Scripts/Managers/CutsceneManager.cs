using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Xml;
using UnityEditor;

public class CutsceneManager : MonoBehaviour
{
    public GameObject Canvas = null;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetCutscene(string name)
    {
        XmlReader reader = XmlReader.Create("Assets/XML/Cutscene.xml");
        while(reader.Read())
        {
           if(reader.IsStartElement("cutscenes"))
           {
               int maxNumOfCutscenes = int.Parse(reader.GetAttribute("numOfCutscenes"));
               for (int i = 0; i < maxNumOfCutscenes; i++)
               {
                   reader.Read();
                   if (reader.IsStartElement("cutscene") && reader.GetAttribute("name") == name)
                   {
                       // Load background
                       Canvas.GetComponent<Image>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>(reader.GetAttribute("background"));

                       // Load Character Left
                       Canvas.transform.FindChild("Characters").FindChild("CharacterLeft").GetComponent<Image>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>(reader.GetAttribute("characterLeft"));

                       // Load Character Right
                       Canvas.transform.FindChild("Characters").FindChild("CharacterRight").GetComponent<Image>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>(reader.GetAttribute("characterRight"));

                       // Get Dialogues
                       List<string> dialogueList = new List<string>();

                       int maxNumOfDialogues = int.Parse(reader.GetAttribute("numOfDialogues"));
                       for (int j = 0; j < maxNumOfDialogues; j++)
                       {
                           reader.Read();
                           if (reader.IsStartElement("dialogue"))
                           {
                               //fill strings
                               string dialogue = reader.ReadString();
                               dialogueList.Add(dialogue);
                           }
                       }

                       // Set Dialogues
                       Canvas.transform.FindChild("DialogueBox").FindChild("Dialogue").GetComponent<DialogueScript>().textList = dialogueList;
                       Canvas.transform.FindChild("DialogueBox").FindChild("Dialogue").GetComponent<DialogueScript>().SetDialogue(0);

                       reader.Close();
                       return;
                   }
               }
           }
        }
    }
}
