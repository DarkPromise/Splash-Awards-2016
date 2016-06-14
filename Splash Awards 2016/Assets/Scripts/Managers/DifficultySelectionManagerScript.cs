using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DifficultySelectionManagerScript : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    }

    #region Functions

    #region OnClick Functions

    public void ClickDifficulty(string difficulty)
    {
        if (difficulty == "Easy")
        {
            GameControllerScript.Instance.SelectDifficulty(GameControllerScript.DIFFICULTY.EASY);
        }
        else if (difficulty == "Medium")
        {
            GameControllerScript.Instance.SelectDifficulty(GameControllerScript.DIFFICULTY.MEDIUM);
        }
        else if (difficulty == "Hard")
        {
            GameControllerScript.Instance.SelectDifficulty(GameControllerScript.DIFFICULTY.HARD);
        }
    }

    #endregion

    #endregion
}
