using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public enum Direction
{
    top,
    left,
    bottom,
    right
}


public class Status : MonoBehaviour {

    public int Lv;
    public int HP;
    public int MP;
    public int AT;
    public int MAT;
    public int DF;
    public int MDF;
    public int TEC;         // ダメージ補正,素早さ
    public int MOV;         // 移動力,範囲
    public int HEIGHT;      // 今いる高さ,0が一番低い
    public Direction DIRECTION;   // キャラクターの向き,0から
    public int FIRE_RATE;
    public int WATER_RATE;
    public int WIND_RATE;
    public int SOIL_RATE;
    public int CUT_RATE;

    [SerializeField] private Image m_HP;
    [SerializeField] private Image m_MP;

    private int MaxHP;
    private int MaxMP;

    // Use this for initialization
    void Start () {
        MaxHP = HP;
        MaxMP = MP;
    }
	
	// Update is called once per frame
	void Update () {
        m_HP.rectTransform.sizeDelta = new Vector2((float)MaxHP / HP * m_HP.rectTransform.sizeDelta.x, m_HP.rectTransform.sizeDelta.y);
        m_MP.rectTransform.sizeDelta = new Vector2((float)MaxMP / MP * m_MP.rectTransform.sizeDelta.x, m_MP.rectTransform.sizeDelta.y);
    }
}
