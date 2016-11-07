using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof(MapManager))]
public class MapEditor : Editor
{
    [MenuItem("MyTools/Test Selected Object")]
    public override void OnInspectorGUI()
    {
        //元のInspector部分を表示
        base.OnInspectorGUI();

        MapManager m_MapManager = target as MapManager;

        //ボタンを表示
        if (GUILayout.Button("Open MapEditorWindow"))
        {
            EditorWindow.GetWindow(typeof(MapEditorWidow));

            MapEditorWidow.m_MapManager = m_MapManager;
        }
    }
}


public class MapEditorWidow : EditorWindow
{
    //編集するマップのマネージャー
    public static MapManager m_MapManager;

    //スクロールの座標
    Vector2 ScrollPos = Vector2.zero;

    //現在選択されているステージオブジェクト
    private StageInfo SelectObject;

    float IntervalX = 1;

    float IntervalY = 1;

    float ObjectHeight = 1;

    [MenuItem("Window/MapEditorWindow")]
    public static void ShowWindow()
    {
        //ウィンドウの作成
        EditorWindow.GetWindow(typeof(MapEditorWidow));
    }

    void OnGUI()
    {
        if (m_MapManager)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                //マップ全体のオプションの表示
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    EditorGUILayout.PrefixLabel("MapSize");

                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUI.BeginChangeCheck();

                        int TempX = EditorGUILayout.IntField("X", m_MapManager.m_MapX);

                        int TempY = EditorGUILayout.IntField("Y", m_MapManager.m_MapY);

                        //1000個以上になっていたら変更しない
                        if (TempX * TempY < 1000)
                        {
                            m_MapManager.m_MapX = TempX;

                            m_MapManager.m_MapY = TempY;
                        }

                        //オブジェクトの数が変わっていたら変更する
                        if (EditorGUI.EndChangeCheck())
                        {
                            CreateMap();
                        }

                    }
                    EditorGUILayout.EndHorizontal();

                    ObjectHeight = EditorGUILayout.FloatField("HeightInterval", ObjectHeight);

                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUI.BeginChangeCheck();

                        float TempX = EditorGUILayout.FloatField("IntervalX", IntervalX);

                        float TempY = EditorGUILayout.FloatField("IntervalY", IntervalY);

                        //間隔がマイナスになっていたら変更しない
                        if (TempX > 0 && TempY > 0)
                        {
                            IntervalX = TempX;

                            IntervalY = TempY;
                        }

                        //オブジェクトの数が変わっていたら変更する
                        if (EditorGUI.EndChangeCheck())
                        {
                            SetMapPossion();
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    //ボタンを表示
                    if (GUILayout.Button("Clear"))
                    {
                        //オブジェクトをすべて削除
                        for (int i = m_MapManager.transform.childCount - 1; i >= 0; --i)
                        {
                            GameObject.DestroyImmediate(m_MapManager.transform.GetChild(i).gameObject);
                        }

                        m_MapManager.m_MapList.Clear();

                        m_MapManager.m_MapX = 0;
                        m_MapManager.m_MapY = 0;
                        IntervalX = 1;
                        IntervalY = 1;
                    }
                }
                EditorGUILayout.EndVertical();

                //ステージを選択するボタンの表示
                if (m_MapManager.m_MapX * m_MapManager.m_MapY > 0)
                {
                    ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos, GUI.skin.box);
                    {
                        // スクロール範囲
                        for (int y = 0; y < m_MapManager.m_MapY; y++)
                        {
                            EditorGUILayout.BeginHorizontal(GUI.skin.box);
                            {
                                for (int x = 0; x < m_MapManager.m_MapX; x++)
                                {
                                    if (GUILayout.Button("[" + x + "][" + y + "]", GUILayout.Width(75)))
                                    {
                                        SelectObject = m_MapManager.m_MapList[y][x].GetComponent<StageInfo>();

                                        List<Object> objects = new List<Object>();

                                        objects.Add(SelectObject.gameObject);

                                        Selection.objects = objects.ToArray();
                                    }
                                }
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                    EditorGUILayout.EndScrollView();

                    //オブジェクトが選択されていたら
                    if (SelectObject != null)
                    {
                        EditorGUILayout.BeginVertical(GUI.skin.box);
                        {
                            EditorGUI.BeginChangeCheck();

                            EditorGUILayout.LabelField("Stage[" + SelectObject.position[0] + "][" + SelectObject.position[1] + "]");

                            SelectObject.height = EditorGUILayout.IntField("Height", SelectObject.height);

                            SelectObject.stageLook = (GameObject)EditorGUILayout.ObjectField("MeshObj", SelectObject.stageLook, typeof(GameObject), true);

                            if (EditorGUI.EndChangeCheck())
                            {
                                ChangeStage();
                            }
                        }
                        EditorGUILayout.EndVertical();
                    }
                }
                EditorGUILayout.EndVertical();
            }
        }
    }

    //オブジェクトが選択されたときの関数
    void OnSelectionChange()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            if (go.GetComponentInParent(typeof(MapManager)) != null)
            {
                SelectObject = go.GetComponent<StageInfo>();

                FocusWindowIfItsOpen(typeof(MapEditorWidow));

                break;
            }
        }
    }

    void CreateMap()
    {
        if (m_MapManager.m_MapList.Count > 0)
        {
            if (m_MapManager.m_MapList[0].Count < m_MapManager.m_MapX)
            {
                for (int y = 0; y < m_MapManager.m_MapList.Count; y++)
                {
                    for (int x = m_MapManager.m_MapList[y].Count; x < m_MapManager.m_MapX; x++)
                    {
                        //オブジェクトを生成
                        GameObject RespawnObject = Instantiate(m_MapManager.m_StageObject);

                        m_MapManager.m_MapList[y].Add(RespawnObject);

                        RespawnObject.AddComponent<StageInfo>();

                        RespawnObject.GetComponent<StageInfo>().stageLook = m_MapManager.m_StageObject;

                        RespawnObject.GetComponent<StageInfo>().height = 1;

                        //子オブジェクトにする
                        RespawnObject.transform.parent = m_MapManager.transform;

                        //生成時パラメーターの設定
                        RespawnObject.GetComponent<StageInfo>().position[0] = x;
                        RespawnObject.GetComponent<StageInfo>().position[1] = y;
                        RespawnObject.transform.position = m_MapManager.gameObject.transform.position + new Vector3(x * IntervalX, 0, y * IntervalY);
                    }
                }
            }
            else
            {
                for (int y = 0; y < m_MapManager.m_MapList.Count; y++)
                {
                    for (int x = m_MapManager.m_MapList[y].Count - 1; x >= m_MapManager.m_MapX; x--)
                    {
                        GameObject.DestroyImmediate(m_MapManager.m_MapList[y][x]);

                        m_MapManager.m_MapList[y].Remove(m_MapManager.m_MapList[y][x]);
                    }
                }
            }
        }


        if (m_MapManager.m_MapList.Count < m_MapManager.m_MapY)
        {
            for (int y = m_MapManager.m_MapList.Count; y < m_MapManager.m_MapY; y++)
            {
                m_MapManager.m_MapList.Add(new List<GameObject>());

                for (int x = 0; x < m_MapManager.m_MapX; x++)
                {
                    //オブジェクトを生成
                    GameObject RespawnObject = Instantiate(m_MapManager.m_StageObject);

                    m_MapManager.m_MapList[y].Add(RespawnObject);

                    RespawnObject.AddComponent<StageInfo>();

                    RespawnObject.GetComponent<StageInfo>().stageLook = m_MapManager.m_StageObject;

                    RespawnObject.GetComponent<StageInfo>().height = 1;

                    //子オブジェクトにする
                    RespawnObject.transform.parent = m_MapManager.transform;

                    //生成時パラメーターの設定
                    RespawnObject.GetComponent<StageInfo>().position[0] = x;

                    RespawnObject.GetComponent<StageInfo>().position[1] = y;

                    RespawnObject.transform.position = m_MapManager.gameObject.transform.position + new Vector3(x * IntervalX, 0, y * IntervalY);
                }
            }
        }
        else
        {
            for (int y = m_MapManager.m_MapList.Count - 1; y >= m_MapManager.m_MapY; y--)
            {
                for (int x = m_MapManager.m_MapList[y].Count - 1; x >= 0; x--)
                {
                    GameObject.DestroyImmediate(m_MapManager.m_MapList[y][x]);

                    m_MapManager.m_MapList[y].Remove(m_MapManager.m_MapList[y][x]);
                }

                m_MapManager.m_MapList.Remove(m_MapManager.m_MapList[y]);
            }
        }
    }

    void ChangeStage()
    {
        //オブジェクトを生成
        GameObject RespawnObject = Instantiate(SelectObject.stageLook);

        m_MapManager.m_MapList[SelectObject.position[1]][SelectObject.position[0]] = RespawnObject;

        //子オブジェクトにする
        RespawnObject.transform.parent = m_MapManager.transform;

        RespawnObject.transform.position = m_MapManager.gameObject.transform.position + new Vector3(SelectObject.position[0] * IntervalX, (SelectObject.height -1) * ObjectHeight, SelectObject.position[1] * IntervalY);

        RespawnObject.AddComponent<StageInfo>();

        //生成時パラメーターの設定
        RespawnObject.GetComponent<StageInfo>().position = SelectObject.position;

        RespawnObject.GetComponent<StageInfo>().height = SelectObject.height;

        RespawnObject.GetComponent<StageInfo>().possible = SelectObject.possible;

        RespawnObject.GetComponent<StageInfo>().charaCategory = SelectObject.charaCategory;

        RespawnObject.GetComponent<StageInfo>().stageLook = SelectObject.stageLook;

        GameObject ChildObject;

        for (int HeightNum = 0; HeightNum < SelectObject.height; HeightNum++)
        {
            //オブジェクトを生成
            ChildObject = Instantiate(SelectObject.stageLook);

            //子オブジェクトにする
            ChildObject.transform.parent = RespawnObject.transform;

            ChildObject.transform.position = m_MapManager.gameObject.transform.position + new Vector3(SelectObject.position[0] * IntervalX, RespawnObject.transform.position.y - ObjectHeight * HeightNum, SelectObject.position[1] * IntervalY);
        }

        GameObject.DestroyImmediate(SelectObject.gameObject);

        SelectObject = RespawnObject.GetComponent<StageInfo>();
    }

    //マップの間隔によるオブジェクトの生成
    void SetMapPossion()
    {
        for (int y = 0; y < m_MapManager.m_MapY; y++)
        {
            for (int x = 0;x < m_MapManager.m_MapX;x++)
            {
                m_MapManager.m_MapList[y][x].transform.position = m_MapManager.gameObject.transform.position + new Vector3(x * IntervalX, 0, y * IntervalY);
            }
        }
    }
}