using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoundSetting : MonoBehaviour
{
    private const int valueRate = 10;

    public Slider m_BGMSlider;      //BGMのスライダー
    public Slider m_SESlider;       //SEのスライダー
    public Slider m_TestSlider;
    private float m_oldBGMValue;    //BGMの前の値
    private float m_oldSEValue;     //SEの前の値
    private float m_oldSpeed;

    // Use this for initialization
    void Start ()
    {
        m_BGMSlider.value = SoundController.m_BGMVolume * valueRate;
        m_SESlider.value = SoundController.m_SEVolume * valueRate;
        m_TestSlider.value = TextSpeed.m_speed;
        m_oldBGMValue = m_BGMSlider.value;
        m_oldSEValue = m_SESlider.value;
        m_oldSpeed = m_TestSlider.value;

    }
	

    //BGMの音量変更
    public void ChangeBGMVolume()
    {
        SoundController.m_BGMVolume = m_BGMSlider.value / valueRate;
    }

    //SEの音量変更
    public void ChangeSEVolume()
    {
        SoundController.m_SEVolume = m_SESlider.value / valueRate;
    }

    public void ChangeTextSpeed()
    {
        TextSpeed.m_speed = m_SESlider.value;
    }

    public void ChangeApply()
    {
        SoundController.ChangeFile();
        TextSpeed.ChangeFile();
    }

    public void ChangeCancel()
    {
        SoundController.m_BGMVolume = m_oldBGMValue;
        SoundController.m_SEVolume = m_oldSEValue;
        TextSpeed.m_speed = m_oldSpeed;
    }
}
