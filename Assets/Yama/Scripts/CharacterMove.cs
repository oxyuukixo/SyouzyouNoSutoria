﻿using UnityEngine;
using System.Collections;

public class CharacterMove : MonoBehaviour {

    public GameObject[][] m_stage;
    public float m_speed;               //移動速度
    public bool m_move;                 //移動判定

    private Vector2[] m_moveRoute;      //移動ルート
    private int m_routeCount;           //移動ルート番号

    private MapManager m_MMClass;

    void Start()
    {
        m_routeCount = 0;
        m_MMClass = GameObject.Find("Stage").GetComponent<MapManager>();

        m_stage = new GameObject[m_MMClass.m_MapList.Count][];
        for (int i = 0; i < m_MMClass.m_MapList.Count; i++)
        {
            m_stage[i] = m_MMClass.m_MapList[i].ToArray();
        }
    }

	//移動
	public void Move()
    {
        if (!m_move) return;
        MoveLenge();
        if (m_moveRoute.Length != m_routeCount) return;
        m_move = false;
        m_moveRoute = null;
        m_routeCount = 0;
	}

    //移動距離を出して移動
    void MoveLenge()
    {
        Vector2 movePoint;          //移動地点
        Vector2 moveLouteLenge;     //移動ルートまでの距離
        int stageX;                 //ステージX
        int stageY;                 //ステージY
        float height;               //高さ
        float routeLenge;           //移動ルートまでの直線距離
        float speed;                //直線移動の移動速度
        float radian;               //ラジアン角
        stageX = (int)m_moveRoute[m_routeCount].x;
        stageY = (int)m_moveRoute[m_routeCount].y;
        moveLouteLenge = new Vector2
        (
         gameObject.transform.position.z - m_stage[stageY][stageX].transform.position.z,
         gameObject.transform.position.x - m_stage[stageY][stageX].transform.position.x
        );
        radian = Mathf.Atan2(moveLouteLenge.y, moveLouteLenge.x);
        routeLenge = Mathf.Abs(Mathf.Sqrt(Mathf.Pow(moveLouteLenge.y, 2) + Mathf.Pow(moveLouteLenge.x, 2)));
        //自分の座標と移動座標の差が移動速度未満だったら
        if (routeLenge < m_speed) speed = routeLenge;
        else speed = m_speed;
        movePoint.x = gameObject.transform.position.x + Mathf.Cos(radian) * speed;
        movePoint.y = gameObject.transform.position.z + Mathf.Sin(radian) * speed;
        height = m_stage[Mathf.RoundToInt(movePoint.y)][Mathf.RoundToInt(movePoint.x)].transform.position.y;
        gameObject.transform.position = new Vector3
        (
         movePoint.x,
         height,
         movePoint.y
        );
        if (gameObject.transform.position != m_stage[stageY][stageX].transform.position) return;
        m_routeCount++;
    }

    //移動ルートを検索する
    public void SelectMovePoiont(GameObject movePoint)
    {
        m_moveRoute = SarchRange.SarchMoveRoute(m_stage, gameObject, movePoint, GetComponent<Status>().MOV);
        m_move = true;
    }
}
