using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Xml;
using UnityEditor;

public class DialogueManager : MonoBehaviour
{
    public GameObject Canvas = null;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetDialogueScene(string name)
    {
        XmlReader reader = XmlReader.Create("Assets/XML/Dialoguescene.xml");
        while (reader.Read())
        {
            if (reader.IsStartElement("dialogueScenes"))
            {
                int maxNumOfDialogueScenes = int.Parse(reader.GetAttribute("numOfDialogueScenes"));
                for (int i = 0; i < maxNumOfDialogueScenes; i++)
                {
                    reader.Read();
                    if (reader.IsStartElement("dialogueScene") && reader.GetAttribute("name") == name)
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
