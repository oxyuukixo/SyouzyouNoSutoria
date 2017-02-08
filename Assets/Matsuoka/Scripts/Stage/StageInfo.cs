using UnityEngine;
using System.Collections;


public class StageInfo : MonoBehaviour {


    public int height;                      // 高さ,1番下が1
    public int[] position = new int[2];     // 位置,原点は(1,1) [0] = x.[1] = z
    public bool possible;                   // 障害物有る無し trueなら無し
    public bool[] m_displayArea;            // 許可範囲の表示フラグ
    public GameObject charaCategory;        // 上にいるキャラクターの種類
    public GameObject stageLook;            // マスの見た目(テクスチャの種類)
    public GameObject[] m_moveArea;         // 移動許可範囲
    public Color[] m_color;                 // 移動エリアの色
    public bool[] m_moveAriaActive;         // 移動範囲の表示判定

    // Use this for initialization
    void Start () {
        // 地面の高さを取得
        float pos_y = transform.position.y * 2;
        height = Mathf.RoundToInt(pos_y);
        // マスの位置を取得
        position[0] = (int)transform.position.x + 1;
        position[1] = (int)transform.position.z + 1;

        //追加部分
        m_displayArea = new bool[(int)MoveAreaType.number];
        m_moveArea = new GameObject[(int)MoveAreaType.number];
        m_color = new Color[(int)MoveAreaType.number];
        m_moveAriaActive = new bool[(int)MoveAreaType.number];
        for (MoveAreaType i = 0; i < MoveAreaType.number; i++)
        {
            m_moveArea[(int)i] = Instantiate(Resources.Load(MoveArea.MoveAreaAssetsName(i)), transform.position, transform.rotation) as GameObject;
            m_moveArea[(int)i].transform.position = new Vector3
            (
             m_moveArea[(int)i].transform.position.x + 0.5f,
             m_moveArea[(int)i].transform.position.y,
             m_moveArea[(int)i].transform.position.z + 0.5f
            );
            m_moveArea[(int)i].name = MoveArea.RemoveNameClone(m_moveArea[(int)i].name);
            m_moveArea[(int)i].transform.parent = gameObject.transform;
            m_color[(int)i] = m_moveArea[(int)i].GetComponent<Renderer>().material.color;
            m_moveArea[(int)i].SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
        Color color;
        m_moveArea[0].SetActive(true);
        color = Color.black;
        color.a = m_color[0].a / 2;
        m_moveArea[0].GetComponent<Renderer>().material.color = color;
        for (int i = 0; i < (int)MoveAreaType.number; i++)
        {
            if (i != 0) return;
            if (m_moveAriaActive[i])
            {
                m_moveArea[i].SetActive(true);
                if (m_displayArea[i]) color = m_color[i];
                else
                {
                    color = Color.black;
                    color.a = m_color[i].a / 2;
                }
                m_moveArea[i].GetComponent<Renderer>().material.color = color;
            }
            else m_moveArea[i].SetActive(false);
        }
    }

}
