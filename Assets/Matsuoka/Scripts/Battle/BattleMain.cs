using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using GodTouches;

public class BattleMain : MonoBehaviour
{
    // 配置するプレハブの読み込み
    public GameObject prefabNormal;
    public GameObject prefabWall;
    public GameObject animationCamera;
    public GameObject[] Player;
    public List<GameObject> Enemy;
    public GameObject[] Status_TAC; // 順番
    public int TurnElapsedNum;      // ターン数
    public string m_NextScene;

    protected int m_SceneTask;
    
    private int m_iPlayerNum;   // 呼び出すキャラクターの番号
    private float m_time = 0f;  // 終了までの時間
    [SerializeField] private float m_WaitTime = 5f; // 終了までの時間
    private UICtrl m_UIClass;
    private CameraControl m_CameraCtrl;

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
        m_CameraCtrl = GameObject.Find("Camera").GetComponent<CameraControl>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Enemy.Count; i++)
        {
            if (Enemy[i] == null)
            {
                Enemy.RemoveAt(i);
            }
        }
        if (animationCamera.GetComponent<Animation>().isPlaying) return;
        // プレイアブルキャラクターの初期配置
        if (m_iPlayerNum < Player.Length)
        {
            if (CrossPlatformInputManager.GetButtonUp("Fire1") || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
            {
                m_UIClass.m_Start.enabled = false;

                RaycastHit hit;     // 光線に当たったオブジェクトを受け取るクラス
                Ray ray;            // 光線クラス
                // スクリーン座標に対してマウスの位置の光線を取得
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // マウスの光線の先にオブジェクトが存在していたら hit に入る
                if (Physics.Raycast(ray, out hit))
                {
                    m_CameraCtrl.m_CenterObj = hit.collider.gameObject;
                    // 当たったオブジェクトを取得
                    // ステージ上
                    if (hit.collider.gameObject.tag == "Stage")
                    {
                        StageInfo stage = hit.collider.gameObject.GetComponent<StageInfo>();
                        if (stage.possible) return;
                        Player[m_iPlayerNum].SetActive(true);
                        Player[m_iPlayerNum].transform.position = hit.transform.position + new Vector3(0.5f, 0.662f, 0.5f);
                        Player[m_iPlayerNum].GetComponent<CharaControl>().oldMapChip = hit.collider.gameObject;
                        stage.possible = true;
                        stage.charaCategory = Player[m_iPlayerNum];
                        m_iPlayerNum++;
                        if (m_iPlayerNum == Player.Length) TurnController.SetCharacter();
                    }
                }
            }
        }

        if (Enemy.Count == 0)
        {
            m_time += Time.deltaTime;
            m_UIClass.m_End.enabled = true;
            if(m_time > m_WaitTime)
            {
                SceneManager.LoadScene(m_NextScene);
            }
        }

        m_UIClass.m_Turn.text = TurnElapsedNum.ToString();
    }
}
