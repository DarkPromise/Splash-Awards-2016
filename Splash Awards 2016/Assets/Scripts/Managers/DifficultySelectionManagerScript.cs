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

    public void ClickEasy()
    {
        GameControllerScript.Instance.SelectDifficulty(GameControllerScript.DIFFICULTY.EASY);
        SceneManager.LoadScene("GameTemplate");
    }
    public void ClickMedium()
    {
        GameControllerScript.Instance.SelectDifficulty(GameControllerScript.DIFFICULTY.MEDIUM);
        SceneManager.LoadScene("GameTemplate");
    }
    public void ClickHard()
    {
        GameControllerScript.Instance.SelectDifficulty(GameControllerScript.DIFFICULTY.HARD);
        SceneManager.LoadScene("GameTemplate");
    }

    #endregion

    #endregion
}
