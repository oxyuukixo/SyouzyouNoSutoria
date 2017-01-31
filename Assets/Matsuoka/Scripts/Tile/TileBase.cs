using UnityEngine;
using System.Collections;

public class TileBase : MonoBehaviour {

    private Color default_color;  // 初期化カラー
    private Color select_color;    // 選択時カラー

    protected Material m_Material;

    public bool bColorState;

    // Use this for initialization
    void Start()
    {
        // このクラスが付属しているマテリアルを取得 
        m_Material = this.gameObject.GetComponent<Renderer>().material;
        // 選択時と非選択時のカラーを保持 
        default_color = m_Material.color;
        select_color = Color.magenta;
        bColorState = false;
    }

    // Update is called once per frame
    void Update()
    {
        m_Material.color = default_color;
        // StageBaseからbColorStateの値がtrueにされていれば色をかえる 
        if (bColorState)
        {
            bColorState = false;
            m_Material.color = select_color;
        }
    }
}
