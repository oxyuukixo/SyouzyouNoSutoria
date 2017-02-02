using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using GodTouches;

public class BattleMain : MonoBehaviour
{
    // 配置するプレハブの読み込み
    public GameObject prefabNormal;
    public GameObject prefabWall;
    public GameObject[] Player;
    public GameObject[] Status_TAC; // 順番
    public int TurnElapsedNum;      // ターン数

    protected int m_SceneTask;

    private int m_iPlayerNum;   // 呼び出すキャラクターの番号
    private UICtrl m_UIClass;

    // Use this for initialization
    void Start()
    {
        // 配置元のオブジェクト指定
        //GameObject stageObject = GameObject.FindWithTag("Stage");
        //// ステージ配置
        //for (int i = 0; i < 20; i++)
        //{
        //    for (int j = 0; j < 20; j++)
        //    {
        //        Vector3 tile_pos = new Vector3(
        //            0 + prefabNormal.transform.localScale.x * i,
        //            0,
        //            0 + prefabNormal.transform.localScale.z * j

        //          );

        //        if (prefabNormal != null)
        //        {
        //            // プレハブの複製 
        //            GameObject instant_object =
        //              (GameObject)GameObject.Instantiate(prefabNormal,
        //                                                  tile_pos, Quaternion.identity);
        //            // 生成元の下に複製したプレハブをくっつける
        //            instant_object.transform.parent = stageObject.transform;
        //        }
        //    }
        //}
        // 生成元の下に複製したプレハブをくっつける
        //Vector3 tile_pos = new Vector3(
        //-1 + prefabNormal.transform.localScale.x,
        //0,
        //-1 + prefabNormal.transform.localScale.z
        //);

        //GameObject instant_object = (GameObject)GameObject.Instantiate(prefabNormal, tile_pos, Quaternion.identity);
        //instant_object.transform.parent = stageObject.transform;

        for (int i = 0; i < Player.Length; i++)
        {
            Player[i].SetActive(false);
        }

        m_iPlayerNum = 0;
        TurnElapsedNum = 0;
        m_UIClass = GetComponent<UICtrl>();
    }

    // Update is called once per frame
    void Update()
    {
        // プレイアブルキャラクターの初期配置
        if (m_iPlayerNum < Player.Length)
        {
            if (CrossPlatformInputManager.GetButtonDown("Fire1") || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            {
                m_UIClass.m_Start.enabled = false;

                RaycastHit hit;     // 光線に当たったオブジェクトを受け取るクラス
                Ray ray;            // 光線クラス

                // スクリーン座標に対してマウスの位置の光線を取得
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // マウスの光線の先にオブジェクトが存在していたら hit に入る
                if (Physics.Raycast(ray, out hit))
                {
                    // 当たったオブジェクトを取得
                    // ステージ上
                    if (hit.collider.gameObject.tag == "Stage")
                    {
                        StageInfo stage = hit.collider.gameObject.GetComponent<StageInfo>();
                        Player[m_iPlayerNum].SetActive(true);
                        Player[m_iPlayerNum].transform.position = hit.transform.position + new Vector3(0.5f, 0.662f, 0.5f);
                        m_iPlayerNum++;
                        if (m_iPlayerNum == Player.Length) TurnController.SetCharacter();
                    }
                }
            }
        }
        m_UIClass.m_Turn.text = TurnElapsedNum.ToString();
    }
}
