using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

public class ConversationControll : MonoBehaviour {

    //テキストを表示する親となるオブジェクト
    public GameObject m_TextBox;

    //文字を表示するスピード
    public float m_ReadSpeed;

    //文字を表示する数(1バイト文字での計算)
    public int m_WeightNum;
    public int m_HeightNum;

    //2バイト文字を使うかどうか
    public bool m_Use2byte;

    //使用する文字のオプション
    public Font m_Font;
    public FontStyle m_FontStyle;
    public int m_FontSize = 25;
    public bool m_IsRichText = true;
    public Color m_Color = new Color(1,1,1,1);

    //読み込むテキストのパス(シーン移動する前に設定)
    [HideInInspector]
    public static string m_TextPath = "Test.txt";

    //テキストをすべて読み終わったか
    [HideInInspector]
    public bool m_IsFinish = false;

    //テキストボックスのリスト
    private List<GameObject> m_TextLIst = new List<GameObject>();

    //ストリームリーダー
    private StreamReader m_Sr;

    //ShiftJISにするエンコーダー
    Encoding m_EncodingShiftJIS;

    //読み込むテキストを格納する変数
    private string m_Text;

    //入力待ちをするかどうかのフラグ
    private bool m_IsWait = false;

    //フォントサイズに足すテキストボックスの幅
    private int m_BoxOffset = 3;

    //前の文字を表示してからの時間
    private float m_ReadCurrentTime;

    //現在の文字の位置
    private int m_CurrentTextNum = 0;

    // Use this for initialization
    void Start () {
        m_EncodingShiftJIS = Encoding.GetEncoding("Shift_JIS");

        //TextRead();

        m_Text = "aaaあふぁふ\nぁふぁsfgaｓｇｓ\n\n";

        Debug.Log(m_Text);

        for (int i = 0; i < m_Text.Length; i++)
        {
            Debug.Log(i + ":" + m_Text[i] + ":" + m_EncodingShiftJIS.GetByteCount(m_Text[i].ToString()));
        }
    }
	
	// Update is called once per frame
	void Update () {

        if(!m_IsWait)
        {
           if((m_ReadCurrentTime += Time.deltaTime) > m_ReadSpeed)
            {
                
            }
        }

        if(CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            CreateTextBox();

            m_TextLIst[m_TextLIst.Count - 1].GetComponent<Text>().text = "aaaaaaaaaaaaaaaaaaaa";

            m_IsWait = false;
        }

	}

    void TextRead()
    {
        //ストリームリーダーを作成
        m_Sr = new StreamReader(Application.dataPath + "/Text/" + m_TextPath, m_EncodingShiftJIS);

        //最後まで読み込む
        m_Text = m_Sr.ReadToEnd();
        m_Sr.Close();
    }

    void CreateTextBox()
    {
        GameObject NewTextBox = new GameObject();
        RectTransform RTransform = NewTextBox.AddComponent<RectTransform>();
        Text TextComp = NewTextBox.AddComponent<Text>();

        //基準となる座標を左上にする
        RTransform.position = new Vector3(0, -(m_FontSize + m_BoxOffset) * m_TextLIst.Count, 0);
        RTransform.sizeDelta = new Vector2(m_TextBox.GetComponent<RectTransform>().rect.width, m_FontSize + m_BoxOffset);
        RTransform.anchorMin = new Vector2(0, 1);
        RTransform.anchorMax = new Vector2(0, 1);
        RTransform.pivot = new Vector2(0, 1);

        //パラメーターを設定
        TextComp.font = m_Font;
        TextComp.fontStyle = m_FontStyle;
        TextComp.fontSize = m_FontSize;
        TextComp.supportRichText = m_IsRichText;
        TextComp.color = m_Color;

        NewTextBox.transform.SetParent(m_TextBox.transform,false);
        m_TextLIst.Add(NewTextBox);
    }

    void CheckWord()
    {
       switch(m_Text[m_CurrentTextNum])
        {
            case '\n':

                m_CurrentTextNum++;

                CheckWord();

                break;

            case '#':

                break;

            case '@':

                break;

            case '*':

                break;

            case '$':

                break;
        }
    }
}
