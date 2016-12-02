using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

//魔法の名前
public enum MagicName
{
    fire,       //ファイアー
    water,      //ウォーター
    wind,       //ウィンド
    soil,       //ソイル
    number,     //要素数
}

public class MagicData : MonoBehaviour
{
    public static List<AttackData> m_magicData;     //魔法データ

    //魔法データの最初の行の内容
    private enum MagicDataFirstLine
    {
        property,           //属性
        power,              //攻撃力
    }

    //魔法の名前を返す
    private static string MagicNameString(MagicName magic)
    {
        switch(magic)
        {
            case MagicName.fire:
                return "Fire";
            case MagicName.water:
                return "Water";
            case MagicName.wind:
                return "Wind";
                case MagicName.soil:
                return "Soil";
            default:
                return  "";       
        }
    }

	//初期化
	void Start ()
    {
        m_magicData = new List<AttackData>();
        for (MagicName i = 0; i < MagicName.number; i++)
        {
            m_magicData.Add(new AttackData());
            ReadMagicDataCsv(i);
        }
    }

    //魔法データを入力する
    void ReadMagicDataCsv(MagicName magic)
    {
        List<List<bool>> attackArea;        //攻撃範囲
        TextAsset csv;                      //テキストアセット
        StringReader reader;                //文字列読み込みクラス
        int count;                          //カウンター
        string line;                        //読み込み文字列
        string[] values;                    //読み込み文字列の単語
        csv = Resources.Load("CSV/" + MagicNameString(magic)) as TextAsset;
        reader = new StringReader(csv.text);
        if (reader.Peek() == -1) return;
        //余分な文字列を排除する
        line = reader.ReadLine();
        //魔法の基礎データを入力
        line = reader.ReadLine();
        values = line.Split(',');
        m_magicData[(int)magic].m_attackType = AttackType.Magic;
        m_magicData[(int)magic].m_attackProperty = (AttackProperty)int.Parse(values[(int)MagicDataFirstLine.property]);
        m_magicData[(int)magic].m_power = int.Parse(values[(int)MagicDataFirstLine.power]);
        //余分な文字列を排除する
        line = reader.ReadLine();
        //魔法の攻撃範囲の情報を取得
        attackArea = new List<List<bool>>();
        count = 0;
        while (reader.Peek() > -1)
        {
            line = reader.ReadLine();
            values = line.Split(',');
            attackArea.Add(new List<bool>());
            for (int i = 0; i < values.Length; i++)
            {
                if (int.Parse(values[i]) == 0) attackArea[count].Add(false);
                else attackArea[count].Add(true);
            }
            count++;
        }
        //攻撃範囲を入力
        m_magicData[(int)magic].m_range = new bool[attackArea.Count][];
        for (int i = 0; i < attackArea.Count; i++)
        {
            m_magicData[(int)magic].m_range[i] = attackArea[i].ToArray();
        }
        m_magicData[(int)magic].m_centerXRange = Mathf.FloorToInt(m_magicData[(int)magic].m_range.Length / 2);
        m_magicData[(int)magic].m_centerYRange = Mathf.FloorToInt(m_magicData[(int)magic].m_range[0].Length / 2);
    }

    //攻撃範囲を表示する
    public static void MagicAttackRange(GameObject character, MagicName magic)
    {
        GameObject[][] stage;   //ステージ
        int characterX;         //ステージ上のプレイヤー座標X
        int characterY;         //ステージ上のキャラクター座標Y
        int stageX;             //調べているステージ座標X
        int stageY;             //調べているステージ座標Y
        MoveAreaType moveAreaType;
        stage = character.GetComponent<CharacterMove>().m_stage;
        characterX = Mathf.FloorToInt(character.transform.position.x);
        characterY = Mathf.FloorToInt(character.transform.position.z);
        moveAreaType = MoveAreaType.player;
        if (character.tag == "Player") moveAreaType = MoveAreaType.player;
        else if (character.tag == "Enemy") moveAreaType = MoveAreaType.enemy;
        for (int i = 0;i < m_magicData[(int)magic].m_range.Length; i++)
        {
            stageY = characterY + i - m_magicData[(int)magic].m_centerYRange;
            if (stageY < 0) continue;
            if (stageY >= stage.Length) break;
            for (int j = 0;j < m_magicData[(int)magic].m_range[i].Length; j++)
            {
                stageX = characterX + j - m_magicData[(int)magic].m_centerXRange;
                if (stageX < 0) continue;
                if (stageX >= stage.Length) break;
                if (stage[stageY][stageX] == null) continue;
                stage[stageY][stageX].GetComponent<StageInfo>().m_displayArea[(int)moveAreaType] = m_magicData[(int)magic].m_range[i][j];
            }
        }

    }

}
