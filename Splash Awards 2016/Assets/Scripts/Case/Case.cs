using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Case {
    public GameControllerScript.SUB_THEME m_theme = GameControllerScript.SUB_THEME.CYBER_BULLYING;
    public List<CaseObject> m_lCaseObjects = new List<CaseObject>();
    public string m_szCutsceneName = null;
    public bool m_bActivated = false;
    public Sprite m_background = null;
    public bool m_bFirstTime = true;
    public bool m_bCompleted = false;
    public CaseObject m_currentCaseObject = null;
    public string m_MCQSceneName = null;

    //public void Init(List<GameControllerScript.SUB_THEME> subthemes) 
    //{
    //    int randomTheme = Random.Range(1, subthemes.Count);
    //    theme = subthemes[randomTheme];
    //    Debug.Log("Theme : " + (GameControllerScript.SUB_THEME));

    //    // Possibly create Case Objects from a "Premade" list of Cases via reading a .xml file
    //    // -> Allows to a wide variety of cases to be made just from adding new .xml files into a folder in the project

    //    switch (theme)
    //    {
    //        case GameControllerScript.SUB_THEME.CYBER_BULLYING:
    //            break;
    //        case GameControllerScript.SUB_THEME.CYBER_SECURITY:
    //            break;
    //        case GameControllerScript.SUB_THEME.HARMFUL_CONTENT_AND_INTERNET_OR_GAMING_ADDICTION:
    //            break;
    //        case GameControllerScript.SUB_THEME.MAKING_FRIENDS_ONLINE:
    //            break;
    //        case GameControllerScript.SUB_THEME.SHARING_PERSONAL_INFORMATION_ONLINE:
    //            break;
    //    }

    //    Debug.Log("Case Initialized");
    //}

    public CaseObject AddObject()
    {
        CaseObject newCaseObject = new CaseObject();
        m_lCaseObjects.Add(newCaseObject);

        return newCaseObject;
    }

    ~Case()
    {
        m_lCaseObjects.Clear();
        m_lCaseObjects.TrimExcess();
    }
}
