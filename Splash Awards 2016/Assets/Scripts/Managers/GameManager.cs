using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public GameObject GuideBook = null;
    public GameObject CaseFiles = null;
    [Range(0,12)]
    public int m_NumberOfSuspiciousObjects = 0;
    public GameObject Dialogue = null;
    public GameObject InteractableObjects = null;

    private enum DIALOGUE_PHASE
    {
        NOT_DISLPAYED = 0,
        DISPLAYING,
        DISPLAYED,
        HIDING
    }
    private DIALOGUE_PHASE m_CurrentDialoguePhase;

    private int m_NumberOfSuspiciousObjectsFound;

    void Awake()
    {
        if (GuideBook == null)
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
        UpdateDialogue();
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
                    if (hit)
                    {
                        // Check each interactable objects whether which object had been hits
                        foreach (Transform child in InteractableObjects.transform)
                        {
                            if (child.name == hit.collider.gameObject.name)
                            {
                                if (child.GetComponent<Interactable>().suspicious)
                                {
                                    // Change this object parent to case file and its sprtie renderer component
                                    m_NumberOfSuspiciousObjectsFound++;
                                    child.transform.parent = CaseFiles.transform.FindChild("Slots").FindChild("Slot " + m_NumberOfSuspiciousObjectsFound.ToString());
                                    child.GetComponent<SpriteRenderer>().sortingLayerName = "UI";
                                    child.GetComponent<SpriteRenderer>().sortingOrder = 1;
                                    // Set the position of this object in the case file to its slot
                                    child.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                                    child.localScale = new Vector3(100.0f, 100.0f, 1.0f);
                                    //DisplayDialogue(true);
                                }
                            }
                        }
                    }
                    else
                    {
                        // Hide guidebook if click outside of guidebook
                        if (GuideBook.activeSelf == true)
                        {
                            CloseGuideBook();
                        }
                        // Hide case files if click outside of guidebook
                        if (CaseFiles.activeSelf == true)
                        {
                            CloseCaseFiles();
                        }
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

    public void CloseGuideBook()
    {
        GuideBook.SetActive(false);
    }

    public void CloseCaseFiles()
    {
        CaseFiles.SetActive(false);
    }

    #region OnClick Functions

    public void ClickGuideBookButton()
    {
        GuideBook.SetActive(true);
    }

    public void ClickCaseFilesButton()
    {
        CaseFiles.SetActive(true);
    }

    public void ClickQuit()
    {
        GameController.Instance.CLearSubThemes();
        SceneManager.LoadScene("MainMenu");
    }

    #endregion

    #endregion
}
