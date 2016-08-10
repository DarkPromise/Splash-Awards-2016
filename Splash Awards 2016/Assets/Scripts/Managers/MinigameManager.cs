using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{
    public GameObject Canvas = null;
    public GameObject Clear = null;
    public GameObject AI = null;
    private CaseObject m_currentCaseObjectInfo = null;

    public List<GameObject> m_AIList = new List<GameObject>();
    public int m_spawnAmountLeft = 0;
    public float m_spawnRate = 0.0f;
    [HideInInspector]
    public float m_timeLeftToNextSpawn = 0.0f;
    
    enum STATE
    {
        PLAYING = 0,
        MCQ,
        CLEAR,
        NUM_OF_STATES
    }

    private STATE m_currentState;

    private float fadingTime = 0.5f;

	// Use this for initialization
	void Start () {
        m_currentState = STATE.PLAYING;
        Text clear = Clear.GetComponent<Text>();
        clear.CrossFadeAlpha(0.0f, 0.0f, false);
	}
	
	// Update is called once per frame
    void Update()
    {
        switch (m_currentState)
        {
            case STATE.PLAYING:
                {
                    CheckAndSpawn();
                    CheckCollisionWithAIList();
                    CheckIfAllAIKilled();
                    break;
                }
            case STATE.CLEAR:
                {
                    if (fadingTime > 0.0f)
                    {
                        fadingTime -= Time.deltaTime;
                        if (Input.GetMouseButtonDown(0))
                        {
                            Text clear = Clear.GetComponent<Text>();
                            clear.CrossFadeAlpha(1.0f, 0.0f, false);
                            fadingTime = 0.0f;
                        }
                    }
                    else
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            GameControllerScript.Instance.CurrentCaseObjectMoveToNextScene();
                        }
                    }
                    break;
                }
        }
    }

    private void CheckAndSpawn()
    {
        if (m_spawnAmountLeft != 0)
        {
            m_timeLeftToNextSpawn += Time.deltaTime;
            // Spawn when time reached
            if (m_timeLeftToNextSpawn >= m_spawnRate)
            {
                m_timeLeftToNextSpawn = 0.0f;
                m_spawnAmountLeft--;


                GameObject newAI = Instantiate(AI);
                newAI.SetActive(true);
                newAI.transform.SetParent(AI.transform.parent);
                newAI.name = AI.name + m_AIList.Count.ToString();

                RectTransform newAIRectTransform = newAI.GetComponent<RectTransform>();
                RectTransform AIRectTransform = AI.GetComponent<RectTransform>();
                newAIRectTransform.localScale = AIRectTransform.localScale;

                RectTransform canvasRectTransform = Canvas.GetComponent<RectTransform>();

                Vector2 randomOffset = new Vector2( Random.RandomRange(0, canvasRectTransform.sizeDelta.x - AIRectTransform.rect.width),
                                                    Random.RandomRange(0, canvasRectTransform.sizeDelta.y - AIRectTransform.rect.height));
                // Top
                newAIRectTransform.offsetMax = new Vector2(AIRectTransform.offsetMax.x + randomOffset.x, AIRectTransform.offsetMax.y + randomOffset.y);
                // Bottom
                newAIRectTransform.offsetMin = new Vector2(AIRectTransform.offsetMin.x + randomOffset.x, AIRectTransform.offsetMin.y + randomOffset.y);

                m_AIList.Add(newAI);
            }
        }

    }

    private void CheckCollisionWithAIList()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if (hit)
            {
                for (int i = 0; i < m_AIList.Count; i++)
                {
                    if (m_AIList[i].name == hit.collider.gameObject.name)
                    {
                        Destroy(m_AIList[i]);
                        m_AIList.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }

    private void CheckIfAllAIKilled()
    {
        if (m_spawnAmountLeft == 0 && m_AIList.Count == 0)
        {
            m_currentState = STATE.CLEAR;
            Text clear = Clear.GetComponent<Text>();
            clear.CrossFadeAlpha(1.0f, fadingTime, false);
        }
    }

    public void SetCurrentCaseObject(CaseObject caseObject)
    {
        m_timeLeftToNextSpawn = 0.0f;
        m_currentCaseObjectInfo = caseObject;
    }
}
