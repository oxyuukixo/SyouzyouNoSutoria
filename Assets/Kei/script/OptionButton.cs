using UnityEngine;
using System.Collections;

public class OptionButton : MonoBehaviour {

    public void ButtonPush()
    {
        FadeManager.Instance.LoadLevel("Option", 1.0f);
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
