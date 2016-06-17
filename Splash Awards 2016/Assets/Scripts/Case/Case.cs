using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Case {

    private List<CaseObject> m_lCaseObjects;

    public Case(List<GameControllerScript.SUB_THEME> subthemes)
    {
        this.m_lCaseObjects = new List<CaseObject>();
        foreach(GameControllerScript.SUB_THEME subtheme in subthemes)
        {
            Debug.Log(subtheme);
        }
        Debug.Log("Case Constructed");
    }

    ~Case()
    {
        m_lCaseObjects.Clear();
        m_lCaseObjects.TrimExcess();
    }
}
