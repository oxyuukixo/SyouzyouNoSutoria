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

    public float StartPos;
    public float EndPos;

    // Use this for initialization
    void Start () {
        m_CamAni = GetComponent<Animation>();
        ui = GameObject.Find("GameManager").GetComponent<UICtrl>();
        m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        m_Offset = new Vector3(0, 0, -20);
        m_CamAni.Play();
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
        transform.position = m_CenterObj.transform.position;
        m_Camera.transform.localPosition = m_Offset;

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

    }

    void InputWheel()
    {
        wheel = CrossPlatformInputManager.GetAxis("Mouse ScrollWheel");
    }
}
