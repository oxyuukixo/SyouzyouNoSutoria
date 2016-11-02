using UnityEngine;
using System.Collections;

public class StageInfo : MonoBehaviour {

    public int height;                      // 高さ,1番下が1
    public int[] position = new int[2];     // 位置,原点は(1,1) [0] = x.[1] = z
    public bool possible;                   // 障害物有る無し trueなら無し
    public GameObject charaCategory;        // 上にいるキャラクターの種類

	// Use this for initialization
	void Start () {
        // 地面の高さを取得
        float pos_y = transform.position.y * 2;
        height = Mathf.RoundToInt(pos_y);
        // マスの位置を取得
        position[0] = (int)transform.position.x + 1;
        position[1] = (int)transform.position.z + 1;
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
