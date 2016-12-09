using UnityEngine;
using System.Collections;
using System;

public class TurnController : MonoBehaviour {

    public static GameObject m_turnCharacter;    //次のターンのキャラクターを出す

    private static  Status[] m_character;        //キャラクターの情報保存
    private static int m_counter;                //キャラクターのカウンター

    //キャラクター情報を入力する
	public static void SetCharacter()
    {
        GameObject[] player;    //プレイヤー
        GameObject[] enemy;     //敵
        int characterNumber;    //キャラクター数
        player = GameObject.FindGameObjectsWithTag("Player");
        enemy = GameObject.FindGameObjectsWithTag("Enemy");
        characterNumber = player.Length + enemy.Length;
        m_character = new Status[characterNumber];
        for (int i = 0; i < player.Length; i++)
        {
            m_character[i] = player[i].GetComponent<Status>();
        }
        for (int i = 0; i < enemy.Length; i++)
        {
            m_character[i + player.Length] = enemy[i].GetComponent<Status>();
        }
        m_counter = 0;
        ReplactoSpeedOrder();
        m_turnCharacter = m_character[m_counter].gameObject;
        GameObject.FindWithTag("TargetCamera").GetComponent<CameraControl>().m_CenterObj = m_turnCharacter;
    }

    //速度順に降順で入れ替える
    private static void ReplactoSpeedOrder()
    {
        Array.Sort(m_character, (a, b) => a.TEC - b.TEC);
        Array.Reverse(m_character);
    }

    //次の移動のキャラクター
    public static void NextMoveCharacter()
    {
        if (++m_counter >= m_character.Length) m_counter = 0;
        m_turnCharacter = m_character[m_counter].gameObject;
        GameObject.FindWithTag("TargetCamera").GetComponent<CameraControl>().m_CenterObj = m_turnCharacter;
    }

    //移動できる順番
    public static string[] NextMoveCharacters(int number)
    {
        string[] name;      //名前
        int counter;        //カウンター
        name = new string[number];
        counter = m_counter;
        for (int i = 0; i < number; i++)
        {
            name[i] = m_character[counter].name;
            if (++counter >= m_character.Length) counter = 0;
        }
        return name;
    }
}
