using UnityEngine;
using System.Collections;


public enum Direction
{
    top,
    right,
    left,
    bottom
}


public class Status : MonoBehaviour {

    public int HP;          
    public int MP;
    public int AT;
    public int MAT;
    public int DF;
    public int MDF;
    public int TEC;         // ダメージ補正
    public int MOV;         // 移動力,範囲
    public int HEIGHT;      // 今いる高さ,0が一番低い
    public int DIRECTION;   // キャラクターの向き,0から

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
