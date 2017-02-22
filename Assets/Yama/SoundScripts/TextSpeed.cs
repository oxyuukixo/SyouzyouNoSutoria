using UnityEngine;
using System.Collections;

public class TextSpeed : MonoBehaviour {

    public static float m_speed;


    private static bool m_loading;         //ロードしたかの判定

    //初期化
    private void Awake()
    {
        //二重の初期化防止
        if (m_loading) return;
        string[][] fileData;
        fileData = TextSystems.ReadText("TextSpeed");
        for (int i = 0; i < fileData.Length; i++)
        {
            for (int j = 0; j < fileData[i].Length; j++)
            {
                if (fileData[i][j] == "") continue;
                m_speed = float.Parse(fileData[i][j]);
            }
        }
        m_loading = true;
        //シーン読み込み時に破棄しない
        DontDestroyOnLoad(this);
    }


    //ファイルの内容を変更
    public static void ChangeFile()
    {
        string[][] text;
        text = new string[1][];
        text[0] = new string[1];
        for (int i = 0; i < text[0].Length; i++)
        {
            text[0][i] = m_speed.ToString();
        }
        TextSystems.WriteText("Text", text);
    }
}
