using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Xml;
using System.IO;
using UnityEngine.SceneManagement;

public class MCQManager : MonoBehaviour
{
    public GameObject m_Canvas = null;

    public List<ChatScript> m_chatList = new List<ChatScript>();
    public List<GameObject> m_goChatList = new List<GameObject>();
    public List<GameObject> m_goMCQList = new List<GameObject>();

    private int m_currentChatIndex;
    private Case m_currentCaseInfo = null;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            // If currently in mcq
            if (m_currentChatIndex >= 0 && m_goMCQList[m_currentChatIndex] != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
                if (hit)
                {
                    for(int i = 0; i < 4; i++)
                    {
                        GameObject answer = m_goMCQList[m_currentChatIndex].transform.FindChild("Answers").FindChild("Answer" + (i + 1).ToString()).gameObject;
                        if (answer.name == hit.collider.gameObject.name)
                        {
                            if (!m_chatList[m_currentChatIndex].m_mcq.m_answers[i].m_bCorrectAnswer)
                                m_goMCQList[m_currentChatIndex].transform.FindChild("Answers").FindChild("Answer" + (i + 1).ToString()).GetComponent<Image>().color = Color.red;
                            else
                                NextChat();
                        }
                    }
                }
            }
            else
            {
                NextChat();
            }
        }
	
	}

    // Go next Chat
    private void NextChat()
    {
        if (m_currentChatIndex >= 0)
        {
            // if there is mcq
            if (m_goMCQList[m_currentChatIndex] != null)
            {
                m_goMCQList[m_currentChatIndex].SetActive(false);
            }
        }
        m_currentChatIndex++;
        if (CheckIfThereIsChat())
        {
            MoveUpChat();
        }
        // Complete the case if no more current chat
        else
        {
            CompleteCase();
        }
    }
    // Check If There Is Chat
    private bool CheckIfThereIsChat()
    {
        return m_currentChatIndex < m_chatList.Count;
    }

    // Move Up Chat
    private void MoveUpChat()
    {
        for(int i = 0; i < m_goChatList.Count; i++)
        {
            RectTransform newRectTransform = m_goChatList[i].GetComponent<RectTransform>();

            float height = newRectTransform.rect.y * 2.0f;
            // Top
            newRectTransform.offsetMax = new Vector2(newRectTransform.offsetMax.x, newRectTransform.offsetMax.y - height);
            // Bottom
            newRectTransform.offsetMin = new Vector2(newRectTransform.offsetMin.x, newRectTransform.offsetMin.y - height);
        }

        // if there is mcq
        if (m_goMCQList[m_currentChatIndex] != null)
        {
            m_goMCQList[m_currentChatIndex].SetActive(true);
        }
    }
    // Complete case
    private void CompleteCase()
    {
        m_currentCaseInfo.m_bCompleted = true;
        SceneManager.LoadScene("CaseScene");
    }

    // Set Scene
    public void SetMCQScene(string name)
    {
        m_currentChatIndex = -1;
        m_currentCaseInfo = GameControllerScript.Instance.m_currentCase;
        TextAsset textAsset = (TextAsset)Resources.Load("XML/MCQScene", typeof(TextAsset));
        XmlReader reader = new XmlTextReader(new StringReader(textAsset.text));
        while(reader.Read())
        {
            if (reader.IsStartElement("mcqScenes"))
           {
               int maxNumOfMCQScenes = int.Parse(reader.GetAttribute("numOfMCQScenes"));
               for (int i = 0; i < maxNumOfMCQScenes; i++)
               {
                   reader.Read();
                   if (reader.IsStartElement("mcqScene") && reader.GetAttribute("name") == name)
                   {

                       int maxNumOfChats = int.Parse(reader.GetAttribute("numOfChats"));
                       reader.Read();
                       for (int j = 0; j < maxNumOfChats; j++)
                       {
                           if (reader.IsStartElement("chat"))
                           {
                               ChatScript newChat = new ChatScript();
                               m_chatList.Add(newChat);

                               MCQScript newMCQ = null;

                               bool gotMCQ = bool.Parse(reader.GetAttribute("gotMCQ"));
                               bool isLeft = bool.Parse(reader.GetAttribute("isLeft"));
                               string text = reader.GetAttribute("text");

                               if (gotMCQ)
                               {
                                   reader.Read();
                                   if (reader.IsStartElement("mcq"))
                                   {
                                       newMCQ = new MCQScript();
                                       newMCQ.m_question = reader.GetAttribute("question");

                                       int maxNumOfAnswers = int.Parse(reader.GetAttribute("numOfAnswers"));
                                       for (int k = 0; k < maxNumOfAnswers; k++)
                                       {
                                           reader.Read();
                                           if (reader.IsStartElement("answer"))
                                           {
                                               AnswerScript newAnswer = new AnswerScript();
                                               newAnswer.m_sAnswer = reader.GetAttribute("text");
                                               newAnswer.m_bCorrectAnswer = bool.Parse(reader.GetAttribute("correct"));
                                               newMCQ.m_answers[k] = newAnswer;
                                           }
                                       }
                                       reader.ReadOuterXml();
                                   }
                                   reader.ReadOuterXml();
                               }
                               // Set Chat
                               newChat.SetChat(isLeft, text, newMCQ);
                               // Add Chat Game Object
                               GameObject newGoChat = null;
                               GameObject copyGo = null;
                               if (newChat.m_bIsLeft)
                               {
                                   copyGo = m_Canvas.transform.FindChild("ChatBox").FindChild("LeftChat").gameObject;
                                   newGoChat = Instantiate(copyGo);
                               }
                               else
                               {
                                   copyGo = m_Canvas.transform.FindChild("ChatBox").FindChild("RightChat").gameObject;
                                   newGoChat = Instantiate(copyGo);
                               }
                               // Set chat info
                               newChat.SetChatInfo(newGoChat.transform);
                               newGoChat.SetActive(true);
                               newGoChat.transform.SetParent(m_Canvas.transform.FindChild("ChatBox"));
                               // Set rect transform
                               RectTransform newRectTransform = newGoChat.GetComponent<RectTransform>();
                               RectTransform copyRectTransform = copyGo.GetComponent<RectTransform>();
                               // Scale
                               newRectTransform.localScale = copyRectTransform.localScale;
                               // Get height
                               float height = copyRectTransform.rect.y * 2.0f;
                               // Top
                               newRectTransform.offsetMax = new Vector2(copyRectTransform.offsetMax.x, copyRectTransform.offsetMax.y + height * (j + 1));
                               // Bottom
                               newRectTransform.offsetMin = new Vector2(copyRectTransform.offsetMin.x, copyRectTransform.offsetMin.y + height * (j + 1));

                               m_goChatList.Add(newGoChat);

                               // Add MCQ Game Object
                               if (gotMCQ)
                               {
                                   copyGo = m_Canvas.transform.FindChild("MCQs").FindChild("MCQ").gameObject;
                                   GameObject newGoMCQ = Instantiate(copyGo);
                                   // Set chat info
                                   newChat.SetMCQInfo(newGoMCQ.transform);
                                   newGoMCQ.transform.SetParent(m_Canvas.transform.FindChild("MCQs"));
                                   // Set rect transform
                                   newRectTransform = newGoMCQ.GetComponent<RectTransform>();
                                   copyRectTransform = copyGo.GetComponent<RectTransform>();
                                   // Scale
                                   newRectTransform.localScale = copyRectTransform.localScale;
                                   // Top
                                   newRectTransform.offsetMax = new Vector2(copyRectTransform.offsetMax.x, copyRectTransform.offsetMax.y);
                                   // Bottom
                                   newRectTransform.offsetMin = new Vector2(copyRectTransform.offsetMin.x, copyRectTransform.offsetMin.y);

                                   m_goMCQList.Add(newGoMCQ);
                               }
                               else
                               {
                                   m_goMCQList.Add(null);
                               }

                               reader.ReadToNextSibling("chat");
                           }
                       }
                       reader.Close();
                       return;
                   }
               }
           }
        }
    }
}
