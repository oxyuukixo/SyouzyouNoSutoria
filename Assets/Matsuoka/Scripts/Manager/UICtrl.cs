using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using GodTouches;

public class UICtrl : MonoBehaviour {

    public Image[] m_Chara;
    public GameObject[] m_Command;
    public Image[] m_ComIcon;
    public Image[] m_Status;
    public Text m_Turn;
    public GameObject[] m_Cover;
    public Text m_Start;
    public Text m_End;

    private Vector3 startPos;
    private Vector3 moveDistance;
    private Vector3 endPos;

    private float[] m_x;
    private float[] m_y;
    private float[] m_radian;
    private float m_radius;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < m_Chara.Length; i++)
        {
            m_Chara[i].enabled = false;
            m_Command[i].SetActive(false);
            m_Status[i].enabled = false;
        }
        m_Cover[0].SetActive(false);
        m_Start.enabled = false;
        m_End.enabled = false;

        m_x = new float[m_ComIcon.Length];
        m_y = new float[m_ComIcon.Length];
        m_radian = new float[m_ComIcon.Length];
        m_radius = 500;
        for (int i = 0; i < m_radian.Length; i++)
        {
            m_radian[i] = i * -10;
        }
    }
	
	// Update is called once per frame
	void Update () {
        CommandUI();
    }

    void CommandUI()
    {
        for (int i = 0; i < m_ComIcon.Length; i++)
        {
            m_x[i] = m_radius * Mathf.Cos(Mathf.PI / 180 * m_radian[i]);
            m_y[i] = m_radius * Mathf.Sin(Mathf.PI / 180 * m_radian[i]);
            m_ComIcon[i].transform.localPosition = new Vector3(m_x[i] - 460, m_y[i] - 60);
        }
        var phase = GodTouch.GetPhase();
        if (phase == GodPhase.Began)
        {
            startPos = GodTouch.GetPosition();
        }
        if (phase == GodPhase.Moved)
        {
            // マウス(タップ)の移動量
            moveDistance = GodTouch.GetPosition() - startPos;
            // コマンドボタンのスライド
            if (/*m_ComIcon[m_ComIcon.Length - 1].transform.localPosition.y <= -65 &&*/
               moveDistance.y > 0)
            {
                for (int i = 0; i < m_radian.Length; i++)
                {
                    m_radian[i] += moveDistance.y / 10;
                }
            }
            else if (/*m_ComIcon[0].transform.localPosition.y >= -65 &&*/
               moveDistance.y < 0)
            {
                for (int i = 0; i < m_radian.Length; i++)
                {
                    m_radian[i] += moveDistance.y / 10;
                }
            }
            // マウス(タップ)の移動量の初期化
            startPos = GodTouch.GetPosition();
        }
    }
}
