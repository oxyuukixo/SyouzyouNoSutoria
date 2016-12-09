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
    void Awake()
    {

        for (int y = m_MapList.Count; y < m_MapY; y++)
        {
            m_MapList.Add(new List<GameObject>());

            for (int x = m_MapList[y].Count; x < m_MapX; x++)
            {
                m_MapList[y].Add(null);
            }
        }

        foreach (Transform child in gameObject.transform)
        {
            StageInfo a = child.gameObject.GetComponent<StageInfo>();

            m_MapList[a.position[1]][a.position[0]] = child.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
