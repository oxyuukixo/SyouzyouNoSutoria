using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

//物理攻撃の名前
public enum PhysicalName
{
    normal,     //通常攻撃
    number,     //要素数
}

//魔法の名前
public enum MagicName
{
    fire,       //ファイアー
    water,      //ウォーター
    wind,       //ウィンド
    soil,       //ソイル
    number,     //要素数
}

public class AttackDataList : MonoBehaviour {

    //探索方向
    private enum SarchDirection
    {
        top,        //上
        left,       //左
        bottom,     //下
        right,      //右
        number,     //要素数
    }

    public static List<AttackData> m_physicalData;  //物理攻撃データ
    public static List<AttackData> m_magicData;     //魔法攻撃データ


    //物理攻撃の名前を返す
    private static string PhysicalNameString(PhysicalName physical)
    {
        switch (physical)
        {
            case PhysicalName.normal:
                return "Normal";
            default:
                return "";
        }
    }

    //魔法攻撃の名前を返す
    private static string MagicNameString(MagicName magic)
    {
        switch (magic)
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
                return "";
        }
    }

    //初期化
    void Start()
    {
        m_physicalData = new List<AttackData>();
        m_magicData = new List<AttackData>();
        for (PhysicalName i = 0; i < PhysicalName.number; i++)
        {
            m_physicalData.Add(new AttackData());
            ReadPhysicalDataCsv(i);
        }
        for (MagicName i = 0; i < MagicName.number; i++)
        {
            m_magicData.Add(new AttackData());
            ReadMagicDataCsv(i);
        }
    }

    //物理攻撃データを入力する
    void ReadPhysicalDataCsv(PhysicalName physical)
    {
        List<List<bool>> attackArea;        //攻撃範囲
        TextAsset csv;                      //テキストアセット
        StringReader reader;                //文字列読み込みクラス
        string line;                        //読み込み文字列
        string[] values;                    //読み込み文字列の単語
        int count;                          //凡庸カウンター
        csv = Resources.Load("CSV/Physical/" + PhysicalNameString(physical)) as TextAsset;
        reader = new StringReader(csv.text);
        if (reader.Peek() == -1) return;
        //余分な文字列を排除する
        line = reader.ReadLine();
        //魔法の基礎データを入力
        line = reader.ReadLine();
        values = line.Split(',');
        m_physicalData[(int)physical].m_attackHeight = int.Parse(values[0]);
        //余分な文字列を排除する
        line = reader.ReadLine();
        //物理の攻撃範囲の情報を取得
        count = 0;
        attackArea = new List<List<bool>>();
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
        m_physicalData[(int)physical].m_range = new bool[attackArea.Count][];
        for (int i = 0; i < attackArea.Count; i++)
        {
            m_physicalData[(int)physical].m_range[i] = attackArea[i].ToArray();
        }
        m_physicalData[(int)physical].m_centerXRange = Mathf.FloorToInt(m_physicalData[(int)physical].m_range.Length / 2);
        m_physicalData[(int)physical].m_centerYRange = Mathf.FloorToInt(m_physicalData[(int)physical].m_range[0].Length / 2);
    }

    //魔法攻撃データを入力する
    void ReadMagicDataCsv(MagicName magic)
    {
        List<List<bool>> attackArea;        //攻撃範囲
        TextAsset csv;                      //テキストアセット
        StringReader reader;                //文字列読み込みクラス
        string line;                        //読み込み文字列
        string[] values;                    //読み込み文字列の単語
        int count;                          //凡庸カウンター
        csv = Resources.Load("CSV/Magic/" + MagicNameString(magic)) as TextAsset;
        reader = new StringReader(csv.text);
        if (reader.Peek() == -1) return;
        //余分な文字列を排除する
        line = reader.ReadLine();
        //魔法の基礎データを入力
        line = reader.ReadLine();
        values = line.Split(',');
        m_magicData[(int)magic].m_attackType = AttackType.Magic;
        m_magicData[(int)magic].m_attackProperty = (AttackProperty)int.Parse(values[0]);
        m_magicData[(int)magic].m_attackHeight = int.Parse(values[1]);
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

    //物理攻撃範囲を表示する
    public static void PhysicalAttackRange(GameObject character, PhysicalName physical)
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
        ActiveAttackArea(character);
        for (int i = 0; i < m_physicalData[(int)physical].m_range.Length; i++)
        {
            stageY = characterY + i - m_physicalData[(int)physical].m_centerYRange;
            if (stageY < 0) continue;
            if (stageY >= stage.Length) break;
            for (int j = 0; j < m_physicalData[(int)physical].m_range[i].Length; j++)
            {
                stageX = characterX + j - m_physicalData[(int)physical].m_centerXRange;
                if (stageX < 0) continue;
                if (stageX >= stage.Length) break;
                if (stage[stageY][stageX] == null) continue;
                stage[stageY][stageX].GetComponent<StageInfo>().m_displayArea[(int)moveAreaType] = m_physicalData[(int)physical].m_range[i][j];
            }
        }
        AttackAreaLimit(stage, moveAreaType, SarchDirection.number, characterX, characterY,
            stage[characterY][characterX].GetComponent<StageInfo>().height, m_physicalData[(int)physical].m_attackHeight,
            m_physicalData[(int)physical].m_centerXRange);
    }

    //魔法攻撃範囲を表示する
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
        ActiveAttackArea(character);
        for (int i = 0; i < m_magicData[(int)magic].m_range.Length; i++)
        {
            stageY = characterY + i - m_magicData[(int)magic].m_centerYRange;
            if (stageY < 0) continue;
            if (stageY >= stage.Length) break;
            for (int j = 0; j < m_magicData[(int)magic].m_range[i].Length; j++)
            {
                stageX = characterX + j - m_magicData[(int)magic].m_centerXRange;
                if (stageX < 0) continue;
                if (stageX >= stage.Length) break;
                if (stage[stageY][stageX] == null) continue;
                stage[stageY][stageX].GetComponent<StageInfo>().m_displayArea[(int)moveAreaType] = m_magicData[(int)magic].m_range[i][j];
            }
        }
        AttackAreaLimit(stage, moveAreaType, SarchDirection.number, characterX, characterY,
            stage[characterY][characterX].GetComponent<StageInfo>().height, m_magicData[(int)magic].m_attackHeight,
            m_magicData[(int)magic].m_centerXRange);
    }

    //物理で攻撃する
    public static void MagicAttack(GameObject myCharacter, GameObject otherCharacter, AttackProperty attackProperty)
    {
        Status status;
        int damage;
        status = otherCharacter.GetComponent<Status>();
        damage = DamageCalculations.Damege(myCharacter, otherCharacter,
            GameLevel.levelEasy, AttackType.Physical, attackProperty);
        status.HP -= damage;
        if (status.HP < 0)
        {
            status.HP = 0;
            Destroy(otherCharacter);
        }
    }

    //魔法で攻撃する
    public static void MagicAttack(GameObject myCharacter, GameObject otherCharacter, MagicName magic)
    {
        Status status;
        int damage;
        status = otherCharacter.GetComponent<Status>();
        damage = DamageCalculations.Damege(myCharacter, otherCharacter,
            GameLevel.levelEasy, m_magicData[(int)magic].m_attackType, m_magicData[(int)magic].m_attackProperty);
        status.HP -= damage;
        if (status.HP < 0)
        {
            status.HP = 0;
            Destroy(otherCharacter);
        }
    }

    //攻撃エリアの表示準備
    private static void ActiveAttackArea(GameObject character)
    {
        GameObject[][] stage;   //ステージ
        stage = character.GetComponent<CharacterMove>().m_stage;
        if (stage == null) return;
        for (int i = 0; i < stage.Length; i++)
        {
            for (int j = 0; j < stage[i].Length; j++)
            {
                for (int k = 0; k < (int)MoveAreaType.number; k++)
                {
                    stage[i][j].GetComponent<StageInfo>().m_displayArea[k] = false;
                    stage[i][j].GetComponent<StageInfo>().m_moveAriaActive[k] = true;
                }
            }
        }
    }

    //攻撃エリアの非表示にする
    public static void HideAttackArea(GameObject character)
    {
        GameObject[][] stage;   //ステージ
        stage = character.GetComponent<CharacterMove>().m_stage;
        if (stage == null) return;
        for (int i = 0; i < stage.Length; i++)
        {
            for (int j = 0; j < stage[i].Length; j++)
            {
                for (int k = 0; k < (int)MoveAreaType.number; k++)
                {
                    stage[i][j].GetComponent<StageInfo>().m_displayArea[k] = false;
                    stage[i][j].GetComponent<StageInfo>().m_moveAriaActive[k] = false;
                }
            }
        }

    }

    //攻撃可能エリアの限定
    private static void AttackAreaLimit(GameObject[][] stage, MoveAreaType moveAreaType, SarchDirection oldSarch,
        int x, int y, int height, int attackHeight, int count)
    {
        StageInfo stageInfo;        //ステージ情報
        StageInfo newStageInfo;     //新しいステージ情報
        int stageX;
        int stageY;
        if (count == 0) return;
        stageInfo = stage[y][x].GetComponentInChildren<StageInfo>();
        for (SarchDirection i = 0; i < SarchDirection.number; i++)
        {
            if (i == oldSarch) continue;
            stageX = x;
            stageY = y;
            switch(i)
            {
                case SarchDirection.top:
                    stageY++;
                    break;
                case SarchDirection.left:
                    stageX--;
                    break;
                case SarchDirection.bottom:
                    stageY--;
                    break;
                case SarchDirection.right:
                    stageX++;
                    break;
            }
            if (stageX < 0 || stageX > stage[0].Length) continue;
            if (stageY < 0 || stageY > stage.Length) continue;
            newStageInfo = stage[stageY][stageX].GetComponent<StageInfo>();
            if (!newStageInfo.m_displayArea[(int)moveAreaType]) continue;
            if(Mathf.Abs(newStageInfo.height - height) > attackHeight)
            {
                newStageInfo.m_displayArea[(int)moveAreaType] = false;
                continue;
            }
            AttackAreaLimit(stage, moveAreaType, i, stageX, stageY, height, attackHeight, count - 1);
        }
    }
}
