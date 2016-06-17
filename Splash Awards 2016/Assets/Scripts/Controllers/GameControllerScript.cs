using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
        SHARING_PERSONAL_iNFORMATION_ONLINE,
        CYBER_SECURITY,
        HARMFUL_CONTENT_AND_INTERNET_OR_GAMING_ADDICTION,
        NUM_OF_SUB_THEMES
    }
    public List<SUB_THEME> m_CurrentSubThemes = new List<SUB_THEME>();

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
            }
        }
        if (m_CurrentSceneName == "GameTemplate")
        {
            GameManagerScript gms = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
            switch (gms.m_CurrentState)
            {
                case GameManagerScript.CURRENT__STATE.PLAYING:
                    {
                        //if (gms.m_NumberOfSuspiciousObjects == gms.m_NumberOfSuspiciousObjectsFound)
                        //{
                        //    // Load what the victim did wrong
                        //    gms.LoadWhatTheVictimHadDoneWrong(m_DifficultySelected);
                        //}
                    }
                    break;
                case GameManagerScript.CURRENT__STATE.VICTIM_DONE_WRONG:
                    {
                        //if (gms.m_NumberOfWrongObjectsLeft == 0)
                        //{
                        //    gms.LoadPrevention(m_DifficultySelected);
                        //}
                    }
                    break;
                case GameManagerScript.CURRENT__STATE.PREVENTION:
                    {
                        //if (gms.m_NumberOfPreventionsLeft == 0)
                        //{
                        //    gms.ClickQuit();
                        //    SceneManager.LoadScene("Mainmenu");
                        //}
                    }
                    break;
            }
        }
	}

    #region Functions

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
                    // Choose multiple sub themmes for other difficulties
                    int noOfSubTheme = Random.Range(2, (int)SUB_THEME.NUM_OF_SUB_THEMES);
                    while (m_CurrentSubThemes.Count != noOfSubTheme)
                    {
                        SUB_THEME randomSubTheme;
                        while (true)
                        {
                            randomSubTheme = (SUB_THEME)Random.Range(0, (int)SUB_THEME.NUM_OF_SUB_THEMES - 1);
                            int i;
                            for (i = 0; i < m_CurrentSubThemes.Count; i++)
                            {
                                if (m_CurrentSubThemes[i] == randomSubTheme)
                                    break;
                            }
                            if (i == m_CurrentSubThemes.Count)
                                break;
                        }
                        m_CurrentSubThemes.Add(randomSubTheme);
                    }
                }
                break;
        }
        // Trim excess spaces
        m_CurrentSubThemes.TrimExcess();
    }
    // Clear all the sub themes
    public void CLearSubThemes()
    {
        m_CurrentSubThemes.Clear();
    }

    public void SetGameSceneToCurrentSubThemes()
    {
        // Create multiple cases
        for (int i = 0; i < 4; i++)
        {
            Case newCase = new Case(m_CurrentSubThemes);
        }

        // Set guides
        GameObject gb = GameObject.Find("GuideBook");
        foreach (Transform child in transform)
        {
            GuideScript toChange = gb.transform.FindChild(child.name).GetComponent<GuideScript>();
            foreach (SUB_THEME subTheme in m_CurrentSubThemes)
            {
                SUB_THEME childSubTheme = child.GetComponent<GuideScript>().SubThemeBelongTo;
                if (subTheme == childSubTheme)
                {
                    toChange.Setup(child);
                    gb.transform.FindChild(child.name).gameObject.SetActive(true);
                    break;
                }
            }
        }
        gb.SetActive(false);
    }

    #endregion
}
