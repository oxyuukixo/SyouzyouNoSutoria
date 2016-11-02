using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;

public class ConversationControll : MonoBehaviour {

    //読み込むテキストのパス(シーン移動する前に設定)
    [HideInInspector]
    public static string m_TextPath = "Test.txt";

    private string m_TextDate;

    private string[] m_DisplayText;

    private FileInfo m_FileInfo;

    // Use this for initialization
    void Start () {
        m_FileInfo = new FileInfo(Application.dataPath + "/Text/" + m_TextPath);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
