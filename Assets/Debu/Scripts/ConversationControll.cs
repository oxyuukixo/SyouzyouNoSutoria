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

    //スクロールするスピード
    public float m_ScrollSpeed = 1;

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

    //名前を表示するためのテキスト
    public GameObject[] m_NameText;

    //画像を表示するためのオブジェクト
    public Image[] m_Graphic;

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

    //現在の行の文字数
    private int m_WeightCurrentNum = 0;

    //画像があるフォルダまでのパス
    private string m_GraphicPath;

    //スクロールするかどうかのフラグ
    private bool m_IsScroll = false;

    // Use this for initialization
    void Start () {

        m_EncodingShiftJIS = Encoding.GetEncoding("Shift_JIS");

        m_GraphicPath = "Conversation/";

        //TextRead();

        if (m_Use2byte)
        {
            m_WeightNum *= 2;
        }
        
        m_Text = "aaaあふぁaddddddddd@dd、、、。、、、、tttttttt、、dddふぇ*1 testKUN*あっふ$1 Leo/Leo_laugh$ぇあふぁ@sffaf#gaｓ\n\nｇ@ｓ@";

        CreateTextBox();
    }
	
	// Update is called once per frame
	void Update () {

        if(m_IsScroll)
        {
            ScrollText();
        }
        //入力待ちじゃなかったら
        else if(!m_IsWait && !m_IsFinish)
        {
           if((m_ReadCurrentTime += Time.deltaTime) > m_ReadSpeed)
            {
                //まだ全部読んでいなかったら
                if(m_CurrentTextNum < m_Text.Length)
                {
                    if(CheckWord())
                    {
                        if(m_WeightCurrentNum >= m_WeightNum)
                        {
                            CreateTextBox();
                            m_WeightCurrentNum = 0;
                            
                            if(m_TextLIst.Count > m_HeightNum)
                            {
                                m_IsScroll = true;
                            }

                        }
                        else
                        {
                            AddText();
                        }
                    }
                }
                else
                {
                    m_IsFinish = true;
                }

                m_ReadCurrentTime = 0;
            }      
        }

        //クリックされたら
        if(CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            if(m_IsWait)
            {
                m_IsWait = false;

                m_CurrentTextNum++;
            }

            //CreateTextBox();

            //m_TextLIst[m_TextLIst.Count - 1].GetComponent<Text>().text = "aaaaaaaaaaaaaaaaaaaa";
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

    //現在の行に現在の文字を追加
    void AddText()
    {
        m_TextLIst[m_TextLIst.Count - 1].GetComponent<Text>().text += m_Text[m_CurrentTextNum];

        m_WeightCurrentNum += m_EncodingShiftJIS.GetByteCount(m_Text[m_TextLIst.Count - 1].ToString());

        m_CurrentTextNum++;
    }

    void ScrollText()
    {
        for(int i = 0;i < m_TextLIst.Count;i++)
        {
            m_TextLIst[i].GetComponent<RectTransform>().localPosition += new Vector3(0, m_ScrollSpeed, 0);

            if (m_TextLIst[i].GetComponent<RectTransform>().localPosition.y >= -(m_FontSize + m_BoxOffset) * (i - 1))
            {
                m_TextLIst[i].GetComponent<RectTransform>().localPosition = new Vector2(0, -(m_FontSize + m_BoxOffset) * (i - 1));

                m_IsScroll = false;
            }
        }

        if(!m_IsScroll)
        {
            Destroy(m_TextLIst[0]);
            m_TextLIst.RemoveAt(0);
        }
    }

    //通常文字かのチェック
    bool CheckWord()
    {
       switch(m_Text[m_CurrentTextNum])
        {
            case '\n':

                m_CurrentTextNum++;

                break;

            case '#':

                CreateTextBox();

                m_WeightCurrentNum = 0;

                if(m_TextLIst.Count > m_HeightNum)
                {
                    m_IsScroll = true;
                }

                m_CurrentTextNum++;

                break;

            case '@':

                m_IsWait = true;

                break;

            case '*':

                string Name = null;

                int NameNum = int.Parse(m_Text[m_CurrentTextNum + 1].ToString()) - 1;

                //現在の文字から"*","名前の番号","スペース"を飛ばした数から最後まで調べる
                for(int i = m_CurrentTextNum + 3;i < m_Text.Length;i++)
                {
                    //*が見つかったら終了
                    if(m_Text[i] == '\n' || m_Text[i] == '*')
                    {
                        m_CurrentTextNum = i + 1;

                        break;
                    }

                    Name += m_Text[i];
                }

                m_NameText[NameNum].GetComponent<Text>().text = Name;

                break;

            case '$':

                string GraphicPath = null;

                int GraphicNum = int.Parse(m_Text[m_CurrentTextNum + 1].ToString()) - 1;

                //現在の文字から"*","名前の番号","スペース"を飛ばした数から最後まで調べる
                for (int i = m_CurrentTextNum + 3; i < m_Text.Length; i++)
                {
                    //*が見つかったら終了
                    if (m_Text[i] == '\n' || m_Text[i] == '$')
                    {
                        m_CurrentTextNum = i + 1;

                        break;
                    }

                    GraphicPath += m_Text[i];
                }

                m_Graphic[GraphicNum].sprite = Resources.Load<Sprite>(m_GraphicPath + GraphicPath);

                break;

            case '、':
            case '。':

                AddText();

                break;

            default:

                return true;
        }

        return false;
    }
}
