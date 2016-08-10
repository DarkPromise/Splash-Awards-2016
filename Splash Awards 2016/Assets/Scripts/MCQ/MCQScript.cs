using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MCQScript {
    public string m_question = null;
    public const int m_numOfAnswers = 4;
    public AnswerScript[] m_answers = new AnswerScript[m_numOfAnswers];

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void SetMCQInfo(Transform goMCQ)
    {
        // Set Question
        goMCQ.FindChild("Question").GetComponent<Text>().text = m_question;
        // Set Answers
        for (int i = 0; i < m_numOfAnswers; i++)
        {
            goMCQ.FindChild("Answers").FindChild("Answer" + (i + 1).ToString()).FindChild("Text").GetComponent<Text>().text = m_answers[i].m_sAnswer;
        }
    }
}
