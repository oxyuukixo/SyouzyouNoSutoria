using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class TouchStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            FadeManager.Instance.LoadLevel("Game", 1.0f);
        }
    }

    //void SceneChange()
    //{
    //    FadeManager.Instance.LoadLevel("Game", 1.0f);
    //}
}
