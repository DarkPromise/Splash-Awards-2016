using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChatScript
{
    public bool m_bIsLeft = true;
    public string m_chat = null;
    public MCQScript m_mcq = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetChatInfo(Transform goChat)
    {
        goChat.FindChild("MessageBox").FindChild("Text").GetComponent<Text>().text = m_chat;
    }

    public void SetMCQInfo(Transform goMCQ)
    {
        m_mcq.SetMCQInfo(goMCQ);
    }

    public void SetChat(bool isLeft, string chat, MCQScript mcq)
    {
        m_bIsLeft = isLeft;
        m_chat = chat;
        m_mcq = mcq;
    }


}
