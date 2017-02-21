using UnityEngine;
using System.Collections;

public class CharacterDirection : MonoBehaviour {

    public static string AnimationLayerName(Direction direction)
    {
        switch(direction)
        {
            case Direction.top:
                return "Base Layer.Top.";
            case Direction.left:
                return "Base Layer.Left.";
            case Direction.bottom:
                return "Base Layer.Bottom.";
            case Direction.right:
                return "Base Layer.Right.";
            default:
                return "";
        }
    }

    //位置関係でキャラクターの向きを返す
    public static Direction CharaDirection(GameObject my, GameObject other)
    {
        float x;
        float y;

        x = my.transform.position.x - other.transform.position.x;
        y = my.transform.position.y - other.transform.position.y;
        if (x < 0) return Direction.left;
        else if (x > 0) return Direction.right;
        else if (y < 0) return Direction.top;
        else return Direction.bottom;
    }

}
