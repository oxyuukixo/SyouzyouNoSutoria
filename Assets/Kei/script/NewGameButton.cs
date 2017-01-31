using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class NewGameButton: MonoBehaviour {

    private Fade m_Fade;
    public GameObject NewGameObj;
    private bool NewGame = false;

    // Use this for initialization
    void Start()
    {
        m_Fade = NewGameObj.GetComponent<Fade>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_Fade.m_IsFadeIn & m_Fade.m_IsFadeFinish)
        {
            if (NewGame)
            {
                SceneManager.LoadScene("Conversation");
            }
        }
    }

    public void ButtonPush()
    {
        m_Fade.FadeOut();
        NewGame = true;
        Debug.Log("New");
    }
}
