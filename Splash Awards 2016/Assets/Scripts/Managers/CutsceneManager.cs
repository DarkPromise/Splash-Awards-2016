using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Xml;
using System.IO;
using UnityEngine.SceneManagement;


public class CutsceneManager : MonoBehaviour
{
    public GameObject Canvas = null;
    public List<Sprite> spriteList = new List<Sprite>();
    private int currentIndex = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            // Load next image
            if (++currentIndex < spriteList.Count)
            {
                Canvas.GetComponent<Image>().sprite = spriteList[currentIndex];
            }
            else
            {
                SceneManager.LoadScene("DialogueScene");
            }
        }
	}

    public void SetCutscene(string name)
    {
        TextAsset textAsset = (TextAsset)Resources.Load("XML/Cutscene", typeof(TextAsset));
        XmlReader reader = new XmlTextReader(new StringReader(textAsset.text));
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

                       int maxNumOfImages = int.Parse(reader.GetAttribute("numOfImages"));
                       for (int j = 0; j < maxNumOfImages; j++)
                       {
                           reader.Read();
                           if (reader.IsStartElement("image"))
                           {
                               string path = reader.ReadString();
                               spriteList.Add(Resources.Load<Sprite>(path));

                           }
                       }
                       reader.Close();
                       // Load frist image
                       Canvas.GetComponent<Image>().sprite = spriteList[currentIndex];
                       return;
                   }
               }
           }
        }
    }
}
