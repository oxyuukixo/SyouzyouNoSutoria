using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SarchRange : MonoBehaviour {

    private static bool[][] m_sarchRange;   //探索範囲
    private static Vector2[] m_moveRoute;   //移動ルート
    private static int m_notTurnCount;      //道を曲がらなかった回数
    private static float m_moveMin;         //最小値
    private static bool m_reachMovePoint;   //移動地点に到達したか

    //探索方向
    private enum SarchDirection
    {
        top,        //上
        left,       //左
        bottom,     //下
        right,      //右
        number,     //要素数
    }

    //探索範囲の移動可能範囲を出す
    public static bool[][] PermitSarchRange(GameObject[][] stage, GameObject character, int moveRange)
    {
       
        Vector2 position;           //探索地点
        position = new Vector2(character.transform.position.x, character.transform.position.z);
        m_sarchRange = null;
        m_sarchRange = new bool[stage.Length][];
        for (int i = 0; i < stage.Length; i++) 
        {
            m_sarchRange[i] = new bool[stage[i].Length];
            for (int j = 0; j < stage[0].Length; j++) 
            {
                m_sarchRange[i][j] = false;
            }
        }
        m_sarchRange[(int)position.y][(int)position.x] = true;
        Sarch(stage, position, moveRange);
        return m_sarchRange;
    }

    //探索をする
    private static void Sarch(GameObject[][] stage, Vector2 position, int moveRange)
    {
        Vector2 sarchPosition;      //探索座標
        if (moveRange <= 0) return;
        for (SarchDirection i = SarchDirection.top; i < SarchDirection.number; i++)
        {
            sarchPosition = SarchPosition(stage, position, i);
            if (stage[(int)sarchPosition.y][(int)sarchPosition.x].GetComponent<StageInfo>().charaCategory != null) continue;
            if (sarchPosition == position) continue;
            m_sarchRange[Mathf.FloorToInt(sarchPosition.y)][Mathf.FloorToInt(sarchPosition.x)] = true;
            Sarch(stage, sarchPosition, moveRange - 1);
        }
        return;
    }

    //移動経路を決める
    public static Vector2[] SarchMoveRoute(GameObject[][] stage, GameObject character, GameObject movePosition, int moveRange)
    {
        List<Vector2> moveRouteDammy;   //移動経路のダミー
        Vector2 characterPosition;      //キャラクター座標
        Vector2 position;               //移動先の座標
        string enemyTag;                //敵のタグ
        m_moveRoute = null;
        m_reachMovePoint = false;
        moveRouteDammy = new List<Vector2>();
        characterPosition = new Vector2(character.transform.position.x, character.transform.position.z);
        if (movePosition.tag == "Stage")
            position = new Vector2(movePosition.transform.position.x + 0.5f, movePosition.transform.position.z + 0.5f);
        else
            position = new Vector2(movePosition.transform.position.x, movePosition.transform.position.z);
        if (character.tag == "Player") enemyTag = "Enemy";
        else enemyTag = "Player";
        m_moveMin = moveRange;
        m_notTurnCount = 0;
        SarchRoute(stage, characterPosition, position, moveRouteDammy, SarchDirection.top, m_notTurnCount, moveRange, moveRange, enemyTag);
        if(!m_reachMovePoint) m_moveRoute = null;
        return m_moveRoute;
    }

    //最短の移動経路を出す
    private static void SarchRoute(GameObject[][] stage, Vector2 characterPosition, Vector2 position,
    List<Vector2> moveRouteDammy, SarchDirection oldDirection,int notTurnCount, int moveRange, int moveRageMax, string enemyTag)
    {
        Vector2 sarchPosition;      //探索座標
        SarchDirection direction;   //向き
        int count;                  //凡庸カウンター
        int notTurn;                //曲がらなかった回数
        direction = oldDirection;
        count = 0;
        //キャラクター座標と移動先の座標が一致していたら
        if(characterPosition == position)
        {
            if (stage[(int)characterPosition.y][(int)characterPosition.x].GetComponent<StageInfo>().charaCategory != null) return;
            //移動量の最小値を移動回数が超えたら何もしない
            if (m_moveMin < moveRageMax - moveRange) return;
            if (m_moveMin == moveRageMax - moveRange && m_notTurnCount > notTurnCount) return;
            m_reachMovePoint = true;
            m_moveMin = moveRageMax - moveRange;
            m_notTurnCount = notTurnCount;
            m_moveRoute = moveRouteDammy.ToArray();
            return;
        }
        //移動先の座標とキャラクター座標の差が移動量を超えたら何もしない
        if (Mathf.Abs((int)(characterPosition.x - position.x) + Mathf.Abs(characterPosition.y - position.y)) > moveRange) return;
        //移動量の最小値を移動回数が超えたら何もしない
        if (m_moveMin < moveRageMax - moveRange) return;
        //移動量がなくなったら何もしない
        if (moveRange <= 0) return;
        //移動方向を決める
        while (count < (int)SarchDirection.number)
        {
            notTurn = notTurnCount;
            sarchPosition = SarchPosition(stage, characterPosition, direction);
            if (sarchPosition == characterPosition)
            {
                if (++direction == SarchDirection.number) direction = SarchDirection.top;
                count++;
                continue;
            }
            if (direction == oldDirection || notTurnCount == 0)
            {
                ++notTurn;
            }
            moveRouteDammy.Add(sarchPosition);
            SarchRoute(stage, sarchPosition, position, moveRouteDammy, direction, notTurn, moveRange - 1, moveRageMax, enemyTag);
            moveRouteDammy.RemoveAt(moveRouteDammy.Count - 1);
            if (++direction == SarchDirection.number) direction = SarchDirection.top;
            count++;
        }
        return;
    }

    //探索位置の条件を照合
    private static Vector2 SarchPosition(GameObject[][] stage, Vector2 characterPosition, SarchDirection sarch)
    {
        Vector2 sarchPosition;  //探索ポジション
        int height;             //移動前と移動後の位置の高さの差
        if (characterPosition == null) return characterPosition;
        sarchPosition = characterPosition;
        switch (sarch)
        {
            case SarchDirection.top:
                if ((sarchPosition.y += 1.0f) >= stage.Length) return characterPosition;
                break;
            case SarchDirection.left:
                if ((sarchPosition.x -= 1.0f) < 0) return characterPosition;
                break;
            case SarchDirection.bottom:
                if ((sarchPosition.y -= 1.0f) < 0) return characterPosition;
                break;
            case SarchDirection.right:
                if ((sarchPosition.x += 1.0f) >= stage[0].Length) return characterPosition;
                break;
            default:
                return characterPosition;
        }
        //高さの差の絶対値を出す
        height = Mathf.Abs(stage[(int)sarchPosition.y][(int)sarchPosition.x].GetComponent<StageInfo>().height
            - stage[(int)characterPosition.y][(int)characterPosition.x].GetComponent<StageInfo>().height);
        if (stage[(int)sarchPosition.y][(int)sarchPosition.x] == null) return characterPosition;
        if (stage[(int)sarchPosition.y][(int)sarchPosition.x].GetComponent<StageInfo>().possible) return characterPosition;
        if (stage[(int)sarchPosition.y][(int)sarchPosition.x].tag != "Stage") return characterPosition;
        if (height != 0 && height != 1) return characterPosition;
        return sarchPosition;
    }
}
