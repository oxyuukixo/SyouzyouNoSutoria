using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;


enum PlayerNumber
{
    Default,
    Leo,
    Mashira,
    Ryunne,
    Akane,
    Ciel,
    Ignir
}

enum EnemyNumber
{
    Enemy1 = 11,
    Enemy2,
    Enemy3
}


public class CharaControl : MonoBehaviour {

    private GameObject m_MoveSprite;
    private GameObject m_AttackSprite;
    private Color default_color;     // 初期化カラー
    private Color select_color;      // 選択時カラー
    private bool once;

    protected Material m_Material;

    public int iSelectCommand;
    public bool bColorState;
    public GameObject oldMapChip;

    UICtrl m_UIClass;
    BattleMain m_BMClass;
    Status m_StatusClass;
    StageInfo m_StageInfoClass;
    CharacterMove m_CharaMoveClass;

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
    void Start()
    {
        m_MoveSprite = (GameObject)Resources.Load("Objects/image_0");
        m_AttackSprite = (GameObject)Resources.Load("Objects/image_1");
        // このクラスが付属しているマテリアルを取得 
        m_Material = this.gameObject.GetComponent<Renderer>().material;
        // 選択時と非選択時のカラーを保持 
        default_color = m_Material.color;
        select_color = Color.magenta;
        bColorState = false;
        once = false;
        m_UIClass = GameObject.Find("SceneManager/GameManager").GetComponent<UICtrl>();
        m_BMClass = GameObject.Find("SceneManager/GameManager").GetComponent<BattleMain>();
        m_StatusClass = GetComponent<Status>();
        m_StageInfoClass = GetComponent<StageInfo>();
        m_CharaMoveClass = GetComponent<CharacterMove>();

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
        if (gameObject != TurnController.m_turnCharacter) return;
        switch (iSelectCommand)
        {
            case 0: //待機
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

        m_CharaMoveClass.Move();    // 移動
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
        if (iSelectCommand == 0) MoveArea.MoveAreaSarch(gameObject);
        iSelectCommand = 1;
        CommandUIFalse(0);
        m_UIClass.m_Cover[0].SetActive(false);

        if (CrossPlatformInputManager.GetButtonUp("Fire1"))
        {
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                RaycastHit hit;  // 光線に当たったオブジェクトを受け取るクラス
                Ray ray;  // 光線クラス

                // スクリーン座標に対してマウスの位置の光線を取得
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // マウスの光線の先にオブジェクトが存在していたら hit に入る
                if (Physics.Raycast(ray, out hit))
                {
                    // ステージ上
                    if (hit.collider.gameObject.tag == "Stage")
                    {
                        if (!m_CharaMoveClass.SelectMovePoiont(hit.collider.gameObject))
                        {
                            MoveArea.ResetMoveArea();
                            StageInfo stage = hit.collider.gameObject.GetComponent<StageInfo>();
                            stage.possible = true;
                            stage.charaCategory = gameObject;
                            StageInfo oldStage = oldMapChip.gameObject.GetComponent<StageInfo>();
                            oldStage.possible = false;
                            oldStage.charaCategory = null;
                            oldMapChip = hit.collider.gameObject;
                        }
                        //m_SelectPlayer.transform.position = hit.transform.position + new Vector3(0.5f, (-hit.transform.position.y) + (stage.height / 2) + 0.662f, 0.5f);
                        m_StageInfoClass = hit.collider.gameObject.GetComponent<StageInfo>();
                        m_StatusClass.HEIGHT = m_StageInfoClass.height; // 高さを取得
                        iSelectCommand = 0;
                    }
                }
            }
        }
    }

    public void Attack()
    {
        RaycastHit hit;  //光線に当たったオブジェクトを受け取るクラス
        Ray ray;         //光線クラス
        Status status;   //ステータス
        GameObject enemy;   //敵のオブジェクト
        int damage;      //ダメージ
        if (iSelectCommand == 0) AttackDataList.PhysicalAttackRange(gameObject, PhysicalName.normal);
        iSelectCommand = 2;
        CommandUIFalse(0);
        m_UIClass.m_Cover[0].SetActive(false);
        if (!CrossPlatformInputManager.GetButtonDown("Fire1")) return;
        // スクリーン座標に対してマウスの位置の光線を取得
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // マウスの光線の先にオブジェクトが存在していたら hit に入る
        if (!Physics.Raycast(ray, out hit)) return;

        //マウスの光線に当たったのが敵以外なら何もしない
        if (hit.collider.gameObject.tag == "Enemy")
        {
            enemy = hit.collider.gameObject;
            if (!enemy.GetComponent<EnemyControl>().m_StageInfoClass.m_displayArea[(int)MoveAreaType.player])
            {
                AttackDataList.HideAttackArea(gameObject);
                iSelectCommand = 0;
                return;
            }
        }
        else if (hit.collider.gameObject.tag == "Stage")
        {
            if (!hit.collider.gameObject.GetComponent<StageInfo>().m_displayArea[(int)MoveAreaType.player])
            {
                AttackDataList.HideAttackArea(gameObject);
                iSelectCommand = 0;
                return;
            }
            enemy = hit.collider.gameObject.GetComponent<StageInfo>().charaCategory;
        }
        else
        {
            AttackDataList.HideAttackArea(gameObject);
            iSelectCommand = 0;
            return;
        }

        status = enemy.GetComponent<Status>();
        damage = DamageCalculations.Damege(gameObject, enemy,
            GameLevel.levelEasy, AttackType.Physical, AttackProperty.NoPropertyAttack);
        status.HP -= damage;
        if (status.HP < 0)
        {
            status.HP = 0;
            Destroy(hit.collider.gameObject);
        }
        AttackDataList.HideAttackArea(gameObject);
        iSelectCommand = 0;
    }

    public void Magic()
    {
        RaycastHit hit;  //光線に当たったオブジェクトを受け取るクラス
        Ray ray;         //光線クラス
        Status status;   //ステータス
        GameObject enemy;   //敵のオブジェクト
        int damage;      //ダメージ
        if (iSelectCommand == 0) AttackDataList.MagicAttackRange(gameObject, MagicName.fire);
        iSelectCommand = 3;
        CommandUIFalse(0);
        m_UIClass.m_Cover[0].SetActive(false);
        if (!CrossPlatformInputManager.GetButtonDown("Fire1")) return;
        // スクリーン座標に対してマウスの位置の光線を取得
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // マウスの光線の先にオブジェクトが存在していたら hit に入る
        if (!Physics.Raycast(ray, out hit)) return;

        //マウスの光線に当たったのが敵以外なら何もしない
        if (hit.collider.gameObject.tag == "Enemy")
        {
            enemy = hit.collider.gameObject;
            if (!enemy.GetComponent<EnemyControl>().m_StageInfoClass.m_displayArea[(int)MoveAreaType.player])
            {
                AttackDataList.HideAttackArea(gameObject);
                iSelectCommand = 0;
                return;
            }
        }
        else if (hit.collider.gameObject.tag == "Stage")
        {
            if (!hit.collider.gameObject.GetComponent<StageInfo>().m_displayArea[(int)MoveAreaType.player])
            {
                AttackDataList.HideAttackArea(gameObject);
                iSelectCommand = 0;
                return;
            }
            enemy = hit.collider.gameObject.GetComponent<StageInfo>().charaCategory;
        }
        else
        {
            AttackDataList.HideAttackArea(gameObject);
            iSelectCommand = 0;
            return;
        }


        status = hit.collider.gameObject.GetComponent<Status>();
        damage = DamageCalculations.Damege(gameObject, hit.collider.gameObject,
            GameLevel.levelEasy, AttackType.Magic, AttackProperty.FireAttack);
        status.HP -= damage;
        if (status.HP < 0)
        {
            status.HP = 0;
            Destroy(hit.collider.gameObject);
            m_UIClass.m_End.enabled = true;
        }
        AttackDataList.HideAttackArea(gameObject);
        iSelectCommand = 0;
    }

    public void Skil()
    {
        CommandUIFalse(0);
        m_UIClass.m_Cover[0].SetActive(false);
    }

    public void Item()
    {
        CommandUIFalse(0);
        m_UIClass.m_Cover[0].SetActive(false);
    }

    public void Wait()
    {
        CommandUIFalse(0);
    }

    public void End()
    {
        CommandUIFalse(0);
        m_UIClass.m_Cover[0].SetActive(false);
        m_BMClass.TurnElapsedNum++; // ターン経過
        for (int i = 0; i < m_UIClass.m_Cover.Length; i++)
        {
            m_UIClass.m_Cover[i].SetActive(false);
        }
        //m_BMClass.TurnElapsedNum++;
        // ターン経過
        TurnController.NextMoveCharacter();
    }

    public void Psy()
    {
        iSelectCommand = 8;
        CommandUIFalse(0);
        m_UIClass.m_Cover[0].SetActive(false);

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

    public void ChangeOnStagePossible(bool flag)
    {
        oldMapChip.GetComponent<StageInfo>().possible = flag;
    }




}

