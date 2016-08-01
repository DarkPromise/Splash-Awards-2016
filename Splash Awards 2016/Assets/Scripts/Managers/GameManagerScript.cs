using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour {
    public GameObject Guidebook = null;
    public GameObject CaseFiles = null;
    [HideInInspector]
    public int m_NumberOfSuspiciousObjects = 0;
    [HideInInspector]
    public int m_NumberOfSuspiciousObjectsFound;
    [HideInInspector]
    public int m_NumberOfWrongObjectsLeft = 0;
    [HideInInspector]
    public int m_NumberOfPreventionsLeft = 0;

    private Transform slotSelected = null;
    private float timeToFullyExpandSlot = 0.25f;
    private Vector2 speedToFullyExpandSlot = new Vector2();
    private Vector2 speedToCenter = new Vector2();

    public enum CURRENT_STATE
    {
        PLAYING = 0,
        OPENING_CASE,
        OPENED_CASE,
        //VICTIM_DONE_WRONG,
        //PREVENTION
    }
    [HideInInspector]
    public CURRENT_STATE m_CurrentState = CURRENT_STATE.PLAYING;

    //private enum DIALOGUE_PHASE
    //{
    //    NOT_DISLPAYED = 0,
    //    DISPLAYING,
    //    DISPLAYED,
    //    HIDING
    //}
    //private DIALOGUE_PHASE m_CurrentDialoguePhase;

    void Awake()
    {
        if (Guidebook == null)
        {
            Debug.LogAssertion("No Guide Book");
        }
        if (CaseFiles == null)
        {
            Debug.LogAssertion("No Case Files");
        }
    }

	// Use this for initialization
	void Start () {
        //m_CurrentDialoguePhase = DIALOGUE_PHASE.NOT_DISLPAYED;
        m_NumberOfSuspiciousObjectsFound = 0;
	}

    #region Update

    // Update is called once per frame
    void Update()
    {
        switch (m_CurrentState)
        {
            case CURRENT_STATE.OPENING_CASE:
                {
                    // Set rect transform
                    RectTransform rectTransform = slotSelected.GetComponent<RectTransform>();

                    if (Input.GetMouseButtonDown(0))
                    {
                        rectTransform.anchorMin = Vector2.zero;
                        rectTransform.anchorMax = Vector2.one;
                        rectTransform.anchoredPosition = Vector2.zero;
                        m_CurrentState = CURRENT_STATE.OPENED_CASE;
                        break;
                    }
                    else
                    {
                        rectTransform.anchoredPosition += speedToCenter * Time.deltaTime;

                        rectTransform.anchorMin -= speedToFullyExpandSlot * Time.deltaTime;
                        if (rectTransform.anchorMin.x < 0.0f)
                        {
                            rectTransform.anchorMin = Vector2.zero;
                        }
                        rectTransform.anchorMax += speedToFullyExpandSlot * Time.deltaTime;
                        if (rectTransform.anchorMax.x > 1.0f)
                        {
                            rectTransform.anchorMax = Vector2.one;
                        }

                        timeToFullyExpandSlot -= Time.deltaTime;
                        if (timeToFullyExpandSlot <= 0.0f)
                        {
                            rectTransform.anchoredPosition = Vector2.zero;
                            m_CurrentState = CURRENT_STATE.OPENED_CASE;
                        }
                    }
                    break;
                }
            case CURRENT_STATE.OPENED_CASE:
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (GameControllerScript.Instance.m_currentCase.m_bFirstTime)
                        {
                            GameControllerScript.Instance.m_currentCase.m_bFirstTime = false;
                            SceneManager.LoadScene("Cutscene");
                        }
                        else
                            SceneManager.LoadScene("CaseScene");
                    }
                    break;
                }
        }
        //if (Input.GetMouseButtonDown(0))
        //{
        //    CastRay();
        //}
    }

    #endregion

    #region Functions

    void CastRay()
    {
        //switch (m_CurrentDialoguePhase)
        //{
        //    case DIALOGUE_PHASE.NOT_DISLPAYED:
        //        {
        //            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        //            switch(m_CurrentState)
        //            {
        //                case CURRENT__STATE.PLAYING:
        //                    {
        //                        if (hit)
        //                        {
                                    
        //                        }
        //                    }
        //                    break;
        //                case CURRENT__STATE.VICTIM_DONE_WRONG:
        //                    {
        //                        if (CaseFiles.activeSelf == true)
        //                        {
        //                            if (hit)
        //                            {
        //                                Debug.Log(hit.collider.gameObject.name);
        //                                // Check each interactable objects whether which object had been hits
        //                                foreach (Transform child in CaseFiles.transform.FindChild("Slots").transform)
        //                                {
        //                                    if (child.name == hit.collider.gameObject.name)
        //                                    {
        //                                        foreach (Transform item in child.transform)
        //                                        {
        //                                            if (item.GetComponent<InteractableScript>().wrong)
        //                                            {
        //                                                item.gameObject.SetActive(false);
        //                                                m_NumberOfWrongObjectsLeft--;
        //                                                break;
        //                                            }
        //                                        }
        //                                        break;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    break;
        //            }
                    
        //        }
        //        break;
        //    case DIALOGUE_PHASE.DISPLAYED:
        //        {
        //            m_CurrentDialoguePhase = DIALOGUE_PHASE.HIDING;
        //        }
        //        break;
        //}
    }

    public void LoadPrevention(GameControllerScript.DIFFICULTY difficulty)
    {
        //m_CurrentState = CURRENT__STATE.PREVENTION;
        //switch (difficulty)
        //{
        //    case GameControllerScript.DIFFICULTY.HARD:
                CaseFiles.GetComponent<CaseFilesScript>().CloseCaseFiles();
                Guidebook.GetComponent<GuideBookScript>().OpenGuideBook();
                Guidebook.transform.FindChild("Close").gameObject.SetActive(false);
        //        break;
        //    default:
        //        break;
        //}
    }

    public void LoadWhatTheVictimHadDoneWrong(GameControllerScript.DIFFICULTY difficulty)
    {
        //m_CurrentState = CURRENT__STATE.VICTIM_DONE_WRONG;
        //switch(difficulty)
        //{
        //    case GameControllerScript.DIFFICULTY.HARD:
                CaseFiles.GetComponent<CaseFilesScript>().OpenCaseFiles();
                CaseFiles.transform.FindChild("Background").gameObject.GetComponent<Collider2D>().enabled = false;
                CaseFiles.transform.FindChild("Close").gameObject.SetActive(false);
        //        break;
        //    default:
        //        break;
        //}
    }


    #region OnClick Functions

    public void ClickQuit()
    {
        if (GameControllerScript.Instance)
            GameControllerScript.Instance.ClearInfo();
    }

    public void ClickSlot(int slotNumber)
    {
        // Set cases
        slotSelected = GameObject.Find("Slot " + slotNumber.ToString()).transform;

        if (slotSelected.childCount > 0)
        {
            for (int i = 0; i < GameControllerScript.Instance.m_lCaseList.Count; i++)
            {
                if (GameControllerScript.Instance.m_lCaseList[i].name == slotSelected.GetChild(0).name)
                {
                    if (!GameControllerScript.Instance.m_lCaseInfoList[i].m_bCompleted)
                    {
                        GameControllerScript.Instance.m_currentCase = GameControllerScript.Instance.m_lCaseInfoList[i];
                        m_CurrentState = CURRENT_STATE.OPENING_CASE;

                        // Set rect transform
                        RectTransform rectTransform = slotSelected.GetComponent<RectTransform>();

                        speedToCenter = (Vector2.zero - rectTransform.anchoredPosition) / timeToFullyExpandSlot;
                        speedToFullyExpandSlot = (Vector2.one - rectTransform.anchorMax) / timeToFullyExpandSlot;
                    }
                }
            }
        }
    }

    #endregion

    #endregion
}
