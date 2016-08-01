using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Xml;
using System.IO;

public class GameControllerScript : MonoBehaviour {

    // Make global
    public static GameControllerScript Instance
    {
        get;
        set;
    }

    private string m_CurrentSceneName = null;

    void Awake()
    {
        // If there is an gameobject, don't destroy it and point Instance to this
        if (transform.gameObject)
        {
            DontDestroyOnLoad(transform.gameObject);
            Instance = this;
        }
    }

    ~GameControllerScript()
    {
        ClearInfo();
    }

    /* Data variables */
    // Difficullty
    public enum DIFFICULTY
    {
        NONE,
        EASY,
        MEDIUM,
        HARD
    };
    private DIFFICULTY m_DifficultySelected = DIFFICULTY.NONE;
    // Sub Theme
    public enum SUB_THEME
    {
        CYBER_BULLYING = 0,
        MAKING_FRIENDS_ONLINE,
        SHARING_PERSONAL_INFORMATION_ONLINE,
        CYBER_SECURITY,
        HARMFUL_CONTENT_AND_INTERNET_OR_GAMING_ADDICTION,
        NUM_OF_SUB_THEMES,
    }
    public List<SUB_THEME> m_CurrentSubThemes = new List<SUB_THEME>();
    public List<GameObject> m_lCaseList = new List<GameObject>();
    public List<Case> m_lCaseInfoList = new List<Case>();
    private bool m_bFirstTimeLoadCases = true;
    public Case m_currentCase = null;
    private int m_nCasesActivated = 0;

	// Use this for initialization
	void Start () {
        if(m_CurrentSceneName == null)
        {
            m_CurrentSceneName = SceneManager.GetActiveScene().name;
        }
	}

	// Update is called once per frame
	void Update () {
        if (CheckIfSceneChnaged())
        {
            m_CurrentSceneName = SceneManager.GetActiveScene().name;
            switch (m_CurrentSceneName)
            {
                case "GameTemplate":
                    SetGameSceneToCurrentSubThemes();
                    break;
                case "GuideBook":
                    foreach(Transform child in transform)
                    {
                        GuideScript toChange = GameObject.Find("GuideBook").transform.FindChild(child.name).GetComponent<GuideScript>();
                        toChange.Setup(child);
                    }
                    break;
                case "Cutscene":
                    {
                        if (m_currentCase.m_szCutsceneName != null)
                            GameObject.Find("CutsceneManager").GetComponent<CutsceneManager>().SetCutscene(m_currentCase.m_szCutsceneName);
                    }
                    break;
                case "DialogueScene":
                    {
                        if (m_currentCase.m_szCutsceneName != null)
                            GameObject.Find("DialogueManager").GetComponent<DialogueManager>().SetDialogueScene(m_currentCase.m_szCutsceneName);
                    }
                    break;
                case "CaseScene":
                    {
                        if (m_currentCase != null)
                            GameObject.Find("CaseSceneManager").GetComponent<CaseSceneManager>().SetCaseScene(m_currentCase);
                    }
                    break;
            }
        }
        //if (m_CurrentSceneName == "GameTemplate")
        //{
        //    GameManagerScript gms = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        //    switch (gms.m_CurrentState)
        //    {
        //        case GameManagerScript.CURRENT__STATE.PLAYING:
        //            {
        //                if (gms.m_NumberOfSuspiciousObjects == gms.m_NumberOfSuspiciousObjectsFound)
        //                {
        //                    // Load what the victim did wrong
        //                    gms.LoadWhatTheVictimHadDoneWrong(m_DifficultySelected);
        //                }
        //            }
        //            break;
        //        case GameManagerScript.CURRENT__STATE.VICTIM_DONE_WRONG:
        //            {
        //                if (gms.m_NumberOfWrongObjectsLeft == 0)
        //                {
        //                    gms.LoadPrevention(m_DifficultySelected);
        //                }
        //            }
        //            break;
        //        case GameManagerScript.CURRENT__STATE.PREVENTION:
        //            {
        //                if (gms.m_NumberOfPreventionsLeft == 0)
        //                {
        //                    gms.ClickQuit();
        //                    SceneManager.LoadScene("Mainmenu");
        //                }
        //            }
        //            break;
        //    }
        //}
    }

    #region Functions

    public void ClearInfo()
    {
        m_CurrentSubThemes.Clear();
        m_CurrentSubThemes.TrimExcess();
        m_lCaseList.Clear();
        m_lCaseInfoList.TrimExcess();
        m_lCaseInfoList.Clear();
        m_lCaseInfoList.TrimExcess();
    }

    // Check if SceneChanged
    private bool CheckIfSceneChnaged()
    {
        return m_CurrentSceneName != SceneManager.GetActiveScene().name;
    }

    // Select and choose random sub themes 
    public void SelectDifficulty(DIFFICULTY difficulty)
    {
        m_DifficultySelected = difficulty;
        ChooseRandomSubThemes();
    }

    // Choose random sub themes 
    private void ChooseRandomSubThemes()
    {
        m_CurrentSubThemes.Clear();
        m_CurrentSubThemes.TrimExcess();
        // Choose random sub themes based on difficulty
        switch (m_DifficultySelected)
        {
            case DIFFICULTY.EASY:
                {
                    // Choose an sub themme for easy difficulty
                    SUB_THEME randomSubTheme = (SUB_THEME)Random.Range(0, (int)SUB_THEME.NUM_OF_SUB_THEMES - 1);
                    m_CurrentSubThemes.Add(randomSubTheme);
                }
                break;
            default: 
                {
                    // Every Available Subtheme
                    for(int i = 0; i < (int)SUB_THEME.NUM_OF_SUB_THEMES; i++)
                    {
                        m_CurrentSubThemes.Add((SUB_THEME)i);
                    }
                }
                break;
        }
        // Trim excess spaces
        m_CurrentSubThemes.TrimExcess();
        // Load all case infos
        LoadAllCaseInfos();
        m_bFirstTimeLoadCases = true;
    }

    public void SetGameSceneToCurrentSubThemes()
    {
        // Set cases
        Transform slots = GameObject.Find("UI").transform.FindChild("CaseFiles").FindChild("Slots");

        // Create multiple cases
        LoadAllCases();
        m_nCasesActivated = 0;
        if (m_bFirstTimeLoadCases)
        {
            for (int i = 1; i <= m_CurrentSubThemes.Count; i++)
            {
                // Add Random Case Component To Case
                AddRandomCase(m_CurrentSubThemes[i - 1], slots.FindChild("Slot " + i.ToString()));
            }
            m_bFirstTimeLoadCases = false;
        }
        else
        {
            int slotNumber = 1;
            for (int i = 0; i < m_lCaseInfoList.Count; i++)
            {
                if (m_lCaseInfoList[i].m_bActivated)
                    // Add Activated Case
                    AddCase(i, slots.FindChild("Slot " + (slotNumber++).ToString()));
            }
        }

        // Set guides
        Transform gb = GameObject.Find("UI").transform.FindChild("GuideBook");
        foreach (Transform child in transform)
        {
            GuideScript toChange = gb.FindChild(child.name).GetComponent<GuideScript>();
            foreach (SUB_THEME subTheme in m_CurrentSubThemes)
            {
                SUB_THEME childSubTheme = child.GetComponent<GuideScript>().SubThemeBelongTo;
                if (subTheme == childSubTheme)
                {
                    toChange.Setup(child);
                    gb.FindChild(child.name).gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

    void LoadAllCaseInfos()
    {
        m_lCaseInfoList.Clear();
        m_lCaseInfoList.TrimExcess();
        TextAsset textAsset = (TextAsset)Resources.Load("XML/Case", typeof(TextAsset));
        XmlReader reader = new XmlTextReader(new StringReader(textAsset.text));
        while (reader.Read())
        {
            if (reader.IsStartElement("themes"))
            {
                int maxNumOfThemes = int.Parse(reader.GetAttribute("numOfThemes"));
                reader.Read();
                for (int i = 0; i < maxNumOfThemes; i++)
                {
                    if (reader.IsStartElement("theme"))
                    {
                        SUB_THEME currentTheme = (SUB_THEME)System.Enum.Parse(typeof(SUB_THEME), reader.GetAttribute("name"));
                        int maxNumOfCases = int.Parse(reader.GetAttribute("numOfCases"));
                        reader.Read();
                        for (int j = 0; j < maxNumOfCases; j++)
                        {
                            if (reader.IsStartElement("case"))
                            {
                                // Add Case Info
                                Case newCaseInfo = new Case();
                                m_lCaseInfoList.Add(newCaseInfo);

                                // Set case
                                newCaseInfo.m_theme = currentTheme;
                                newCaseInfo.m_szCutsceneName = reader.GetAttribute("cutscene");

                                string path = reader.GetAttribute("background");
                                newCaseInfo.m_background = Resources.Load<Sprite>(path);

                                int maxNumOfCaseObjects = int.Parse(reader.GetAttribute("numOfObjects"));
                                reader.Read();
                                for (int k = 0; k < maxNumOfCaseObjects; k++)
                                {
                                    if (reader.IsStartElement("caseObject"))
                                    {
                                        CaseObject newCaseObject = newCaseInfo.AddObject();
                                        newCaseObject.m_name = reader.GetAttribute("name");
                                        newCaseObject.m_eType = (CaseObject.CASE_OBJECT_TYPE)System.Enum.Parse(typeof(CaseObject.CASE_OBJECT_TYPE), reader.GetAttribute("type"));
                                        newCaseObject.m_topOffset = float.Parse(reader.GetAttribute("topOffset"));
                                        newCaseObject.m_bottomOffset = float.Parse(reader.GetAttribute("bottomOffset"));
                                        newCaseObject.m_leftOffset = float.Parse(reader.GetAttribute("leftOffset"));
                                        newCaseObject.m_rightOffset = float.Parse(reader.GetAttribute("rightOffset"));

                                        reader.ReadToNextSibling("caseObject");
                                    }
                                }
                                reader.ReadOuterXml();
                                reader.ReadToNextSibling("case");
                            }
                        }
                        reader.ReadOuterXml();
                        reader.ReadToNextSibling("theme");
                    }
                }
            }
            reader.Close();
            return;
        }
    }
    void LoadAllCases()
    {
        m_lCaseList.Clear();
        m_lCaseList.TrimExcess();
        TextAsset textAsset = (TextAsset)Resources.Load("XML/Case", typeof(TextAsset));
        XmlReader reader = new XmlTextReader(new StringReader(textAsset.text));
        while (reader.Read())
        {
            if (reader.IsStartElement("themes"))
            {
                int maxNumOfThemes = int.Parse(reader.GetAttribute("numOfThemes"));
                reader.Read();
                for (int i = 0; i < maxNumOfThemes; i++)
                {
                    if (reader.IsStartElement("theme"))
                    {
                        int maxNumOfCases = int.Parse(reader.GetAttribute("numOfCases"));
                        reader.Read();
                        for (int j = 0; j < maxNumOfCases; j++)
                        {
                            if (reader.IsStartElement("case"))
                            {
                                GameObject newCase = new GameObject();
                                // Add to list
                                newCase.SetActive(false);
                                m_lCaseList.Add(newCase);

                                // Set name
                                newCase.name = reader.GetAttribute("name");

                                // Set general parameter
                                newCase.AddComponent<Image>();
                                
                                reader.ReadOuterXml();
                                reader.ReadToNextSibling("case");
                            }
                        }
                        reader.ReadOuterXml();
                        reader.ReadToNextSibling("theme");
                    }
                }
            }
            reader.Close();
            return;
        }
    }

    private void AddRandomCase(SUB_THEME theme, Transform parentSlot)
    {
        int numOfCasesTOChoose = 0;
        int i = 0;
        for (i = 0; i < m_lCaseInfoList.Count; i++)
        {
            if (m_lCaseInfoList[i].m_theme == theme)
            {
                numOfCasesTOChoose++;
            }
            else if (numOfCasesTOChoose != 0 && m_lCaseInfoList[i].m_theme != theme)
            {
                break;
            }
        }
        i -= numOfCasesTOChoose;

        int randomIndex;
        Case newCaseInfo = null;
        int loopAmount = 0;
        do {
            randomIndex = Random.Range(i + 1, i + numOfCasesTOChoose);
            newCaseInfo = m_lCaseInfoList[randomIndex];
            if (loopAmount++ > 100)
                break;
        } while (newCaseInfo.m_bActivated);

        // Set Active
        newCaseInfo.m_bActivated = true;

        // Add case
        AddCase(randomIndex, parentSlot);
    }

    private void AddCase(int index, Transform parentSlot)
    {
        GameObject newCase = m_lCaseList[index];

        // Set Active
        newCase.SetActive(true);

        // Set parent to respective slots
        newCase.transform.SetParent(parentSlot);
        // Add image
        newCase.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Circle");
        m_nCasesActivated++;
        // Set rect transform
        RectTransform rectTransform = newCase.GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.one;
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
    }

    #endregion
}
