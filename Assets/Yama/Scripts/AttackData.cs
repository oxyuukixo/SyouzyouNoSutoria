using UnityEngine;
using System.Collections;

public class AttackData : MonoBehaviour {

    public AttackType m_attackType;          //攻撃の種類
    public AttackProperty m_attackProperty;  //攻撃の属性
    public int m_power;                      //攻撃力
    public int m_centerXRange;               //攻撃範囲の横方向の端から中心点までの距離
    public int m_centerYRange;               //攻撃範囲の縦方向の端から中心点までの距離
    public bool[][] m_range;                 //攻撃範囲
}
