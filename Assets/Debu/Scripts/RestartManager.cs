using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RestartManager : SingletonMonoBehaviour<RestartManager> {

    public float m_RestartTime = 30;

    private float m_CurrentTime = 0;

    void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this);
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.touchCount > 0 || Input.GetMouseButtonDown(0))
        {
            m_CurrentTime = 0;
        }

        if((m_CurrentTime += Time.deltaTime) > m_RestartTime)
        {
            if(SceneManager.GetActiveScene().name != "Title")
            {
                ConversationControll.m_TextPath = "Prologue";
                SceneManager.LoadScene("Title");
            }
        }

	}
}
