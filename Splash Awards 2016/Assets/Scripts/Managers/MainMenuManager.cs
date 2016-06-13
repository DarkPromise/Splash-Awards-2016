using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    #region Functions

    #region OnClick Functions

    public void ClickPlay()
    {
        SceneManager.LoadScene("DifficultySelection");
    }
    public void ClickExit()
    {
        Application.Quit();
    }

    #endregion

    #endregion
}
