using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICtrl : MonoBehaviour {

    public Image[] m_Chara;
    public GameObject[] m_Command;
    public Image[] m_Status;
    public Text m_Turn;
    public GameObject[] m_Cover;
    public Text m_Start;
    public Text m_End;

    // Use this for initialization
    void Start () {
        for (int i = 0; i < m_Chara.Length; i++)
        {
            m_Chara[i].enabled = false;
            m_Command[i].SetActive(false);
            m_Status[i].enabled = false;
        }
        m_Cover[0].SetActive(false);
        m_Start.enabled = false;
        m_End.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        
	}
}
