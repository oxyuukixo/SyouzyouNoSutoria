using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundController : MonoBehaviour {

    private const string m_BGMfileName = "BGM";       //BGMのテキストファイル名
    private const string m_SEfileName = "SE";         //SEのテキストファイル名
    private const string m_VolumefileName = "Sound";  //音量のテキストファイル名

    //音の種類
    public enum SoundType
    {
        BGM,            //BGM
        SE,             //SE
        number,         //要素数
    }

    //BGMの
    public enum BGMType
    {

    }

    //SEの
    public enum SEType
    {
        select,     //選択音
        decision,   //決定音
    }


    public const float m_changeVolumeRate = 0.05f;

    public static List<AudioClip> m_BGM;    //BGM
    public static List<AudioClip> m_SE;     //SE
    public static float m_BGMVolume;    //BGM音量
    public static float m_SEVolume;     //SE音量



    private static bool m_loading;         //ロードしたかの判定

    //初期化
    private void Awake()
    {
        //二重の初期化防止
        if (m_loading) return;
        LoadingBGM();
        LoadingSE();
        LoadingVolume();
        m_loading = true;
        //シーン読み込み時に破棄しない
        DontDestroyOnLoad(this);
    }

    //BGMの読み込み
    private void LoadingBGM()
    {
        string[][] fileData;
        fileData = TextSystems.ReadText(m_BGMfileName);
        m_BGM = new List<AudioClip>();
        for (int i = 0; i < fileData.Length; i++)
        {
            for (int j = 0; j < fileData[i].Length; j++)
            {
                if (fileData[i][j] == "") continue;
                m_BGM.Add(Resources.Load("Sound/BGM/" + fileData[i][j]) as AudioClip);
            }
        }
    }


    //SEの読み込み
    private void LoadingSE()
    {
        string[][] fileData;
        fileData = TextSystems.ReadText(m_SEfileName);
        m_SE = new List<AudioClip>();
        for (int i = 0; i < fileData.Length; i++)
        {
            for (int j = 0; j < fileData[i].Length; j++)
            {
                if (fileData[i][j] == "") continue;
                m_SE.Add(Resources.Load("Sound/SE/" + fileData[i][j]) as AudioClip);
            }
        }
    }


    //音量設定の読み込み
    private void LoadingVolume()
    {
        string[][] fileData;
        fileData = TextSystems.ReadText(m_VolumefileName);
        m_BGMVolume = float.Parse(fileData[0][(int)SoundType.BGM]);
        m_SEVolume = float.Parse(fileData[0][(int)SoundType.SE]);

    }

    //ファイルの内容を変更
    public static void ChangeFile()
    {
        string[][] text;
        text = new string[1][];
        text[0] = new string[(int)SoundType.number];
        for (int i = 0; i < text[0].Length; i++)
        {
            switch (i)
            {
                case (int)SoundType.BGM:
                    text[0][i] = m_BGMVolume.ToString();
                    break;
                case (int)SoundType.SE:
                    text[0][i] = m_SEVolume.ToString();
                    break;
            }
        }
        TextSystems.WriteText(m_VolumefileName, text);

    }

    //オーディオクリップの設定変更(BGM)
    public static AudioSource ChangeBGM(AudioSource audio, BGMType BGM)
    {
        AudioSource changeAudio;
        changeAudio = audio;
        changeAudio.volume = m_BGMVolume;
        changeAudio.clip = m_BGM[(int)BGM];
        return changeAudio;
    }

    //オーディオクリップの設定変更(SE)
    public static AudioSource ChangeSE(AudioSource audio, SEType SE)
    {
        AudioSource changeAudio;
        changeAudio = audio;
        changeAudio.volume = m_SEVolume;
        changeAudio.clip = m_SE[(int)SE];
        return changeAudio;
    }


}
