﻿using UnityEngine;
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
            //Invoke("SceneChange", 2f);
            SceneManager.LoadScene("Game");
        }
    }

    //void SceneChange () {
    //    SceneManager.LoadScene("Game");
    //}
}
