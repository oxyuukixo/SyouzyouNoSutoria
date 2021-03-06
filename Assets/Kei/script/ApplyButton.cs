﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ApplyButton : MonoBehaviour {

    private Fade m_Fade;
    public GameObject OptionObj;
    private bool Option = false;

    // Use this for initialization
    void Start()
    {
        m_Fade = OptionObj.GetComponent<Fade>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_Fade.m_IsFadeIn & m_Fade.m_IsFadeFinish)
        {
            if (Option)
            {
                SceneManager.LoadScene("Title");
            }
        }
    }
    public void ButtonPush()
    {
        m_Fade.FadeOut();
        Option = true;
    }
}