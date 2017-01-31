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

    //フェードに使用するオブジェクト
    public Image m_FadeObject;

    //場所を表示するためのテキスト
    public GameObject m_PlaceObject;

    public GameObject m_LeftTextUI;
    public GameObject m_RightTextUI;

    //名前を表示するためのテキスト
    public Text m_LeftNameText;
    public Text m_RightNameText;

    //画像を表示するためのオブジェクト
    public Image m_GraphicLeft;
    public Image m_GraphicRight;

    //背景画像
    public Image m_BackGround;

    public GameObject m_Canvas;

    //選択肢表示のオブジェクト
    public GameObject m_ChoiceParent;
    public GameObject m_ChoiceObject;

    //文字を表示するスピード
    public float m_ReadSpeed;

    //スクロールするスピード
    public float m_ScrollSpeed = 1;

    //現在地を表示する時間
    public float m_PlaceTime;

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

    //フェードする速さ
    public float m_FadeSpeed;

    //キャラクターが変わるときの速さ
    public float m_ChangeSpeed;

    //読み込むテキストのパス(シーン移動する前に設定)
    public static string m_TextPath = "Conversation_1220";

    //テキストをスキップするか
    [HideInInspector]
    public bool m_Skip = false;



    //テキストの状態
    enum ConversationState
    {
        Read,       //テキスト表示
        Wait,       //入力待ち
        Scroll,     //スクロール
        Place,      //地名表示
        CharaChange,//キャラクター変更
        Choice,     //選択肢表示
        Fade,       //暗転中
        Finish      //テキスト読み込み終了    
    }

    //テキストの状態
    private ConversationState m_State;

    //テキストボックスのリスト
    private List<GameObject> m_TextLIst = new List<GameObject>();

    //ストリームリーダー
    private StreamReader m_Sr;

    //ShiftJISにするエンコーダー
    Encoding m_EncodingShiftJIS;

    //読み込むテキストを格納する変数
    private string m_Text;

    //前の文字を表示してからの時間
    private float m_ReadCurrentTime;

    //現在の文字の位置
    private int m_CurrentTextNum = 0;

    //現在の行の文字数
    private int m_WeightCurrentNum = 0;

    //画像があるフォルダまでのパス
    private string m_ConversationPath;

    //何行スクロールするか
    private int m_ScrollNum = 0;

    //テキストをすべてクリアするかのフラグ(すべてスクロールさせる)
    private bool m_IsListClear = false;

    //フェードオブジェクトのコンポーネント
    private Fade m_FadeObjectComponent;

    //フェード後に移行するステート
    private ConversationState m_FadeAfterState;

    //地名表示のときの状態
    private int m_PlaceViewState;

    //地名表示の現在時間
    private float m_PlaceCurrentTime;

    //選択肢を管理するリスト
    private List<GameObject> m_ChoicesList = new List<GameObject>();

    //変更する画像パス
    private string m_ChangeGraphcPath;

    //変更する画像の位置(1 左　2 右)
    private int m_ChangeGraphicNum;

    //キャラチェンジ時のステート
    private int m_ChangeGrahicState;

    //=============================================================================
    //
    // Purpose : スタート関数．
    //           Update関数の前に1度だけ呼び出される
    //
    // Return : なし．
    //
    //=============================================================================
    void Start () {

        //場所表示用のオブジェクトの非アクティブにする
        m_PlaceObject.SetActive(false);

        m_FadeObjectComponent = m_FadeObject.GetComponent<Fade>();

        //Shift_JISのエンコーダーを取得
        m_EncodingShiftJIS = Encoding.GetEncoding("Shift_JIS");

        //会話シーンよう画像のパス
        m_ConversationPath = "Conversation/";

        //現在設定されているテキストを読み込む
        TextRead();

        //2byte文字使用だったら文字列カウントの幅を倍にする
        if (m_Use2byte)
        {
            m_WeightNum *= 2;
        }
        
        //表示用のテキストボックスを作成
        CreateTextBox();
    }

    //=============================================================================
    //
    // Purpose : アップデート関数
    //
    // Return : なし．
    //
    //=============================================================================
    void Update()
    {
        switch (m_State)
        {
            //テキストを一文づつ確認して表示
            case ConversationState.Read:

                Read();

                break;

            //入力待ち
            case ConversationState.Wait:

                Wait();

                break;

            //スクロール
            case ConversationState.Scroll:

                //スクロールする    
                Scroll();

                break;

            //地名の表示
            case ConversationState.Place:

                Place();

                break;

            case ConversationState.CharaChange:

                CharaChange();

                break;

            //選択肢の表示
            case ConversationState.Choice:

                Choice();

                break;

            //フェード
            case ConversationState.Fade:

                if(m_FadeObjectComponent.m_IsFadeFinish)
                {
                    m_State = m_FadeAfterState;
                }

                break;

            //テキスト読み込み終了
            case ConversationState.Finish:



                break;

        }
    }

    //=============================================================================
    //
    // Purpose : テキストを表示用の変数に読み込む．
    //
    // Return : なし．
    //
    //=============================================================================
    void TextRead()
    {
        //ストリームリーダーを作成
        m_Sr = new StreamReader(Application.dataPath + "/Resources/" + m_ConversationPath + "Text/" + m_TextPath + ".txt", m_EncodingShiftJIS);

        //最後まで読み込む
        m_Text = m_Sr.ReadToEnd();
        m_Sr.Close();
    }

    //=============================================================================
    //
    // Purpose : 新しい行の作成．
    //           最大の行数を超えたらスクロールするフラグを立てる
    //
    // Return : なし．
    //
    //=============================================================================
    void CreateTextBox()
    {
        //新しいテキストボックスの作成
        GameObject NewTextBox = new GameObject();

        //必要なコンポーネントの追加
        RectTransform RTransform = NewTextBox.AddComponent<RectTransform>();
        Text TextComp = NewTextBox.AddComponent<Text>();

        //パラメーターを設定
        TextComp.font = m_Font;
        TextComp.fontStyle = m_FontStyle;
        TextComp.fontSize = m_FontSize;
        TextComp.supportRichText = m_IsRichText;
        TextComp.color = m_Color;

        //基準となる座標を左上にする
        RTransform.position = new Vector3(0, -(TextComp.preferredHeight) * m_TextLIst.Count, 0);
        RTransform.sizeDelta = new Vector2(m_TextBox.GetComponent<RectTransform>().rect.width,TextComp.preferredHeight);
        RTransform.anchorMin = new Vector2(0, 1);
        RTransform.anchorMax = new Vector2(0, 1);
        RTransform.pivot = new Vector2(0, 1);

        //マスクの子にする
        NewTextBox.transform.SetParent(m_TextBox.transform,false);

        //テキストボックスを管理するリストに追加
        m_TextLIst.Add(NewTextBox);

        m_WeightCurrentNum = 0;

        //最大の行数の数を超えていたらスクロールする。
        if (m_TextLIst.Count > m_HeightNum)
        {
            m_ScrollNum++;
            m_State = ConversationState.Scroll;
        }
    }

    //=============================================================================
    //
    // Purpose : 現在表示してあるテキストをすべてクリアして行を1番上にする
    //           (この関数で行わず、必要なフラグをたてるだけ)
    //
    // Return : なし．
    //
    //=============================================================================
    void ListClear()
    {
        //すべての行をスクロールする
        m_ScrollNum = m_TextLIst.Count;

        //リストをクリアするフラグを立てる
        m_IsListClear = true;

        //スクロールさせる
        m_State = ConversationState.Scroll;
    }

    //=============================================================================
    //
    // Purpose : 現在の行に表示する文字を追加．
    //
    // Return : なし．
    //
    //=============================================================================
    void AddText()
    {
        m_TextLIst[m_TextLIst.Count - 1].GetComponent<Text>().text += m_Text[m_CurrentTextNum];

        char a = m_Text[m_CurrentTextNum];

        m_WeightCurrentNum += m_EncodingShiftJIS.GetByteCount(m_Text[m_TextLIst.Count - 1].ToString());

        m_CurrentTextNum++;
    }

    //=============================================================================
    //
    // Purpose : テキストの表示．
    //
    // Return : なし．
    //
    //=============================================================================
    void Read()
    {
        //次の文字を表示できる状態だったら
        if (ReadCheck())
        {
            //まだ全部読んでいなかったら
            if (m_CurrentTextNum < m_Text.Length)
            {
                //特殊文字じゃなかったら
                if (CheckWord())
                {
                    //現在の行が一杯だったら改行する
                    if (m_WeightCurrentNum >= m_WeightNum)
                    {
                        CreateTextBox();
                    }
                    else
                    {
                        //現在の行に文字を追加
                        AddText();

                        //文字を表示する経過時間のリセット
                        m_ReadCurrentTime = 0;
                    }
                }
            }
            else
            {
                //全部表示されていたら全部表示された状態にする
                m_State = ConversationState.Finish;
            }
        }
    }

    //=============================================================================
    //
    // Purpose : 入力待ち時の処理．
    //
    // Return : なし．
    //
    //=============================================================================
    void Wait()
    {
        //クリックまたはタッチされたら
        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            //入力待ち状態だったら
            if (m_State == ConversationState.Wait)
            {
                //入力待ちを解除する
                m_State = ConversationState.Read;
            }
        }
    }

    //=============================================================================
    //
    // Purpose : テキストのスクロール．
    //
    // Return : なし．
    //
    //=============================================================================
    void Scroll()
    {
        //スクロールする行があったら
        if (m_ScrollNum > 0)
        {
            bool IsScrollFinish = false;    //テキストのスクロールが終わっているかのフラグ

            //現在ある行の数だけ繰り返す
            for (int i = 0; i < m_TextLIst.Count; i++)
            {
                m_TextLIst[i].GetComponent<RectTransform>().localPosition += new Vector3(0, m_ScrollSpeed, 0);

                if(m_Skip)
                {
                    m_TextLIst[i].GetComponent<RectTransform>().localPosition = new Vector2(0, -(m_TextLIst[i].GetComponent<Text>().preferredHeight) * (i - m_ScrollNum));

                    IsScrollFinish = true;
                }
                else
                {
                    if (m_TextLIst[i].GetComponent<RectTransform>().localPosition.y >= -(m_TextLIst[i].GetComponent<Text>().preferredHeight) * (i - m_ScrollNum))
                    {
                        m_TextLIst[i].GetComponent<RectTransform>().localPosition = new Vector2(0, -(m_TextLIst[i].GetComponent<Text>().preferredHeight) * (i - m_ScrollNum));

                        IsScrollFinish = true;
                    }
                }             
            }

            //スクロールが完了していたら
            if (IsScrollFinish)
            {
                //リストをクリアするフラグが立っていたら
                if (m_IsListClear)
                {
                    //現在ある行をすべて
                    for (int i = 0; i < m_TextLIst.Count; i++)
                    {
                        //テキストボックスの削除
                        Destroy(m_TextLIst[i]);
                    }

                    //リストのクリア
                    m_TextLIst.Clear();

                    //クリアするフラグをfalseにする
                    m_IsListClear = false;

                    //新しいテキストボックスの作成
                    CreateTextBox();
                }
                else
                {
                    //上にスクロールして見えなくなったテキストボックスの削除
                    Destroy(m_TextLIst[0]);
                    m_TextLIst.RemoveAt(0);
                }

                m_ScrollNum = 0;

                //今の行の文字数カウントを0にする。
                m_WeightCurrentNum = 0;
            }

        }
        else
        {
            m_State = ConversationState.Read;
        }

    }

    //=============================================================================
    //
    // Purpose : 場所表示
    //
    // Return : なし
    //
    //=============================================================================
    void Place()
    {
        switch(m_PlaceViewState)
        {
            //フェードアウト開始
            case 0:

                FadeStart(false,ConversationState.Place);

                m_PlaceViewState = 1;

                break;

            //地名を表示してフェードイン
            case 1:

                m_PlaceObject.SetActive(true);

                FadeStart(true, ConversationState.Place);

                m_PlaceViewState = 2;
                m_PlaceCurrentTime = 0;

                break;

            //指定時間経過後フェードアウト
            case 2:

                if((m_PlaceCurrentTime += Time.deltaTime) > m_PlaceTime)
                {
                    FadeStart(false, ConversationState.Place);

                    m_PlaceViewState = 3;
                }

                break;

            //地名を非表示にしてフェードイン
            case 3:

                m_PlaceObject.SetActive(false);

                FadeStart(true);

                break;
        }
    }

    //=============================================================================
    //
    // Purpose : キャラクターの変更関数
    //
    // Return : なし
    //
    //=============================================================================
    void CharaChange()
    {
       switch(m_ChangeGrahicState)
        {
            case 0:

                if (m_ChangeGraphicNum == 1)
                {
                    m_GraphicLeft.rectTransform.localPosition -= new Vector3(m_ChangeSpeed, 0, 0);

                    if (m_GraphicLeft.rectTransform.localPosition.x < -m_GraphicLeft.rectTransform.sizeDelta.x)
                    {
                        m_GraphicLeft.sprite = Resources.Load<Sprite>(m_ChangeGraphcPath);

                        m_ChangeGrahicState = 1;
                    }
                }
                else
                {
                    m_GraphicRight.rectTransform.localPosition += new Vector3(m_ChangeSpeed, 0, 0);

                    if (m_GraphicRight.rectTransform.localPosition.x > Screen.width)
                    {
                        m_GraphicRight.sprite = Resources.Load<Sprite>(m_ChangeGraphcPath);

                        m_ChangeGrahicState = 1;
                    }
                }

                break;

            case 1:

                if (m_ChangeGraphicNum == 1)
                {
                    m_GraphicLeft.rectTransform.localPosition += new Vector3(m_ChangeSpeed, 0, 0);

                    if (m_GraphicLeft.rectTransform.localPosition.x > 0)
                    {
                        m_GraphicLeft.rectTransform.localPosition = new Vector3(0, m_GraphicLeft.rectTransform.localPosition.y, m_GraphicLeft.rectTransform.localPosition.z);

                        m_State = ConversationState.Read;
                    }
                }
                else
                {
                    m_GraphicRight.rectTransform.localPosition -= new Vector3(m_ChangeSpeed, 0, 0);

                    if (m_GraphicRight.rectTransform.localPosition.x < Screen.width - m_GraphicRight.rectTransform.sizeDelta.x)
                    {
                        m_GraphicRight.rectTransform.localPosition = new Vector3(Screen.width - m_GraphicRight.rectTransform.sizeDelta.x, m_GraphicRight.rectTransform.localPosition.y, m_GraphicRight.rectTransform.localPosition.z);

                        m_State = ConversationState.Read;
                    }
                }

                break;
        }

    }

    //=============================================================================
    //
    // Purpose : 選択肢表示時の管理関数
    //
    // Return : なし
    //
    //=============================================================================
    void Choice()
    {
        for (int i = 0; i < m_ChoicesList.Count; i++)
        {
            if (m_ChoicesList[i].GetComponent<ChoiceButton>().m_IsPush)
            {
                m_TextPath = m_ChoicesList[i].GetComponent<ChoiceButton>().m_ReadText;

                TextRead();

                ListClear();

                m_ChoiceParent.SetActive(false);

                for (int j = 0; j < m_ChoicesList.Count; j++)
                {
                    Destroy(m_ChoicesList[j]);
                }

                m_ChoicesList.Clear();

                break;
            }
        }
    }

    //=============================================================================
    //
    // Purpose : フェード開始関数
    //
    // Return : なし
    //
    //=============================================================================
    void FadeStart(bool FadeIn,ConversationState AfterState = ConversationState.Read)
    {
        //フェード状態にする
        m_State = ConversationState.Fade;

        if (FadeIn)
        {
            m_FadeObjectComponent.FadeIn();
        }
        else
        {
            m_FadeObjectComponent.FadeOut();
        }

        //フェード後の状態のセット
        m_FadeAfterState = AfterState;
    }

    //=============================================================================
    //
    // Purpose : 次の文字を表示するかのチェック
    //
    // Return : 表示できるならtrue,できないならfalseを返す．
    //
    //=============================================================================
    bool ReadCheck()
    {
        if( (m_ReadCurrentTime += Time.deltaTime) > m_ReadSpeed || m_Skip)
        {
            return true;
        }

        return false;
    }

    //=============================================================================
    //
    // Purpose : 文字列の取得関数
    //          
    // Return : ""で囲まれている文字を取得して返す。
    //          "で始まらなかったり、"が来る前に特殊文字がきたらnullを返す。
    //
    //=============================================================================
    string PickString()
    {
        //次の文字にする
        m_CurrentTextNum++;

        //"から始まっていたら
        if (m_Text[m_CurrentTextNum] == '\"')
        {
            //読みだした文字列を格納する変数
            string Text = null;

            for (int i = m_CurrentTextNum + 1; i < m_Text.Length && m_Text[i] != '#'; i++)
            {
                //文字列終わりの記号が見つかったら
                if(m_Text[i] == '\"')
                {
                    //カウントを見つかった次のカウントにする。
                    m_CurrentTextNum = i + 1;

                    return Text;
                }

                Text += m_Text[i];
            }
        }

        //規則にあった文字列が見つからないのでnullを返す。
        return null;
    }

    //=============================================================================
    //
    // Purpose : 文字列の分割関数
    //          
    // Return : |で文字列を分割しその配列を返す。
    //
    //=============================================================================
    List<string> SplitString(string Text,char SplitKey)
    {
        List<string> SplitText = new List<string>();

        string OnceText = null;

        for(int i = 0;i < Text.Length;i++)
        {
            if(Text[i] == SplitKey)
            {
                SplitText.Add(OnceText);
                OnceText = null;
            }
            else
            {
                OnceText += Text[i];
            }
        }

        if(Text[Text.Length - 1] != SplitKey)
        {
            SplitText.Add(OnceText);
        }

        return SplitText;
    }

    //=============================================================================
    //
    // Purpose : 通常文字化のチェック
    //           通常文字ではなかったらそれに見合った処理をする
    //
    // Return : 通常文字ならtrue,違ったらfalseを返す。．
    //
    //=============================================================================
    bool CheckWord()
    {
        //操作用の文字列を格納する配列
        string OperationText;

        //分割した操作用の文字列を格納するリスト
        List<string> SplitText;

        switch (m_Text[m_CurrentTextNum])
        {
            //改行文字は無視する
            case '\r':
            case '\n':

                m_CurrentTextNum++;

                break;

            //以下の文字の場合は強制的に今の行に追加する
            case '、':
            case '。':
            case '!':
            case '?':
            case '！':
            case '？':

                AddText();

                break;
            
            //特殊文字
            case '#':

                //次に進める
                m_CurrentTextNum++;

                //文字を確認
                switch (m_Text[m_CurrentTextNum])
                {
                    case '#':

                        AddText();

                        break;

                    //改行
                    case '\\':

                        if (m_Text[m_CurrentTextNum + 1] == 'n')
                        {
                            //新しい行の作成
                            CreateTextBox();

                            //テキストのカウントを\\とn分進める
                            m_CurrentTextNum += 2;
                        }

                        break;

                    //行のクリア
                    case 'u':

                        //リストクリアのフラグを立てる
                        ListClear();

                        //テキストのカウントを進める
                        m_CurrentTextNum++;

                        break;

                    //入力待ち
                    case 'w':

                        if (!m_Skip)
                        {
                            m_State = ConversationState.Wait;
                        }

                        //テキストのカウントを進める
                        m_CurrentTextNum++;

                        break;

                    //地名の表示
                    case 'p':

                        OperationText = PickString();

                        //文字列が返ってきたら
                        if (OperationText != null)
                        {
                            SplitText = SplitString(OperationText,'|');

                            if(SplitText.Count == 2)
                            {
                                m_PlaceObject.transform.FindChild("NameImage").GetComponent<Image>().sprite = Resources.Load<Sprite>(m_ConversationPath + SplitText[0]);
                                m_PlaceObject.transform.FindChild("BackGround").GetComponent<Image>().sprite = Resources.Load<Sprite>(m_ConversationPath + SplitText[1]);

                                m_PlaceViewState = 0;
                                m_State = ConversationState.Place;
                            }
                        }

                        break;

                    //キャラの名前変更
                    case 'n':

                        OperationText = PickString();

                        //文字列が返ってきたら
                        if (OperationText != null)
                        {
                            SplitText = SplitString(OperationText, '|');

                            switch (SplitText[0])
                            {
                                case "1":

                                    m_LeftTextUI.SetActive(true);
                                    m_RightTextUI.SetActive(false);

                                    m_LeftNameText.text = SplitText[1];

                                    break;

                                case "2":

                                    m_RightTextUI.SetActive(true);
                                    m_LeftTextUI.SetActive(false);

                                    m_RightNameText.text = SplitText[1];

                                    break;

                                default:

                                    //エラーログの表示
                                    Debug.Log("存在しない番号です。/nテキストを確認してください。");

                                    break;
                            }
                        }

                        break;

                    //表示するキャラの画像変更
                    case 'c':

                        OperationText = PickString();

                        //文字列が返ってきたら
                        if (OperationText != null)
                        {
                            SplitText = SplitString(OperationText, '|');

                            if (SplitText.Count == 2)
                            {
                                m_ChangeGrahicState = 0;

                                switch (SplitText[0])
                                {
                                    case "1":

                                        m_ChangeGraphicNum = 1;

                                        m_ChangeGraphcPath = m_ConversationPath + SplitText[1];

                                        m_State = ConversationState.CharaChange;

                                        break;

                                    case "2":

                                        m_ChangeGraphicNum = 2;

                                        m_ChangeGraphcPath = m_ConversationPath + SplitText[1];

                                        m_State = ConversationState.CharaChange;

                                        break;

                                    default:

                                        //エラーログの表示
                                        Debug.Log("存在しない番号です。/nテキストを確認してください。");

                                        break;
                                }
                            }
                        }

                        break;

                    //キャラの表情変更
                    case 'e':

                        OperationText = PickString();

                        //文字列が返ってきたら
                        if (OperationText != null)
                        {
                            SplitText = SplitString(OperationText, '|');

                            if(SplitText.Count == 2)
                            {
                                switch (SplitText[0])
                                {
                                    case "1":

                                        m_GraphicLeft.sprite = Resources.Load<Sprite>(m_ConversationPath + SplitText[1]);

                                        break;

                                    case "2":

                                        m_GraphicRight.sprite = Resources.Load<Sprite>(m_ConversationPath + SplitText[1]);

                                        break;

                                    default:

                                        //エラーログの表示
                                        Debug.Log("存在しない番号です。/nテキストを確認してください。");

                                        break;
                                }
                            }
                        }

                        break;
                    
                    //背景変更
                    case 'b':

                        OperationText = PickString();

                        //文字列が返ってきたら
                        if (OperationText != null)
                        {
                            m_BackGround.sprite = Resources.Load<Sprite>(m_ConversationPath + OperationText);
                        }

                        break;

                    //フェード
                    case 'f':

                        OperationText = PickString();

                        //文字列が返ってきたら
                        if (OperationText != null)
                        {
                            switch(OperationText)
                            {
                                //フェードイン
                                case "in":

                                    FadeStart(true);

                                    break;

                                //フェードアウト
                                case "out":

                                    FadeStart(false);

                                    break;

                                //テキストがあっていない場合
                                default:

                                    //エラーログの表示
                                    Debug.Log("フェードの操作が間違っています。/nテキストを確認してください。");

                                    break;
                            }
                        }

                        break;

                    //選択肢の表示
                    case 's':

                        OperationText = PickString();

                        //文字列が返ってきたら
                        if (OperationText != null)
                        {
                            SplitText = SplitString(OperationText, '|');

                            List<string> ChoiceSplit;

                            for(int i = 0;i < SplitText.Count;i++)
                            {
                                ChoiceSplit = SplitString(SplitText[i], ',');

                                m_ChoicesList.Add(Instantiate(m_ChoiceObject));

                                m_ChoicesList[i].transform.SetParent(m_Canvas.transform);

                                m_ChoicesList[i].transform.FindChild("Text").GetComponent<Text>().text = ChoiceSplit[0];

                                m_ChoicesList[i].GetComponent<ChoiceButton>().m_ReadText = ChoiceSplit[1];

                                float ChoiceHeight = m_ChoicesList[i].GetComponent<RectTransform>().sizeDelta.y;

                                m_ChoicesList[i].GetComponent<RectTransform>().localPosition = new Vector3(0, ChoiceHeight * (SplitText.Count - 1) - ((ChoiceHeight * 2) * i), 0);
                            }

                            m_CurrentTextNum = 0;
                            m_ChoiceParent.SetActive(true);
                            m_State = ConversationState.Choice;        
                        }

                        break;
                  }

                break;

            //他は通常文字
            default:

                //通常文字なのでtrueを返す。
                return true;
        }

        return false;
    }
}
