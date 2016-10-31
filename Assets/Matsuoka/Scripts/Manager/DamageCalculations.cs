using UnityEngine;
using System.Collections;


//攻撃タイプ
public enum AttackType : int
{
    Physical,       //物理
    Magic,          //魔法
};

//攻撃属性
public enum AttackProperty : int
{
    FireAttack,         //火属性
    WaterAttack,        //水属性
    WindAttack,         //風属性
    SoilAttack,         //土属性
    NoPropertyAttack,   //無属性
};

//ゲーム難易度
public enum GameLevel : int
{
    levelEasy,      //イージー
    levelNormal,    //ノーマル
    levelHard,      //ハード
};

//攻撃ヒット面
public enum HitDirection
{
    back = 0,     //背面
    side1,        //側面1
    front,        //正面
    side2,        //側面2
}

public class DamageCalculations : MonoBehaviour
{
    private const float m_damegeRate = 0.2f;


    //ダメージ
    public static int Damege(GameObject me, GameObject other, GameLevel level, AttackType attackType, AttackProperty attackProperty)
    {
        Status myStatus = null;        //自分のステータス
        Status otherStatus = null;     //相手のステータス
        int damege;             //ダメージ
        myStatus = me.GetComponent<Status>();
        otherStatus = other.GetComponent<Status>();
        damege = 0;
        damege += RateCalculation(myStatus, otherStatus,level, attackProperty);
        switch(attackType)
        {
            case AttackType.Physical:
                damege += PhysicalDamege(myStatus, otherStatus);
                break;
            case AttackType.Magic:
                damege += MagicDamege(myStatus, otherStatus);
                break;
            default:
                break;
        }

        return damege;
    }

    //物理ダメージ
    private static int PhysicalDamege(Status myStatus, Status otherStatus)
    {
        return (int)((myStatus.AT + myStatus.TEC - otherStatus.DF) * m_damegeRate);
    }

    //魔法ダメージ
    private static int MagicDamege(Status myStatus, Status otherStatus)
    {
        return (int)((myStatus.MAT + myStatus.TEC - otherStatus.MDF) * m_damegeRate);
    }

    //レート計算
    private static int RateCalculation(Status myStatus, Status otherStatus,GameLevel level, AttackProperty attackProperty)
    {
        return (int)((20 + CorrectionCalculation(myStatus, otherStatus, level)) * PropertyRate());
    }

    //補正計算
    private static int CorrectionCalculation(Status myStatus, Status otherStatus, GameLevel level)
    {
        return HeightCorrection(myStatus, otherStatus) + DirectionCorrection(myStatus, otherStatus) + CriticalDamage(myStatus, otherStatus, level);
    }

    //高さ補正
    private static int HeightCorrection(Status myStatus, Status otherStatus)
    {
        int damage;     //ダメージ
        switch (myStatus.HEIGHT - otherStatus.HEIGHT)
        {
            case 1:
                damage = 10;
                break;
            case 2:
                damage = 20;
                break;
            case 3:
                damage = 30;
                break;
            default:
                damage = 0;
                break;
        }
        return damage;
    }

    //向き補正
    private static int DirectionCorrection(Status myStatus, Status otherStatus)
    {
        int damage;     //ダメージ
        switch (Mathf.Abs((int)myStatus.DIRECTION - (int)otherStatus.DIRECTION))
        {
            case (int)HitDirection.back:
                damage = 20;
                break;
            case (int)HitDirection.side1:
            case (int)HitDirection.side2:
                damage = 10;
                break;
            default:
                damage = 0;
                break;

        }
        return damage;
    }

    //クリティカルダメージ
    private static int CriticalDamage(Status myStatus, Status otherStatus, GameLevel level)
    {
        float critical;                 //クリティカル発生率
        critical = 0.0f;
        switch (myStatus.HEIGHT - otherStatus.HEIGHT)
        {
            case 1:
                critical += 0.06f;
                break;
            case 2:
                critical += 0.04f;
                break;
            case 3:
                critical += 0.02f;
                break;
            default:
                critical += 0.08f;
                break;
        }
        switch(Mathf.Abs((int)myStatus.DIRECTION - (int)otherStatus.DIRECTION))
        {
            case (int)HitDirection.back:
                critical += 0.06f;
                break;
            case (int)HitDirection.side1:
            case (int)HitDirection.side2:
                critical += 0.04f;
                break;
            case (int)HitDirection.front:
                critical += 0.02f;
                break;
            default:
                break;

        }
        ;
        //距離補正値
        critical *= (float)(myStatus.TEC);
        switch (level)
        {
            case GameLevel.levelEasy:
                if (myStatus.gameObject.tag == "Player") critical *= 1.5f;
                else if (myStatus.gameObject.tag == "Enemy") critical *= 0.8f;
                break;
            case GameLevel.levelNormal:
                if (myStatus.gameObject.tag == "Player") critical *= 1.0f;
                else if (myStatus.gameObject.tag == "Enemy") critical *= 1.0f;
                break;
            case GameLevel.levelHard:
                if (myStatus.gameObject.tag == "Player") critical *= 0.8f;
                else if (myStatus.gameObject.tag == "Enemy") critical *= 1.5f;
                break;
        }
        if (critical < Random.Range(0.0f, 100.0f)) return 20;
        return 0;
    }

    //属性倍率
    private static float PropertyRate()
    {
        float rate;
        rate = 1.0f;
        return rate;
    }
}
