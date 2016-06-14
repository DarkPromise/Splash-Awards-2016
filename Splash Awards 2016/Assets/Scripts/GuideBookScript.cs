using UnityEngine;
using System.Collections;

public class GuideBookScript : MonoBehaviour {

    public GameObject Guidebook;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    #region Functions

    public void CloseGuideBook()
    {
        Guidebook.SetActive(false);
    }

    #region OnClick Functions

    public void ClickGuideBookButton()
    {
        Guidebook.SetActive(true);
    }

    #endregion

    #endregion
}
