using UnityEngine;
using System.Collections;

public class BattleMain : MonoBehaviour {

    public int m_MapSize = 10;

    protected int m_SceneTask;

    // Use this for initialization
    void Start()
    {
        // 配置するプレハブの読み込み 
        GameObject prefab = (GameObject)Resources.Load("Objects/Rock");
        // 配置元のオブジェクト指定 
        GameObject stageObject = GameObject.FindWithTag("Stage");
        // タイル配置
        for (int i = 0; i < m_MapSize; i++)
        {
            for (int j = 0; j < m_MapSize; j++)
            {

                Vector3 tile_pos = new Vector3(
                    0 + prefab.transform.localScale.x * i,
                    0,
                    0 + prefab.transform.localScale.z * j

                  );

                if (prefab != null)
                {
                    // プレハブの複製 
                    GameObject instant_object =
                      (GameObject)GameObject.Instantiate(prefab,
                                                          tile_pos, Quaternion.identity);
                    // 生成元の下に複製したプレハブをくっつける 
                    instant_object.transform.parent = stageObject.transform;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
