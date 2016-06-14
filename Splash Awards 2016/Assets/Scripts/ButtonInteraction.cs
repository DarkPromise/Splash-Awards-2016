using UnityEngine;
using System.Collections;

public class ButtonInteraction : MonoBehaviour {

    public void loadScene(int buttons)
    {
        Application.LoadLevel(buttons); 
    }

    public void endGame()
    {
        Application.Quit();
    }
}
