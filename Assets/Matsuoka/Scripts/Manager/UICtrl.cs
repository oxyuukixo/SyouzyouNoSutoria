using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICtrl : MonoBehaviour {

    public Image[] m_Chara;
    public GameObject[] m_Command;
    public Image[] m_Status;
    public Text m_Turn;
    public GameObject[] m_Cover;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < m_Chara.Length; i++)
        {
            m_Chara[i].enabled = false;
            m_Command[i].SetActive(false);
            m_Status[i].enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
