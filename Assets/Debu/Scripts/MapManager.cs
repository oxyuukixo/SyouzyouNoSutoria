using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    //デフォルトで生成するオブジェクト
    public GameObject m_StageObject;

    //マップのサイズ
    [HideInInspector]
    public int m_MapX;
    [HideInInspector]
    public int m_MapY;

    //マップの配列
    [HideInInspector]
    public List<List<GameObject>> m_MapList = new List<List<GameObject>>();

    //マスの間隔
    [HideInInspector]
    public float m_StageInterval;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
