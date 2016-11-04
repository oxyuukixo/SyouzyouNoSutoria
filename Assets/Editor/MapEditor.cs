using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof(MapManager))]
public class MapEditor : Editor
{
    [MenuItem("MyTools/Test Selected Object")]
    static void Create()
    {
        Debug.Log(Selection.gameObjects.Length);

        foreach (GameObject go in Selection.gameObjects)
        {
            Debug.Log(go.name);
        }
    }

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

            List<Object> objects = new List<Object>();

            objects.Add(RespawnObject);

            Selection.objects = objects.ToArray();

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

        //ボタンを表示
        if (GUILayout.Button("Open MapEditorWindow"))
        {
            EditorWindow.GetWindow(typeof(MapEditorWidow));
        }
    }
}


public class MapEditorWidow : EditorWindow
{
    int rightSize = 10;
    Vector2 rightScrollPos = Vector2.zero;

    [MenuItem("Window/MapEditorWindow")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(MapEditorWidow));
    }

    void OnGUI()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            Debug.Log(go.name);
        }

        EditorGUILayout.BeginVertical(GUI.skin.box);
        {
            EditorGUILayout.PrefixLabel("MapSize");

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.IntField("X",rightSize);

                EditorGUILayout.IntField("Y", rightSize);
            }
            EditorGUILayout.EndHorizontal();

            //rightSize = EditorGUILayout.IntSlider("Size", rightSize, 10, 100, GUILayout.ExpandWidth(false));

            // 右側のスクロール
            rightScrollPos = EditorGUILayout.BeginScrollView(rightScrollPos, GUI.skin.box);
            {
                // スクロール範囲

                for (int y = 0; y < rightSize; y++)
                {
                    EditorGUILayout.BeginHorizontal(GUI.skin.box);
                    {
                        // ここの範囲は横並び

                        EditorGUILayout.PrefixLabel("Index " + y);

                        // 下に行くほどボタン数増やす
                        for (int i = 0; i < y + 1; i++)
                        {
                            // ボタン(横幅100px)
                            if (GUILayout.Button("Button" + i, GUILayout.Width(100)))
                            {
                                Debug.Log("Button" + i + "押したよ");
                            }
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                // こんな感じで横幅固定しなくても、範囲からはみ出すときにスクロールバー出してくれる。
            }
            EditorGUILayout.EndScrollView();
        }

        EditorGUILayout.EndVertical();

    }
}