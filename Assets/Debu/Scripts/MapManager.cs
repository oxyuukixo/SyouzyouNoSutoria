using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    //デフォルトで生成するオブジェクト
    public GameObject m_StageObject;

    //マップのサイズ
    public int m_MapX;
    public int m_MapY;

    //マップの配列
    public List<List<GameObject>> m_MapList = new List<List<GameObject>>();

    //マスの間隔
    public float m_StageInterval;

    public float IntervalX = 1;

    public float IntervalY = 1;

    public float ObjectHeight = 1;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
