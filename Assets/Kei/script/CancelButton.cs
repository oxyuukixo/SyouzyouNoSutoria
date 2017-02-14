using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CancelButton : MonoBehaviour {
<<<<<<< HEAD
=======

>>>>>>> origin/development
    private Fade m_Fade;
    public GameObject refObj;

    // Use this for initialization
<<<<<<< HEAD
    void Start()
    {
=======
    void Start () {
>>>>>>> origin/development
        m_Fade = refObj.GetComponent<Fade>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_Fade.m_IsFadeIn & m_Fade.m_IsFadeFinish)
        {
            SceneManager.LoadScene("Title");
        }
    }
    public void ButtonPush()
    {
        m_Fade.FadeOut();
    }
}
