using UnityEngine;
using System.Collections;

public class CaseFilesScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    }

    #region Functions

    public void CloseCaseFiles()
    {
        gameObject.SetActive(false);
    }

    #region OnClick Functions

    public void OpenCaseFiles()
    {
        gameObject.SetActive(true);
    }

    #endregion

    #endregion
}
