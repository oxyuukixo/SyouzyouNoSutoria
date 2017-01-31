using UnityEngine;
using System.Collections;

//移動可能範囲の種類
public enum MoveAreaType
{
    player,     //プレイヤー
    enemy,      //敵
    number,     //要素数
}

public class MoveArea : MonoBehaviour {

    private static GameObject[][] m_stage;     //ステージデータ

    //移動可能範囲を探索する
    public static void MoveAreaSarch(GameObject character)
    {
        bool[][] moveArea;
        MoveAreaType moveAreaType;
        if (m_stage == null) m_stage = character.GetComponent<CharacterMove>().m_stage;
        moveAreaType = MoveAreaType.player;
        if (character.tag == "Player") moveAreaType = MoveAreaType.player;
        else if (character.tag == "Enemy") moveAreaType = MoveAreaType.enemy;
        moveArea = SarchRange.PermitSarchRange(m_stage, character, character.GetComponent<Status>().MOV);
        if (moveArea == null) return;
        for (int i = 0; i < m_stage.Length; i++)
        {
            for (int j = 0; j < m_stage[i].Length; j++)
            {
                m_stage[i][j].GetComponent<StageInfo>().m_displayArea[(int)moveAreaType] = moveArea[i][j];
                m_stage[i][j].GetComponent<StageInfo>().m_moveAriaActive[(int)moveAreaType] = true;
            }
        }
    }

    //移動可能範囲をリセットする
    public static void ResetMoveArea()
    {
        if (m_stage == null) return;
        for (int i = 0; i < m_stage.Length; i++)
        {
            for (int j = 0; j < m_stage[i].Length; j++)
            {
                for (int k = 0; k < (int)MoveAreaType.number; k++)
                {
                    m_stage[i][j].GetComponent<StageInfo>().m_displayArea[k] = false;
                    m_stage[i][j].GetComponent<StageInfo>().m_moveAriaActive[k] = false;

                }
            }
        }
    }

    //アセット内の名前を返す
    public static string MoveAreaAssetsName(MoveAreaType moveAreaType)
    {
        string name;
        switch(moveAreaType)
        {
            case MoveAreaType.player:
                name = "Prefab/PlayerMoveArea";
                break;
            case MoveAreaType.enemy:
                name = "Prefab/EnemyMoveArea";
                break;
            default:
                name = "";
                break;
        }
        return name;
    }

    //名前からクローンを取り除く
    public static string RemoveNameClone(string name)
    {
        return name.Replace("(Clone)", "");
    }
}
