using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class NewGameButton: MonoBehaviour {

    private Fade m_Fade;
    public GameObject NewGameObj;
    private bool NewGame = false;

    // Use this for initialization
<<<<<<< HEAD
    void Start()
    {
=======
    void Start () {
>>>>>>> origin/development
        m_Fade = NewGameObj.GetComponent<Fade>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_Fade.m_IsFadeIn & m_Fade.m_IsFadeFinish)
        {
<<<<<<< HEAD
            if (NewGame)
            {
                SceneManager.LoadScene("Conversation");
            }
=======
            if(NewGame)
            SceneManager.LoadScene("Conversation");
>>>>>>> origin/development
        }
    }

    public void ButtonPush()
    {
        m_Fade.FadeOut();
        NewGame = true;
<<<<<<< HEAD
=======
        Debug.Log("New");
>>>>>>> origin/development
    }
}
