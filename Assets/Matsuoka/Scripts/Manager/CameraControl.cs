using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;
using GodTouches;

public class CameraControl : MonoBehaviour {

    public GameObject m_CenterObj;  // カメラの中心のオブジェクト

    private Animation m_CamAni;
    private UICtrl ui;
    private Camera m_Camera;
    private Vector3 m_Offset;   // カメラの距離 
    private float wheel;        // ホイールの回転量
    private bool m_ScrolFlg = true;    // 開始時のカメラのスクロールフラグ
    private Vector3 startPos;
    private Vector3 moveDistance;
    private Vector3 endPos;

    public float StartPos;
    public float EndPos;

    // Use this for initialization
    void Start () {
        m_CamAni = GetComponent<Animation>();
        ui = GameObject.Find("GameManager").GetComponent<UICtrl>();
        m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        m_Offset = new Vector3(0, 0, -20);
        m_CamAni.Play();
        startPos = m_Camera.transform.localPosition;
	}

    // Update is called once per frame
    void Update()
    {
        // カメラのアニメーション
        if (Input.GetMouseButton(0))
        {
            m_CamAni.Stop();
        }
        if (!m_CamAni.IsPlaying("CameraAnimation") && m_ScrolFlg)
        {
            ui.m_Start.enabled = true;
            m_ScrolFlg = false;
        }
        if (m_CenterObj != null)
        {
            transform.position = m_CenterObj.transform.position;
            m_Camera.transform.localPosition = m_Offset;
        }

        // ズーム・ルーズ
        if (wheel > 0)
        {
            if (m_Camera.orthographicSize > 2)
            {
                m_Camera.orthographicSize--;
            }
        }
        else if(wheel < 0)
        {
            if (m_Camera.orthographicSize < 10)
            {
                m_Camera.orthographicSize++;
            }
        }

        MoveCamera();
        InputWheel();
    }

    void MoveCamera()
    {
        var phase = GodTouch.GetPhase();
        if (phase == GodPhase.Began)
        {
            startPos = GodTouch.GetPosition();
        }
        if (phase == GodPhase.Moved)
        {
            m_CenterObj = null;
            // マウス(タップ)の移動量
            moveDistance = GodTouch.GetPosition() - startPos;
            // カメラの移動
            m_Camera.transform.localPosition -= moveDistance / 75;
            // マウス(タップ)の移動量の初期化
            startPos = GodTouch.GetPosition();
        }
        
        //if (Input.GetMouseButtonDown(0))
        //{
        //    startPos = m_Camera.transform.localPosition;
        //}
        //else if (Input.GetMouseButton(0))
        //{
        //    // マウス(タップ)の移動量
        //    m_Camera.transform.localPosition = Input.mousePosition;
        //}
        //else if (Input.GetMouseButtonUp(0))
        //{
        //    m_Camera.transform.localPosition = startPos;
        //}
    }

    void InputWheel()
    {
        wheel = CrossPlatformInputManager.GetAxis("Mouse ScrollWheel");
    }
}
