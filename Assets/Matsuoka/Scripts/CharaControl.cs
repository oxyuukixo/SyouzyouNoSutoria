using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class CharaControl : MonoBehaviour {

    private Color default_color;     // 初期化カラー
    private Color select_color;      // 選択時カラー

    protected Material m_Material;

    public bool bColorState;
    public int iSelectCommand;
    
    NavMeshAgent agent;
    UICtrl m_UIClass;

	// Use this for initialization
	void Start ()
    {
        // このクラスが付属しているマテリアルを取得 
        m_Material = this.gameObject.GetComponent<Renderer>().material;
        // 選択時と非選択時のカラーを保持 
        default_color = m_Material.color;
        select_color = Color.magenta;
        bColorState = false;
        agent = GetComponent<NavMeshAgent>();
        m_UIClass = GameObject.Find("Common/GameManager").GetComponent<UICtrl>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Material.color = default_color;
        // StageBaseからbColorStateの値がtrueにされていれば色をかえる
        if (bColorState)
        {
            bColorState = false;
            m_Material.color = select_color;
        }

        switch(iSelectCommand)
        {
            case 0: //待機
                Idle();
                break;
            case 1: // 移動
                break;
            case 2: // 攻撃
                break;
            case 3: // 魔法
                break;
            case 4: // スキル
                break;
            case 5: // アイテム
                break;
            case 6: // チェンジ
                break;
            case 7: // エンド
                break;
            case 8: // 彩術
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
            if (hit.collider.gameObject.tag == "Player")
            {
                bColorState = true;
                m_UIClass.m_Status[0].enabled = true;

                if (CrossPlatformInputManager.GetButtonDown("Fire1"))
                {
                    CommandUITrue(0);
                }
            }
            else if (!m_UIClass.m_Command[0].activeInHierarchy)
            {
                m_UIClass.m_Status[0].enabled = false;
            }

            if (hit.collider.gameObject.tag == "Stage")
            {
                if (CrossPlatformInputManager.GetButtonDown("Fire1"))
                {
                    CommandUIFalse(0);
                }
            }
        }
    }

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
}
