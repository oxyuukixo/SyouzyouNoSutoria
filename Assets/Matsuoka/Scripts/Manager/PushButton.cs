using UnityEngine;
using System.Collections;

public class PushButton : MonoBehaviour {

    int m_Command;
    CharaControl[] cc;

	// Use this for initialization
	void Start () {
        GameObject[] player;
        player = GameObject.FindGameObjectsWithTag("Player");
        cc = new CharaControl[player.Length];
        for (int i = 0; i < player.Length; i++)
        {
            cc[i] = player[i].GetComponent<CharaControl>();
        }
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void ButtonPush()
    {
        CharaControl player;
        player = null;
        for (int i = 0; i < cc.Length; i++)
        {
            if (cc[i].gameObject != TurnController.m_turnCharacter) continue;
            player = cc[i];
            break;
        }
        if (player == null) return;
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
                player.Move();  // 移動
                break;
            case 1:
                player.Attack();// 攻撃
                break;
            case 2:
                player.Magic(); // 魔法
                break;
            case 3:
                player.Skil();  // スキル
                break;
            case 4:
                player.Item();  // アイテム
                break;
            case 5:
                player.Wait();  // 構え
                break;
            case 6:
                player.End();   // ターンエンド
                break;
            case 7:
                player.Psy();   // 彩術
                break;
        }
    }
}
