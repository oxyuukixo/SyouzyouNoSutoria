using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

public class Saizyutsu : MonoBehaviour
{

    //一度に壁を置ける数
    public int WallMaxNum = 3;

    //壁となるオブジェクト
    public GameObject WallObject;

    //彩術を使うかどうかのフラグ
    [HideInInspector]
    public bool IsSaizyutsu;

    //ブロックを置いたマスを保持しておくための変数
    private GameObject[] InstWall;

    //設置した数
    private int InstNum;

    // Use this for initialization
    void Start()
    {

        InstWall = new GameObject[WallMaxNum];
    }

    // Update is called once per frame
    void Update()
    {
        //彩術を使用する
        if (/*IsSaizyutsu &&*/CrossPlatformInputManager.GetButton("Fire1") && InstNum < WallMaxNum)
        {
            RaycastHit hit;  // 光線に当たったオブジェクトを受け取るクラス 
            Ray ray;  // 光線クラス

            // スクリーン座標に対してマウスの位置の光線を取得
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // マウスの光線の先にオブジェクトが存在していたら hit に入る 
            if (Physics.Raycast(ray, out hit))
            {
                // 当たったオブジェクトのTileBaseクラスを取得
                if (hit.collider.gameObject.tag == "Stage" /*&&壁を置けるマスだったら*/)
                {
                    //壁を置くか
                    bool IsInst = true;

                    for(int i = 0; i<InstWall.Length;i++)
                    {
                        if (InstWall[i] == hit.collider.gameObject)
                        {
                            IsInst = false;
                        }
                    }

                    if (IsInst)
                    {
                        //彩術が使用されたマスにする処理？
                        //壁または橋がある判定
                        //
                        //

                        //壁を作成
                        GameObject NowWall = Instantiate(WallObject);

                        //壁の位置をマスの位置にする
                        NowWall.transform.position = hit.transform.position + new Vector3(0, 0.5f, 0);

                        //置いたマスの情報を保存(キャンセルしたときに戻すため)
                        InstWall[InstNum] = hit.collider.gameObject;

                        //置いた数を増やす
                        InstNum++;
                    }
                }
            }
        }
    }
}
