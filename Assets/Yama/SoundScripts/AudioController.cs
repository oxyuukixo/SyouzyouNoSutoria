using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {

    public SoundController.SoundType m_soundType;   //音の種類
    public SoundController.BGMType m_BGMType;       //BGMの種類
    public SoundController.SEType m_SEType;         //SEの種類
    public bool m_soundLoop;                        //サウンドがループするかの判定

    private AudioSource m_audioSource;              //音源

	//初期化
	void Start ()
    {
        m_audioSource = GetComponent<AudioSource>();
        switch(m_soundType)
        {
            case SoundController.SoundType.BGM:
                m_audioSource.volume = SoundController.m_BGMVolume;
                m_audioSource.clip = SoundController.m_BGM[(int)m_BGMType];
                break;
            case SoundController.SoundType.SE:
                m_audioSource.volume = SoundController.m_SEVolume;
                m_audioSource.clip = SoundController.m_SE[(int)m_SEType];
                break;
        }
        m_audioSource.loop = m_soundLoop;
    }

    //フレーム単位で更新
    void Update ()
    {
        ChangeVolume();
    }

    //ボリュームを変更
    void ChangeVolume()
    {
        switch (m_soundType)
        {
            case SoundController.SoundType.BGM:
                if (m_audioSource.volume != SoundController.m_BGMVolume)
                    m_audioSource.volume = SoundController.m_BGMVolume;
                break;
            case SoundController.SoundType.SE:
                if (m_audioSource.volume != SoundController.m_SEVolume)
                    m_audioSource.volume = SoundController.m_SEVolume;
                break;
        }

    }


}
