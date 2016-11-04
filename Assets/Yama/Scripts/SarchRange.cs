using UnityEngine;
using System.Collections;

public class SarchRange : MonoBehaviour {

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
        bool[][] sarchRange;        //探索範囲
        Vector2 position;           //探索地点
        position = new Vector2(character.transform.position.x, character.transform.position.z);
        sarchRange = new bool[stage.Length][];
        for (int i = 0; i < stage.Length; i++) 
        {
            sarchRange[i] = new bool[stage[i].Length];
            for (int j = 0; j < stage[0].Length; j++) 
            {
                sarchRange[i][j] = false;
            }
        }
        sarchRange[(int)position.y][(int)position.x] = true;

        Sarch(stage, sarchRange, position, moveRange);

        return sarchRange;
    }

    //探索をする
    private static void Sarch(GameObject[][] stage, bool[][] sarchRange, Vector2 position, int moveRange)
    {
        Vector2 sarchPosition;      //探索座標
        sarchPosition = position;
        if (moveRange <= 0) return;
        for (SarchDirection i = SarchDirection.top; i < SarchDirection.number; i++)
        {
            if (!SarchPosition(stage, sarchPosition, position, i)) continue;
            sarchRange[(int)sarchPosition.y][(int)sarchPosition.x] = true;

            Sarch(stage, sarchRange, sarchPosition, moveRange - 1);
        }
        return;
    }

    //移動経路を決める
    public static Vector2[] SarchMoveRoute(GameObject[][] stage, GameObject character, GameObject movePosition, int moveRange)
    {
        Vector2[] moveRoute;            //移動経路
        Vector2[] moveRouteDammy;       //移動経路のダミー
        Vector2 characterPosition;      //キャラクター座標
        Vector2 position;               //移動先の座標
        int moveRangeMin;
        moveRoute = new Vector2[moveRange];
        moveRouteDammy = new Vector2[moveRange];
        characterPosition = new Vector2(character.transform.position.x, character.transform.position.z);
        position = new Vector2(movePosition.transform.position.x, movePosition.transform.position.z);
        moveRangeMin = moveRange;

        SarchRoute(stage, characterPosition, position, moveRoute, moveRouteDammy, moveRange, moveRangeMin, moveRange);

        return moveRoute;
    }

    //最短の移動経路を出す
    private static void SarchRoute(GameObject[][] stage, Vector2 characterPosition, Vector2 position, Vector2[] moveRoute,
    Vector2[] moveRouteDammy, int moveRange, int moveRangeMin, int moveRageMax)
    {
        Vector2 sarchPosition;      //探索座標
        sarchPosition = characterPosition;
        //キャラクター座標と移動先の座標が一致していたら
        if(characterPosition == position)
        {
            //移動量の最小値を移動回数が超えたら何もしない
            if (moveRangeMin > moveRageMax - moveRange) return;
            moveRangeMin = moveRageMax - moveRange;
            moveRoute = moveRouteDammy;
            return;
        }
        //移動先の座標とキャラクター座標の差が移動量を超えたら何もしない
        if (Mathf.Abs((int)(characterPosition.x - position.x) + Mathf.Abs(characterPosition.y - position.y)) > moveRange) return;
        //移動量の最小値を移動回数が超えたら何もしない
        if (moveRangeMin > moveRageMax - moveRange) return;
        //移動量がなくなったら何もしない
        if (moveRange <= 0) return;
        for (SarchDirection i = SarchDirection.top; i < SarchDirection.number; i++)
        {
            if (!SarchPosition(stage, sarchPosition, position, i)) continue;
            moveRouteDammy[moveRageMax - moveRange] = sarchPosition;

            SarchRoute(stage, sarchPosition, position, moveRoute, moveRouteDammy, moveRange, moveRangeMin - 1, moveRageMax);
        }
        return;
    }

    //探索位置の条件を照合
    private static bool SarchPosition(GameObject[][] stage, Vector2 sarchPosition, Vector2 position, SarchDirection sarch)
    {
        int height;     //移動前と移動後の位置の高さの差
        switch (sarch)
        {
            case SarchDirection.top:
                sarchPosition.x = position.x;
                if ((sarchPosition.y = position.y + 1.0f) >= stage[0].Length) return false;
                break;
            case SarchDirection.left:
                if ((sarchPosition.x = position.x - 1.0f) < 0) return false;
                sarchPosition.y = position.y;
                break;
            case SarchDirection.bottom:
                sarchPosition.x = position.x;
                if ((sarchPosition.y = position.y - 1.0f) < 0) return false;
                break;
            case SarchDirection.right:
                if ((sarchPosition.x = position.x + 1.0f) >= stage.Length) return false;
                sarchPosition.y = position.y;
                break;
            default:
                break;
        }
        height = Mathf.Abs(stage[(int)sarchPosition.y][(int)sarchPosition.x].GetComponent<StageInfo>().height
            - stage[(int)position.y][(int)position.x].GetComponent<StageInfo>().height);
        if (stage[(int)sarchPosition.y][(int)sarchPosition.x] == null) return false;
        if (height != 0 && height != 1) return false;
        return true;
    }
}
