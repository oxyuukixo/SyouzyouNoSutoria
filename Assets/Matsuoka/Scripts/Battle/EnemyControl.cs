using UnityEngine;
using System.Collections;

public class EnemyControl : MonoBehaviour {

    public StageInfo m_StageInfoClass;

    private Status m_StatusClass;
    private UICtrl m_UIClass;

    // Use this for initialization
    void Start () {
        m_StatusClass = GetComponent<Status>();
        m_StageInfoClass = GetComponent<StageInfo>();
        m_UIClass = GameObject.Find("GameManager").GetComponent<UICtrl>();
    }
	
	// Update is called once per frame
	void Update () {
        // 今いるマップの情報を取得
        GetStageInfo();
        if (gameObject != TurnController.m_turnCharacter) return;
        GetComponent<CPAI>().AI();
    }

    // 今いるマップの情報を取得
    void GetStageInfo()
    {
        // 光線に当たったオブジェクトを受け取るクラス
        RaycastHit hit;
        // 光線の原点と向き
        Vector3 rayOrigin = transform.position;
        Vector3 rayDir = -transform.up;

        // マウスの光線の先にオブジェクトが存在していたら hit に入る 
        if (Physics.Raycast(rayOrigin, rayDir, out hit))
        {
            // 当たったオブジェクトのTileBaseクラスを取得
            if (hit.collider.gameObject.tag == "Stage")
            {
                m_StageInfoClass = hit.collider.gameObject.GetComponent<StageInfo>();
                m_StatusClass.HEIGHT = m_StageInfoClass.height;
            }
        }
    }
}
