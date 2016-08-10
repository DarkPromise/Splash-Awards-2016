using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CaseObject {

    public enum CASE_OBJECT_TYPE
    {
        CO_NONE = 0,
        CO_CHATLOG,
        CO_EMAIL,
        CO_SOCIALMEDIA,
        CO_WEBHISTORY,
        CO_DOWNLOADHISTORY,
        CO_GENERALINFORMATION,
        NUM_CO_TYPE
    };

    public CASE_OBJECT_TYPE m_eType;
    private List<string> m_lText;
    public string m_name = null;
    public bool m_bCompleted = false;
    public float m_topOffset = 0.0f;
    public float m_bottomOffset = 0.0f;
    public float m_leftOffset = 0.0f;
    public float m_rightOffset = 0.0f;
    public List<string> m_scenesName = new List<string>();
    public int m_currentSceneIndex = -1;

    void Start()
    {
        m_lText = new List<string>();
    }

    // Constructor
    public CaseObject()
    {
        this.m_eType = CASE_OBJECT_TYPE.CO_NONE;
    }

    // Constructor
    public CaseObject(string name, CASE_OBJECT_TYPE coType)
    {
        this.m_eType = coType;
        m_name = name;
        switch (coType)
        {
            case CASE_OBJECT_TYPE.CO_CHATLOG:
                break;
            case CASE_OBJECT_TYPE.CO_EMAIL:
                break;
            case CASE_OBJECT_TYPE.CO_SOCIALMEDIA:
                break;
            case CASE_OBJECT_TYPE.CO_WEBHISTORY:
                break;
            case CASE_OBJECT_TYPE.CO_DOWNLOADHISTORY:
                break;
            case CASE_OBJECT_TYPE.CO_GENERALINFORMATION:
                break;
            case CASE_OBJECT_TYPE.CO_NONE:
                break;
        }
    }

    // Destructor
    ~CaseObject()
    {
        if (m_lText.Count > 0)
        {
            m_lText.Clear();
            m_lText.TrimExcess();
        }
    }

    void Update()
    {

    }
}
