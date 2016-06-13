using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SplashScreenManager : MonoBehaviour {
    [Range(0.0f, 10.0f)]
    public float m_TimeCountDown = 0.0f; 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        m_TimeCountDown -= Time.deltaTime;
        if (m_TimeCountDown <= 0.0f)
        {
            SceneManager.LoadScene("MainMenu");
        }
	}
}
