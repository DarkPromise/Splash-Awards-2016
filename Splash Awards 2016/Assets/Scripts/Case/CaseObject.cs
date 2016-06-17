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
    };

    private CASE_OBJECT_TYPE m_eType;
    private List<string> m_lText;

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
    public CaseObject(CASE_OBJECT_TYPE coType)
    {
        this.m_eType = coType;

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
