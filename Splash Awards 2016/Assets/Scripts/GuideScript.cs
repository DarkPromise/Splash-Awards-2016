using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GuideScript : MonoBehaviour {
    public bool m_JustStoreGuides  = false;
    public List<string> m_Guides = new List<string>();
    private int m_CurrentPage;
    [HideInInspector]
    public int m_numOfGuidesPerPage;

	// Use this for initialization
	void Start () {
        m_CurrentPage = 0;
	}
	
	// Update is called once per frame
	void Update () {

    }

    #region Functions

    public void Setup(Transform ChangedTo)
    {
        m_Guides = new List<string>(ChangedTo.GetComponent<GuideScript>().m_Guides);
        m_numOfGuidesPerPage = transform.FindChild("Guides").childCount;
        SetNextPage();
    }

    public void SetNextPage()
    {
        for (int i = 0; i < m_numOfGuidesPerPage; i++)
        {
            int index = i + m_CurrentPage * m_numOfGuidesPerPage;
            if (index < m_Guides.Count)
                transform.FindChild("Guides").FindChild("Guide" + (i + 1).ToString()).GetComponent<Text>().text = m_Guides[index];
            else
                transform.FindChild("Guides").FindChild("Guide" + (i + 1).ToString()).GetComponent<Text>().text = "";
        }
        m_CurrentPage++;
        transform.FindChild("Next Button").GetComponent<Button>().interactable = CheckIfThereIsNextPage();
        transform.FindChild("Previous Button").GetComponent<Button>().interactable = CheckIfThereIsPrevPage();
    }

    private void SetPrevPage()
    {
        for (int i = 0; i < m_numOfGuidesPerPage; i++)
        {
            transform.FindChild("Guides").FindChild("Guide" + (i + 1).ToString()).GetComponent<Text>().text = m_Guides[i + (m_CurrentPage - 2) * m_numOfGuidesPerPage];
        }
        m_CurrentPage--;
        transform.FindChild("Next Button").GetComponent<Button>().interactable = CheckIfThereIsNextPage();
        transform.FindChild("Previous Button").GetComponent<Button>().interactable = CheckIfThereIsPrevPage();
    }

    private bool CheckIfThereIsNextPage()
    {
        return m_CurrentPage * m_numOfGuidesPerPage < m_Guides.Count;
    }

    private bool CheckIfThereIsPrevPage()
    {
        return m_CurrentPage > 1;
    }

    #region OnClick Functions

    public void ClickNext()
    {
        // Change next page if there 
        if (CheckIfThereIsNextPage())
        {
            SetNextPage();
        }
    }

    public void ClickPrev()
    {
        // Change next page if there 
        if (CheckIfThereIsPrevPage())
        {
            SetPrevPage();
        }
    }

    #endregion

    #endregion
}
