using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public GameObject AllCasesSolved = null;
    public GameObject Guidebook = null;
    public GameObject CaseFiles = null;

    private Transform slotSelected = null;
    private float timeToFullyExpandSlot = 0.25f;
    private Vector2 speedToFullyExpandSlot = new Vector2();
    private Vector2 speedToCenter = new Vector2();

    private float fadingTime = 0.5f;

    public enum CURRENT_STATE
    {
        PLAYING = 0,
        OPENING_CASE,
        OPENED_CASE,
        ALL_CASES_SOLVED,
    }
    [HideInInspector]
    public CURRENT_STATE m_CurrentState = CURRENT_STATE.PLAYING;

    void Awake()
    {
        if (AllCasesSolved == null)
        {
            Debug.LogAssertion("No AllCasesSolved");
        }
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
        Text completed = AllCasesSolved.GetComponent<Text>();
        completed.CrossFadeAlpha(0.0f, 0.0f, false);
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
            case CURRENT_STATE.PLAYING:
                {
                    bool allCasesSolved = true;
                    for (int i = 0; i < GameControllerScript.Instance.m_lCaseInfoList.Count; i++)
                    {
                        if (GameControllerScript.Instance.m_lCaseInfoList[i].m_bActivated && !GameControllerScript.Instance.m_lCaseInfoList[i].m_bCompleted)
                        {
                            allCasesSolved = false;
                            break;
                        }
                    }
                    if (allCasesSolved)
                    {
                        m_CurrentState = CURRENT_STATE.ALL_CASES_SOLVED;
                        Text completed = AllCasesSolved.GetComponent<Text>();
                        completed.CrossFadeAlpha(1.0f, fadingTime, false);
                    }
                    break;
                }
            case CURRENT_STATE.ALL_CASES_SOLVED:
                {
                    if (fadingTime > 0.0f)
                    {
                        fadingTime -= Time.deltaTime;
                        if (Input.GetMouseButtonDown(0))
                        {
                            Text completed = AllCasesSolved.GetComponent<Text>();
                            completed.CrossFadeAlpha(1.0f, 0.0f, false);
                            fadingTime = 0.0f;
                        }
                    }
                    else
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            ClickQuit();
                            SceneManager.LoadScene("MainMenu");
                        }
                    }
                    break;
                }
        }
    }

    #endregion

    #region Functions

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
                if (GameControllerScript.Instance.m_lCaseList[i].name == slotSelected.GetChild(slotSelected.childCount - 1).name)
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
