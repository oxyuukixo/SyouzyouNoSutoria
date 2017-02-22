using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeSetting : MonoBehaviour {

    //オプション設定
    public enum OptionSet
    {
        bgm,        //BGM
        se,         //SE
        textSpeed,  //文字速度
    }

    public OptionSet m_setType;


    //設定変更時の音
    public void ChangeSettingSoundPlay()
    {
        AudioSource audio;
        audio = GetComponent<AudioSource>();
        switch (m_setType)
        {
            case OptionSet.bgm:
                if (audio.volume == SoundController.m_BGMVolume) return;
                break;
            case OptionSet.se:
                if (audio.volume == SoundController.m_SEVolume) return;
                audio.volume = SoundController.m_SEVolume;
                break;
            case OptionSet.textSpeed:
                return;
        }
        audio.Play();
    }
}
