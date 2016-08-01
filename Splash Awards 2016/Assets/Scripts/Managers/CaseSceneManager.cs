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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetMouseButtonDown(0))
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
                                caseObjectInfo.m_bCompleted = true;
                            }
                        }
                        caseObject.SetActive(false);
                        numOfCompletedCaseObjects++;
                        break;
                    }
                }
            }
        }
        if(numOfCompletedCaseObjects == numOfCaseObjects)
        {
            GameControllerScript.Instance.m_currentCase.m_bCompleted = true;
            SceneManager.LoadScene("GameTemplate");
        }
	}

    public void SetCaseScene(Case caseInfo)
    {
        m_Canvas.GetComponent<Image>().sprite = caseInfo.m_background;

        for(int i = 0; i < caseInfo.m_lCaseObjects.Count; i++)
        {
            CaseObject currentCaseObject = caseInfo.m_lCaseObjects[i];

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
