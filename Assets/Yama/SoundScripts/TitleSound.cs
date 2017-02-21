using UnityEngine;
using System.Collections;

public class TitleSound : MonoBehaviour {

    public AudioSource m_audioSource;

    private static bool m_loading;

	// Use this for initialization
	void Awak ()
    {
        //二重の初期化防止
        if (m_loading) return;
        m_loading = true;
        //シーン読み込み時に破棄しない
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (m_audioSource.isPlaying) return;
        m_audioSource.Play();
    }

    public void DestroyTitleSound()
    {
        Destroy(gameObject);
    }

}
