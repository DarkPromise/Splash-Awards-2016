using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonInteraction : MonoBehaviour {

    public void loadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName); 
    }

    public void endGame()
    {
        Application.Quit();
    }
}
