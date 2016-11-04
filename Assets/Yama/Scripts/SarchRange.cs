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
        Vector2 sarchPosition;
        sarchPosition = position;
        if (moveRange <= 0) return;
        for (SarchDirection i = 0; i < SarchDirection.number; i++)
        {
            switch(i)
            {
                case SarchDirection.top:
                    sarchPosition.x = position.x;
                    if ((sarchPosition.y = position.y + 1.0f) >= stage[0].Length) continue; ;
                    break;
                case SarchDirection.left:
                    if ((sarchPosition.x = position.x - 1.0f) < 0) continue; ;
                    sarchPosition.y = position.y;
                    break;
                case SarchDirection.bottom:
                    sarchPosition.x = position.x;
                    if ((sarchPosition.y = position.y - 1.0f) < 0) continue; ;
                    break;
                case SarchDirection.right:
                    if ((sarchPosition.x = position.x + 1.0f) >= stage.Length) continue; ;
                    sarchPosition.y = position.y;
                    break;
                default:
                    break;
            }
            if (stage[(int)sarchPosition.y][(int)sarchPosition.x] == null) continue;
            sarchRange[(int)sarchPosition.y][(int)sarchPosition.x] = true;
            Sarch(stage, sarchRange, sarchPosition, moveRange - 1);
        }
        return;
    }

    ////移動経路を決める
    //public static Vector2[] SarchMoveRoute(GameObject[][] stage, GameObject character, GameObject movePosition)
    //{
    //    Vector2[] moveRoute;

    //    return moveRoute;
    //}

    //最短の移動経路を出す
    //private static void SarchRoute(GameObject[][] stage, Vector2 character, Vector2 position, int moveRange, int moveRangeMin)
    //{

    //}
}
