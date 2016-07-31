using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueScript : MonoBehaviour {

    public List<string> textList = new List<string>();
    private int currentTextIndex;

	// Use this for initialization
    void Start()
    {
        currentTextIndex = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    #region Functions

    public void SetDialogue(int index)
    {
        this.GetComponent<Text>().text = textList[index];
    }

    #region OnClick Functions
    public void OnClick()
    {
        // Check if there is still text
        if(++currentTextIndex < textList.Count)
        {
            // Change to next text
            SetDialogue(currentTextIndex);
        }
        else
        {
            // Change scene
            SceneManager.LoadScene("GameTemplate");
        }
    }
    #endregion

    #endregion
}
