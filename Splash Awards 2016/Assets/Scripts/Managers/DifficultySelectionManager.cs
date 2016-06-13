using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DifficultySelectionManager : MonoBehaviour {
    
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
        GameController.Instance.SelectDifficulty(GameController.DIFFICULTY.EASY);
        SceneManager.LoadScene("GameTemplate");
    }
    public void ClickMedium()
    {
        GameController.Instance.SelectDifficulty(GameController.DIFFICULTY.MEDIUM);
        SceneManager.LoadScene("GameTemplate");
    }
    public void ClickHard()
    {
        GameController.Instance.SelectDifficulty(GameController.DIFFICULTY.HARD);
        SceneManager.LoadScene("GameTemplate");
    }

    #endregion

    #endregion
}
