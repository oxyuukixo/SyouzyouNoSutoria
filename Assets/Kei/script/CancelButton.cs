using UnityEngine;
using System.Collections;

public class CancelButton : MonoBehaviour {

    public void ButtonPush()
    {
        FadeManager.Instance.LoadLevel("Title", 1.0f);
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
