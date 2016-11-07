using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICtrl : MonoBehaviour {

    public Image[] m_Chara;
    public GameObject[] m_Command;
    public GameObject[] m_Cover;
    public Image[] m_Status;
    public Text m_Turn;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < m_Chara.Length; i++)
        {
            m_Chara[i].enabled = false;
            m_Command[i].SetActive(false);
            m_Status[i].enabled = false;
        }
        for(int i = 0; i < m_Cover.Length; i++)
        {
            m_Cover[i].SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
