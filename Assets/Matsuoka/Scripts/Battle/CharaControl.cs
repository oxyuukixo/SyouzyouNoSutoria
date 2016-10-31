﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;


enum PlayerNumber
{
    Default,
    Leo,
    Mashira,
    Lune,
    Cuon,
    Ciel,
    Ignir
}

enum EnemyNumber
{
    Enemy1 = 7
}


public class CharaControl : MonoBehaviour {

    private GameObject m_MoveSprite;
    private GameObject m_AttackSprite;
    private GameObject m_SelectPlayer;
    private Color default_color;     // 初期化カラー
    private Color select_color;      // 選択時カラー
    private int iSelectCommand;
    private int iPlayerNum;
    private int CharactorCount;
    private bool once;

    protected Material m_Material;

    public bool bColorState;

    NavMeshAgent agent;
    UICtrl m_UIClass;
    BattleMain m_BMClass;
    Status m_StatusClass;

    // 彩術用
    //一度に壁を置ける数
    public int WallMaxNum = 3;
    //壁となるオブジェクト
    public GameObject WallObject;
    //彩術を使うかどうかのフラグ
    [HideInInspector] public bool IsSaizyutsu;
    //ブロックを置いたマスを保持しておくための変数
    private GameObject[] InstWall;
    //設置した数
    private int InstNum;


    // Use this for initialization
    void Start ()
    {
        m_MoveSprite = (GameObject)Resources.Load("Objects/image_0");
        m_AttackSprite = (GameObject)Resources.Load("Objects/image_1");
        // このクラスが付属しているマテリアルを取得 
        m_Material = this.gameObject.GetComponent<Renderer>().material;
        // 選択時と非選択時のカラーを保持 
        default_color = m_Material.color;
        select_color = Color.magenta;
        bColorState = false;
        CharactorCount = 6;
        once = false;
        agent = GetComponent<NavMeshAgent>();
        m_UIClass = GameObject.Find("Common/GameManager").GetComponent<UICtrl>();
        m_BMClass = GameObject.Find("Common/GameManager").GetComponent<BattleMain>();
        m_StatusClass = GetComponent<Status>();

        // 彩術用
        InstWall = new GameObject[WallMaxNum];
    }

    // Update is called once per frame
    void Update()
    {
        m_Material.color = default_color;
        // StageBaseからbColorStateの値がtrueにされていれば色をかえる
        if (bColorState)
        {
            m_Material.color = select_color;
        }
        
        switch (iSelectCommand)
        {
            case 0: //待機
                Idle();
                break;
            case 1: // 移動
                Move();
                break;
            case 2: // 攻撃
                Attack();
                break;
            case 3: // 魔法
                Magic();
                break;
            case 4: // スキル
                Skil();
                break;
            case 5: // アイテム
                Item();
                break;
            case 6: // 待機モード
                Wait();
                break;
            case 7: // エンド
                End();
                break;
            case 8: // 彩術
                Psy();
                break;
        }
    }

    public void Idle()
    {
        RaycastHit hit;  // 光線に当たったオブジェクトを受け取るクラス
        Ray ray;  // 光線クラス

        // スクリーン座標に対してマウスの位置の光線を取得
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // マウスの光線の先にオブジェクトが存在していたら hit に入る
        if (Physics.Raycast(ray, out hit))
        {
            // 当たったオブジェクトを取得
            CharactorNumber(hit.collider.gameObject);
            switch(iPlayerNum)
            {
                case 0: // ステージ
                    if (CrossPlatformInputManager.GetButtonDown("Fire1"))
                    {
                        for (int i = 0; i < CharactorCount; i++)
                        {
                            CommandUIFalse(i);
                        }
                    }
                    for (int i = 0; i < CharactorCount; i++)
                    {
                        if (!m_UIClass.m_Command[i].activeInHierarchy)
                        {
                            m_UIClass.m_Status[i].enabled = false;
                        }
                    }
                    break;
                case 1: // レオ
                    m_SelectPlayer = hit.collider.gameObject;
                    m_UIClass.m_Status[0].enabled = true;

                    if (CrossPlatformInputManager.GetButtonDown("Fire1"))
                    {
                        for (int i = 0; i < CharactorCount; i++)
                        {
                            CommandUIFalse(i);
                            if (!m_UIClass.m_Command[i].activeInHierarchy)
                            {
                                m_UIClass.m_Status[i].enabled = false;
                            }
                        }
                        CommandUITrue(0);
                    }
                    break;
                case 2: // マシラ
                    m_SelectPlayer = hit.collider.gameObject;
                    m_UIClass.m_Status[1].enabled = true;

                    if (CrossPlatformInputManager.GetButtonDown("Fire1"))
                    {
                        for (int i = 0; i < CharactorCount; i++)
                        {
                            CommandUIFalse(i);
                            if (!m_UIClass.m_Command[i].activeInHierarchy)
                            {
                                m_UIClass.m_Status[i].enabled = false;
                            }
                        }
                        CommandUITrue(1);
                    }
                    break;
                case 3: // リュンヌ
                    m_SelectPlayer = hit.collider.gameObject;
                    m_UIClass.m_Status[1].enabled = true;

                    if (CrossPlatformInputManager.GetButtonDown("Fire1"))
                    {
                        for (int i = 0; i < CharactorCount; i++)
                        {
                            CommandUIFalse(i);
                            if (!m_UIClass.m_Command[i].activeInHierarchy)
                            {
                                m_UIClass.m_Status[i].enabled = false;
                            }
                        }
                        CommandUITrue(1);
                    }
                    break;
                case 4: // 紅音
                    m_SelectPlayer = hit.collider.gameObject;
                    m_UIClass.m_Status[1].enabled = true;

                    if (CrossPlatformInputManager.GetButtonDown("Fire1"))
                    {
                        for (int i = 0; i < CharactorCount; i++)
                        {
                            CommandUIFalse(i);
                            if (!m_UIClass.m_Command[i].activeInHierarchy)
                            {
                                m_UIClass.m_Status[i].enabled = false;
                            }
                        }
                        CommandUITrue(1);
                    }
                    break;
                case 5: // シエル
                    m_SelectPlayer = hit.collider.gameObject;
                    m_UIClass.m_Status[1].enabled = true;

                    if (CrossPlatformInputManager.GetButtonDown("Fire1"))
                    {
                        for (int i = 0; i < CharactorCount; i++)
                        {
                            CommandUIFalse(i);
                            if (!m_UIClass.m_Command[i].activeInHierarchy)
                            {
                                m_UIClass.m_Status[i].enabled = false;
                            }
                        }
                        CommandUITrue(1);
                    }
                    break;
                case 6: // イーグニズル
                    m_SelectPlayer = hit.collider.gameObject;
                    m_UIClass.m_Status[1].enabled = true;

                    if (CrossPlatformInputManager.GetButtonDown("Fire1"))
                    {
                        for (int i = 0; i < CharactorCount; i++)
                        {
                            CommandUIFalse(i);
                            if (!m_UIClass.m_Command[i].activeInHierarchy)
                            {
                                m_UIClass.m_Status[i].enabled = false;
                            }
                        }
                        CommandUITrue(1);
                    }
                    break;
                case -1:
                    break;
            }
        }
    }

    // 選択されたキャラクター
    public void CharactorNumber(GameObject obj)
    {
        if(obj.tag == "Untagged")
        {
            iPlayerNum = -1;
        }
        if(obj.tag == "Stage")
        {
            iPlayerNum = (int)PlayerNumber.Default;
        }
        if(obj.name == "Leo")
        {
            iPlayerNum = (int)PlayerNumber.Leo;
        }
        if (obj.name == "Mashira")
        {
            iPlayerNum = (int)PlayerNumber.Mashira;
        }
        if (obj.name == "Enemy1")
        {
            iPlayerNum = (int)EnemyNumber.Enemy1;
        }
    }

    // UIの一括管理
    public void CommandUITrue(int i)
    {
        m_UIClass.m_Chara[i].enabled = true;
        m_UIClass.m_Command[i].SetActive(true);
    }
    public void CommandUIFalse(int i)
    {
        m_UIClass.m_Chara[i].enabled = false;
        m_UIClass.m_Command[i].SetActive(false);
    }

    public void Move()
    {
        iSelectCommand = 1;
        CommandUIFalse(0);

        //if (!once)
        //{
        //    int i;
        //    for (i = m_StatusClass.MOV; i > 1;)
        //    {
        //        i = MoveSearch(i);
        //    }
        //    if (i == 1)
        //    {
        //        once = true;
        //    }
        //}
        
        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            RaycastHit hit;  // 光線に当たったオブジェクトを受け取るクラス
            Ray ray;  // 光線クラス

            // スクリーン座標に対してマウスの位置の光線を取得
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // マウスの光線の先にオブジェクトが存在していたら hit に入る
            if (Physics.Raycast(ray, out hit))
            {
                // 当たったオブジェクトを取得
                CharactorNumber(hit.collider.gameObject);
                // ステージ上
                if (iPlayerNum == (int)PlayerNumber.Default)
                {
                    m_SelectPlayer.transform.position = hit.transform.position + new Vector3(0.5f, 0.662f, 0.5f);
                    iSelectCommand = 0;
                }
            }

            //if (m_SelectPlayer.transform.position.x < m_BMClass.m_MapSize - 1)
            //{
            //    m_SelectPlayer.transform.position += new Vector3(1, 0, 0);
            //    iSelectCommand = 0;
            //    once = false;
            //}
        }
    }

    //private int MoveSearch(int MovePow)
    //{
    //    Vector3 Posx1z = transform.position + new Vector3(1.0f, 0.0f, 0.0f);
    //    Vector3 Posxz1 = transform.position + new Vector3(0.0f, 0.0f, 1.0f);
    //    Vector3 Posx_1z = transform.position + new Vector3(-1.0f, 0.0f, 0.0f);
    //    Vector3 Posxz_1 = transform.position + new Vector3(0.0f, 0.0f, -1.0f);

    //    Instantiate(m_MoveSprite, Posx1z, new Quaternion());
    //    Instantiate(m_MoveSprite, Posxz1, new Quaternion());
    //    Instantiate(m_MoveSprite, Posx_1z, new Quaternion(90, 0, 0, 0));
    //    Instantiate(m_MoveSprite, Posxz_1, new Quaternion(90, 0, 0, 0));

    //    MovePow--;
    //    return MovePow;
    //}

    public void Attack()
    {
        CommandUIFalse(0);

        if (m_SelectPlayer.transform.position.z < m_BMClass.m_MapSize - 1)
        {
            m_SelectPlayer.transform.position += new Vector3(0, 0, 1);
        }
    }

    public void Magic()
    {
        CommandUIFalse(0);
    }

    public void Skil()
    {
        CommandUIFalse(0);
    }

    public void Item()
    {
        CommandUIFalse(0);
    }

    public void Wait()
    {
        CommandUIFalse(0);
    }

    public void End()
    {
        CommandUIFalse(0);
    }

    public void Psy()
    {
        iSelectCommand = 8;
        CommandUIFalse(0);

        //彩術を使用する
        if (/*IsSaizyutsu &&*/CrossPlatformInputManager.GetButton("Fire1") && InstNum < WallMaxNum)
        {
            RaycastHit hit;  // 光線に当たったオブジェクトを受け取るクラス 
            Ray ray;  // 光線クラス

            // スクリーン座標に対してマウスの位置の光線を取得
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // マウスの光線の先にオブジェクトが存在していたら hit に入る 
            if (Physics.Raycast(ray, out hit))
            {
                // 当たったオブジェクトのTileBaseクラスを取得
                if (hit.collider.gameObject.tag == "Stage" /*&&壁を置けるマスだったら*/)
                {
                    //壁を置くか
                    bool IsInst = true;

                    for (int i = 0; i < InstWall.Length; i++)
                    {
                        if (InstWall[i] == hit.collider.gameObject)
                        {
                            IsInst = false;
                        }
                    }

                    if (IsInst)
                    {
                        //彩術が使用されたマスにする処理？
                        //壁または橋がある判定
                        //
                        //

                        //壁を作成
                        GameObject NowWall = Instantiate(WallObject);

                        //壁の位置をマスの位置にする
                        NowWall.transform.position = hit.transform.position + new Vector3(0, 0.5f, 0);

                        //置いたマスの情報を保存(キャンセルしたときに戻すため)
                        InstWall[InstNum] = hit.collider.gameObject;

                        //置いた数を増やす
                        InstNum++;

                        // コマンド初期化
                        iSelectCommand = 0;
                    }
                }
            }
        }
    }
}

