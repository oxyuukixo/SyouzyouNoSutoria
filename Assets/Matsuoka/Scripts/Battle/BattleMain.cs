using UnityEngine;
using System.Collections;

public class BattleMain : MonoBehaviour {

    // 配置するプレハブの読み込み
    public GameObject prefabNormal;
    public GameObject prefabWall;

    public int m_MapSize = 0;

    protected int m_SceneTask;

    // Use this for initialization
    void Start()
    {
        // 配置するプレハブの読み込み
        //GameObject prefab = (GameObject)Resources.Load("Objects/Rock");
        //GameObject prefab2 = (GameObject)Resources.Load("Objects/Wall");

        // 配置元のオブジェクト指定
        GameObject stageObject = GameObject.FindWithTag("Stage");
        // タイル配置
        // 生成元の下に複製したプレハブをくっつける
        Vector3 tile_pos = new Vector3(
            -1 + prefabNormal.transform.localScale.x,
            0,
            -1 + prefabNormal.transform.localScale.z
            );
        GameObject instant_object = (GameObject)GameObject.Instantiate(prefabNormal, tile_pos, Quaternion.identity);
        instant_object.transform.parent = stageObject.transform;

        //for (int i = 0; i < m_MapSize; i++)
        //{
        //    for (int j = 0; j < m_MapSize; j++)
        //    {
        //        Vector3 tile_pos = new Vector3(
        //            0 + prefabNormal.transform.localScale.x * i,
        //            (float)i/10.0f,
        //            0 + prefabNormal.transform.localScale.z * j
        //            );

        //        if (prefabNormal != null && prefabWall != null)
        //        {
        //            // プレハブの複製
        //            if (i != 5 && j != 5)
        //            {
        //                GameObject instant_object = (GameObject)GameObject.Instantiate(prefabNormal, tile_pos, Quaternion.identity);
                        
        //                // 生成元の下に複製したプレハブをくっつける
        //                instant_object.transform.parent = stageObject.transform;
        //            }
        //            else
        //            {
        //                GameObject instant_object = (GameObject)GameObject.Instantiate(prefabWall, tile_pos, Quaternion.identity);

        //                // 生成元の下に複製したプレハブをくっつける
        //                instant_object.transform.parent = stageObject.transform;
        //            }
        //        }
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {

    }
}
