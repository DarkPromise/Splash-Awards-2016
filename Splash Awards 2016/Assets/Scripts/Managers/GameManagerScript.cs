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
    public GameObject Dialogue = null;
    public GameObject InteractableObjects = null;

    public enum CURRENT__STATE
    {
        PLAYING = 0,
        VICTIM_DONE_WRONG,
        PREVENTION
    }
    [HideInInspector]
    public CURRENT__STATE m_CurrentState = CURRENT__STATE.PLAYING;

    private enum DIALOGUE_PHASE
    {
        NOT_DISLPAYED = 0,
        DISPLAYING,
        DISPLAYED,
        HIDING
    }
    private DIALOGUE_PHASE m_CurrentDialoguePhase;

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
        if (Dialogue == null)
        {
            Debug.LogAssertion("No Dialogue");
        }
        if (InteractableObjects == null)
        {
            Debug.LogAssertion("No Interactable Objects");
        }
    }

	// Use this for initialization
	void Start () {
        m_CurrentDialoguePhase = DIALOGUE_PHASE.NOT_DISLPAYED;
        m_NumberOfSuspiciousObjectsFound = 0;
	}

    #region Update

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CastRay();
        }
        //UpdateDialogue();
    }

    void UpdateDialogue()
    {
        switch (m_CurrentDialoguePhase)
        {
                // Displaying Dialogue
            case DIALOGUE_PHASE.DISPLAYING:
                {
                    foreach (Transform child in Dialogue.transform)
                    {
                        Color color = child.GetComponent<UnityEngine.UI.Image>().color;
                        color.a += Time.deltaTime;
                        if (color.a >= 1.0f)
                        {
                            color.a = 1.0f;
                            m_CurrentDialoguePhase = DIALOGUE_PHASE.DISPLAYED;
                        }
                        child.GetComponent<UnityEngine.UI.Image>().color = color;
                    }
                }
                break;
                // Hiding Dialogue
            case DIALOGUE_PHASE.HIDING:
                {
                    foreach (Transform child in Dialogue.transform)
                    {
                        Color color = child.GetComponent<UnityEngine.UI.Image>().color;
                        color.a -= Time.deltaTime;
                        if (color.a <= 0.0f)
                        {
                            color.a = 0.0f;
                            DisplayDialogue(false);
                        }
                        child.GetComponent<UnityEngine.UI.Image>().color = color;
                    }
                }
                break;
        }
    }

    #endregion

    #region Functions

    void CastRay()
    {
        switch (m_CurrentDialoguePhase)
        {
            case DIALOGUE_PHASE.NOT_DISLPAYED:
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
                    switch(m_CurrentState)
                    {
                        case CURRENT__STATE.PLAYING:
                            {
                                if (hit)
                                {
                                    // Check each interactable objects whether which object had been hits
                                    foreach (Transform child in InteractableObjects.transform)
                                    {
                                        if (child.name == hit.collider.gameObject.name)
                                        {
                                            if (child.GetComponent<InteractableScript>().suspicious)
                                            {
                                                // Change this object parent to case file and its sprtie renderer component
                                                m_NumberOfSuspiciousObjectsFound++;
                                                child.transform.parent = CaseFiles.transform.FindChild("Slots").FindChild("Slot " + m_NumberOfSuspiciousObjectsFound.ToString());
                                                child.GetComponent<SpriteRenderer>().sortingLayerName = "UI";
                                                child.GetComponent<SpriteRenderer>().sortingOrder = 1;
                                                child.GetComponent<Collider2D>().enabled = false;
                                                // Set the position of this object in the case file to its slot
                                                child.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                                                child.localScale = new Vector3(100.0f, 100.0f, 1.0f);

                                                //DisplayDialogue(true);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case CURRENT__STATE.VICTIM_DONE_WRONG:
                            {
                                if (CaseFiles.activeSelf == true)
                                {
                                    if (hit)
                                    {
                                        Debug.Log(hit.collider.gameObject.name);
                                        // Check each interactable objects whether which object had been hits
                                        foreach (Transform child in CaseFiles.transform.FindChild("Slots").transform)
                                        {
                                            if (child.name == hit.collider.gameObject.name)
                                            {
                                                foreach (Transform item in child.transform)
                                                {
                                                    if (item.GetComponent<InteractableScript>().wrong)
                                                    {
                                                        item.gameObject.SetActive(false);
                                                        m_NumberOfWrongObjectsLeft--;
                                                        break;
                                                    }
                                                }
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                    }
                    
                }
                break;
            case DIALOGUE_PHASE.DISPLAYED:
                {
                    m_CurrentDialoguePhase = DIALOGUE_PHASE.HIDING;
                }
                break;
        }
    }

    void DisplayDialogue(bool toShow)
    {
        if (toShow == true) 
        {
            Dialogue.SetActive(true);
            m_CurrentDialoguePhase = DIALOGUE_PHASE.DISPLAYING;
        }
        else
        {
            Dialogue.SetActive(false);
            m_CurrentDialoguePhase = DIALOGUE_PHASE.NOT_DISLPAYED;
        }
    }

    public void LoadPrevention(GameControllerScript.DIFFICULTY difficulty)
    {
        m_CurrentState = CURRENT__STATE.PREVENTION;
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
        m_CurrentState = CURRENT__STATE.VICTIM_DONE_WRONG;
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
        GameControllerScript.Instance.CLearSubThemes();
    }

    #endregion

    #endregion
}
