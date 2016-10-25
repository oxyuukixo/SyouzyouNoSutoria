using UnityEngine;
using System.Collections;

public class PushButton : MonoBehaviour {

    int m_Command;
    CharaControl cc;

	// Use this for initialization
	void Start () {
        cc = GameObject.Find("Leo").GetComponent<CharaControl>();
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void ButtonPush()
    {
        if (gameObject.name == "Move")
        {
            m_Command = 0;
        }
        if (gameObject.name == "Attack")
        {
            m_Command = 1;
        }
        if (gameObject.name == "Magic")
        {
            m_Command = 2;
        }
        if (gameObject.name == "Skil")
        {
            m_Command = 3;
        }
        if (gameObject.name == "Item")
        {
            m_Command = 4;
        }
        if (gameObject.name == "Wait")
        {
            m_Command = 5;
        }
        if (gameObject.name == "End")
        {
            m_Command = 6;
        }
        if(gameObject.name == "Psy")
        {
            m_Command = 7;
        }
        switch (m_Command)
        {
            case 0:
                cc.Move();
                break;
            case 1:
                cc.Attack();
                break;
            case 2:
                cc.Magic();
                break;
            case 3:
                cc.Skil();
                break;
            case 4:
                cc.Item();
                break;
            case 5:
                cc.Wait();
                break;
            case 6:
                cc.End();
                break;
            case 7:
                cc.Psy();
                break;
        }
    }
}
