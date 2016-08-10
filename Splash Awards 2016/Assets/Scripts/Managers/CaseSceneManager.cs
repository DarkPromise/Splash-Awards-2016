using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CaseSceneManager : MonoBehaviour
{
    public GameObject m_Canvas = null;
    public int numOfCaseObjects = 0;
    public int numOfCompletedCaseObjects = 0;
    public List<GameObject> m_caseObjectsList = new List<GameObject>();
    private Case m_currentCaseInfo = null;

    enum STATE
    {
        PLAYING = 0,
        MCQ,
        COMPLETED,
        NUM_OF_STATES
    }

    private STATE m_currentState;

    private float fadingTime = 0.5f;

    // Use this for initialization
    void Start()
    {
        m_currentState = STATE.PLAYING;
        Text completed = m_Canvas.transform.FindChild("UI").FindChild("Completed").GetComponent<Text>();
        completed.CrossFadeAlpha(0.0f, 0.0f, false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_currentState)
        {
            case STATE.PLAYING:
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
                        if (hit)
                        {
                            // Check each interactable objects whether which object had been hits
                            foreach (GameObject caseObject in m_caseObjectsList)
                            {
                                if (caseObject.name == hit.collider.gameObject.name)
                                {
                                    foreach (CaseObject caseObjectInfo in GameControllerScript.Instance.m_currentCase.m_lCaseObjects)
                                    {
                                        if (caseObjectInfo.m_name == caseObject.name)
                                        {
                                            m_currentCaseInfo.m_currentCaseObject = caseObjectInfo;
                                            GameControllerScript.Instance.CurrentCaseObjectMoveToNextScene();
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    if (numOfCompletedCaseObjects == numOfCaseObjects)
                    {
                        if (m_currentCaseInfo.m_bCompleted)
                        {
                            m_currentState = STATE.COMPLETED;
                            Text completed = m_Canvas.transform.FindChild("UI").FindChild("Completed").GetComponent<Text>();
                            completed.CrossFadeAlpha(1.0f, fadingTime, false);
                        }
                        else
                            SceneManager.LoadScene("MCQScene");
                    }
                    break;
                }
            case STATE.COMPLETED:
                {
                    if (fadingTime > 0.0f)
                    {
                        fadingTime -= Time.deltaTime;
                        if (Input.GetMouseButtonDown(0))
                        {
                            Text completed = m_Canvas.transform.FindChild("UI").FindChild("Completed").GetComponent<Text>();
                            completed.CrossFadeAlpha(1.0f, 0.0f, false);
                            fadingTime = 0.0f;
                        }
                    }
                    else
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            SceneManager.LoadScene("GameTemplate");
                        }
                    }
                    break;
                }
        }

    }

    public void SetCaseScene(Case caseInfo)
    {
        m_currentCaseInfo = caseInfo;

        m_Canvas.GetComponent<Image>().sprite = m_currentCaseInfo.m_background;

        for (int i = 0; i < m_currentCaseInfo.m_lCaseObjects.Count; i++)
        {
            CaseObject currentCaseObject = m_currentCaseInfo.m_lCaseObjects[i];

            GameObject newCaseObject = new GameObject();
            newCaseObject.name = currentCaseObject.m_name;
            newCaseObject.transform.SetParent(m_Canvas.transform.FindChild("Scene"));
            // Set rect transform
            newCaseObject.AddComponent<RectTransform>();
            RectTransform rectTransform = newCaseObject.GetComponent<RectTransform>();
            rectTransform.localScale = Vector3.one;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);

            // Left/ Bottom
            rectTransform.offsetMin = new Vector2(currentCaseObject.m_leftOffset, currentCaseObject.m_bottomOffset);
            // Right/ Top
            rectTransform.offsetMax = new Vector2(currentCaseObject.m_rightOffset, currentCaseObject.m_topOffset);

            // Set Box Collider 2D
            newCaseObject.AddComponent<BoxCollider2D>();
            BoxCollider2D boxCollider2D = newCaseObject.GetComponent<BoxCollider2D>();
            boxCollider2D.size = rectTransform.rect.max - rectTransform.rect.min;

            numOfCaseObjects++;
            if (currentCaseObject.m_bCompleted)
            {
                newCaseObject.SetActive(false);
                numOfCompletedCaseObjects++;
            }

            m_caseObjectsList.Add(newCaseObject);
        }
    }
}
