using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(MapManager))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //元のInspector部分を表示
        base.OnInspectorGUI();

        MapManager m_MapManager = target as MapManager;

        //ボタンを表示
        if (GUILayout.Button("Button"))
        {
            //オブジェクトをすべて削除
            for (int i = m_MapManager.transform.childCount - 1; i >= 0; --i)
            {
                GameObject.DestroyImmediate(m_MapManager.transform.GetChild(i).gameObject);
            }

            //オブジェクトを生成
            GameObject RespawnObject = Instantiate(m_MapManager.res);

            //子オブジェクトにする
            RespawnObject.transform.parent = m_MapManager.transform;
        }

        //ボタンを表示
        if (GUILayout.Button("Button"))
        {
            //オブジェクトを生成
            GameObject RespawnObject = Instantiate(m_MapManager.res);

            //子オブジェクトにする
            RespawnObject.transform.parent = m_MapManager.transform;
        }
    }
}
