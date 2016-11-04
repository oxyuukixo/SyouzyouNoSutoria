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
                cc.Move();  // 移動
                break;
            case 1:
                cc.Attack();// 攻撃
                break;
            case 2:
                cc.Magic(); // 魔法
                break;
            case 3:
                cc.Skil();  // スキル
                break;
            case 4:
                cc.Item();  // アイテム
                break;
            case 5:
                cc.Wait();  // 構え
                break;
            case 6:
                cc.End();   // ターンエンド
                break;
            case 7:
                cc.Psy();   // 彩術
                break;
        }
    }
}
