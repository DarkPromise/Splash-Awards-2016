using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Case : MonoBehaviour {

    private List<CaseObject> m_lCaseObjects;

    public void Init(List<GameControllerScript.SUB_THEME> subthemes) 
    {
        this.m_lCaseObjects = new List<CaseObject>();
        int randomTheme = Random.Range(1, subthemes.Count);
        Debug.Log("Theme : " + (GameControllerScript.SUB_THEME)randomTheme);

        // Possibly create Case Objects from a "Premade" list of Cases via reading a .xml file
        // -> Allows to a wide variety of cases to be made just from adding new .xml files into a folder in the project

        switch ((GameControllerScript.SUB_THEME)randomTheme)
        {
            case GameControllerScript.SUB_THEME.CYBER_BULLYING:
                break;
            case GameControllerScript.SUB_THEME.CYBER_SECURITY:
                break;
            case GameControllerScript.SUB_THEME.HARMFUL_CONTENT_AND_INTERNET_OR_GAMING_ADDICTION:
                break;
            case GameControllerScript.SUB_THEME.MAKING_FRIENDS_ONLINE:
                break;
            case GameControllerScript.SUB_THEME.SHARING_PERSONAL_INFORMATION_ONLINE:
                break;
        }

        Debug.Log("Case Initialized");
    } 

    ~Case()
    {
        m_lCaseObjects.Clear();
        m_lCaseObjects.TrimExcess();
    }
}
