using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Xml;
using System.IO;

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
        TextAsset textAsset = (TextAsset)Resources.Load("XML/Dialoguescene", typeof(TextAsset));
        XmlReader reader = new XmlTextReader(new StringReader(textAsset.text));
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
                        string path = reader.GetAttribute("background");
                        Canvas.GetComponent<Image>().sprite = Resources.Load<Sprite>(path);

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

                        for (int k = 0; k < 2; k++)
                        {
                            reader.Read();
                            if (reader.IsStartElement("characterLeft"))
                            {
                                // Load Character Left
                                Transform character = Canvas.transform.FindChild("Characters").FindChild("CharacterLeft");
                                path = reader.GetAttribute("image");
                                character.GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
                                RectTransform rectTransform = character.GetComponent<RectTransform>();
                                rectTransform.sizeDelta = new Vector2(float.Parse(reader.GetAttribute("width")), float.Parse(reader.GetAttribute("height")));
                            }
                            if (reader.IsStartElement("characterRight"))
                            {
                                // Load Character Right
                                Transform character = Canvas.transform.FindChild("Characters").FindChild("CharacterRight");
                                path = reader.GetAttribute("image");
                                character.GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
                                RectTransform rectTransform = character.GetComponent<RectTransform>();
                                rectTransform.sizeDelta = new Vector2(float.Parse(reader.GetAttribute("width")), float.Parse(reader.GetAttribute("height")));
                            }
                        }
                        reader.Close();
                        return;
                    }
                }
            }
        }
    }
}
