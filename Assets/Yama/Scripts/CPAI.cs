using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CPAI : MonoBehaviour {

    //AIの状態
    private enum CPAIType
    {
        sarchMove,  //移動ルートを決める
        move,       //移動する
        attack,     //攻撃
        turnEnd,    //ターン終了
    }

    public GameObject m_target;                     //狙う敵
    public Vector2 m_movePosition;                  //移動する座標

    private List<GameObject> m_targetCandidates;    //狙う敵の候補
    private GameObject[][] m_stage;                 //ステージ情報
    private Status m_myStatus;                      //自分のステータス
<<<<<<< HEAD
    private StageInfo m_onStage;                    //自分が乗っているステージ
=======
>>>>>>> origin/development
    private CPAIType m_aiType;                      //AIの状態

    //開始時に初期化
    private void Start ()
    {
        MapManager map;
        m_myStatus = GetComponent<Status>();
        map = GameObject.Find("Stage").GetComponent<MapManager>();

        m_stage = new GameObject[map.m_MapList.Count][];
        for (int i = 0; i < map.m_MapList.Count; i++)
        {
            m_stage[i] = map.m_MapList[i].ToArray();
        }
<<<<<<< HEAD
        m_onStage = m_stage[Mathf.FloorToInt(transform.position.z)][Mathf.FloorToInt(transform.position.x)].GetComponent<StageInfo>();
        m_onStage.possible = true;
=======
>>>>>>> origin/development
    }

    //CPUのAI
    public void AI()
    {
        switch(m_aiType)
        {
            case CPAIType.sarchMove:
                SarchMove();
                break;
            case CPAIType.move:
                gameObject.GetComponent<CharacterMove>().Move();
                if (!gameObject.GetComponent<CharacterMove>().m_move)
                {
                    if (m_target == null) m_aiType = CPAIType.turnEnd;
                    else m_aiType = CPAIType.attack;
                }
                break;
            case CPAIType.attack:
                Attack();
                break;
            case CPAIType.turnEnd:
                TurnEnd();
                break;
        }
    }

    //移動ルートを決める
    private void SarchMove()
    {
        m_target = null;
        m_targetCandidates = null;
        m_targetCandidates = new List<GameObject>();
        SarchTarget();
        CheckCanMove();
        if (m_target != null) SarchMovePoint();
        else NearMovePlayer();
    }

    //狙う敵の候補を決める
    private void SarchTarget()
    {
        GameObject[] player;        //プレイヤーのオブジェクト
        int playerLength;           //プレイヤーとの距離
        player = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < player.Length; i++)
        {
            playerLength = (int)Mathf.Abs((player[i].transform.position.x - transform.position.x
                + player[i].transform.position.z - transform.position.z));
            if (playerLength - 1 > m_myStatus.MOV) continue;
            m_targetCandidates.Add(player[i]);
        }
    }

    //移動できるかを調べる
    private void CheckCanMove()
    {
        Vector2[] moveRoute;
        if (m_targetCandidates.Count == 0) return;
        for (int i = m_targetCandidates.Count - 1; i >= 0; i--)
        {
<<<<<<< HEAD
            m_targetCandidates[i].GetComponent<CharaControl>().ChangeOnStagePossible(false);
            moveRoute = SarchRange.SarchMoveRoute(m_stage,
                gameObject, m_targetCandidates[i], m_myStatus.MOV + 1);
            m_targetCandidates[i].GetComponent<CharaControl>().ChangeOnStagePossible(true);
=======
            moveRoute = SarchRange.SarchMoveRoute(m_stage,
                gameObject, m_targetCandidates[i], m_myStatus.MOV + 1);
>>>>>>> origin/development
            if (moveRoute == null) m_targetCandidates.Remove(m_targetCandidates[i]);
        }
        if (m_targetCandidates.Count == 0) return;
        //体力の少ない順に並び替える
        m_targetCandidates.Sort((a, b) => a.GetComponent<Status>().HP - b.GetComponent<Status>().HP);
        //ターゲットを決定
        m_target = m_targetCandidates[0];

    }

    //移動ルートを決める
    private void SarchMovePoint()
    {
        Status targetStatus;        //ターゲットのステータス
        GameObject target;          //移動地点
        Vector2[] moveRoute;         //移動ルート
        Direction targetDirection;  //ターゲットとの向き
        int count;                  //カウンター
        if (m_targetCandidates.Count == 0) return;
        targetStatus = m_target.GetComponent<Status>();
        count = 0;
        do
        {
            target = new GameObject();
            target.transform.position = m_target.transform.position;
            targetDirection = targetStatus.DIRECTION;
            switch (count)
            {
                case 1:
                    targetDirection++;
                    if (targetDirection > Direction.right) targetDirection = Direction.top;
                    break;
                case 2:
                    targetDirection--;
                    if (targetDirection < Direction.top) targetDirection = Direction.right;
                    break;
                case 3:
                    targetDirection += 2;
                    if (targetDirection > Direction.right) targetDirection -= Direction.right;
                    break;
            }
            switch(targetDirection)
            {
                case Direction.top:
                    target.transform.position += new Vector3(0.0f, 0.0f, 1.0f);
                    break;
                case Direction.left:
                    target.transform.position += new Vector3(-1.0f, 0.0f, 0.0f);
                    break;
                case Direction.bottom:
                    target.transform.position += new Vector3(0.0f, 0.0f, -1.0f);
                    break;
                case Direction.right:
                    target.transform.position += new Vector3(1.0f, 0.0f, 0.0f);
                    break;
            }

            moveRoute = SarchRange.SarchMoveRoute(m_stage,
                gameObject, target, m_myStatus.MOV);

            if (moveRoute != null) break;
            count++;
        } while (count < (int)Direction.right + 1);
        if (moveRoute == null) return;
        gameObject.GetComponent<CharacterMove>().m_moveRoute = moveRoute;
        gameObject.GetComponent<CharacterMove>().m_move = true;
        m_aiType = CPAIType.move;
    }

    //プレイヤーの近くに移動
    private void NearMovePlayer()
    {
        List<Vector2> moveRoute;    //移動ルート
        GameObject[] player;        //プレイヤーのオブジェクト
        GameObject target;          //移動場所
        int playerLength;           //プレイヤーとの距離
        int moveLength;             //移動距離
        int moveNumber;             //移動可能距離
        moveLength = -1;
        target = null;
        player = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < player.Length; i++)
        {
            playerLength = (int)Mathf.Abs((player[i].transform.position.x - transform.position.x
                + player[i].transform.position.z - transform.position.z));
            if (playerLength > moveLength && moveLength != -1) continue;
            target = player[i];
            moveLength = playerLength;
        }
        moveRoute = new List<Vector2>(SarchRange.SarchMoveRoute(m_stage,
            gameObject, target, moveLength * 2));
        if (moveRoute == null) return;
        moveNumber = target.GetComponent<Status>().MOV;
        moveRoute.RemoveRange(moveNumber, moveRoute.Count - moveNumber);
<<<<<<< HEAD
=======

>>>>>>> origin/development
        gameObject.GetComponent<CharacterMove>().m_moveRoute = moveRoute.ToArray();
        gameObject.GetComponent<CharacterMove>().m_move = true;
        m_aiType = CPAIType.move;
    }

    //AIの攻撃
    private void Attack()
    {
        Status status;   //ステータス
        int damage;      //ダメージ
        status = m_target.GetComponent<Status>();
        damage = DamageCalculations.Damege(gameObject, m_target,
            GameLevel.levelEasy, AttackType.Physical, AttackProperty.NoPropertyAttack);
        status.HP -= damage;
        if (status.HP < 0)
        {
            status.HP = 0;
            Destroy(m_target.gameObject);
        }
        m_aiType = CPAIType.turnEnd;
    }

    //ターンエンド
    private void TurnEnd()
    {
<<<<<<< HEAD
        m_onStage.possible = false;
        m_onStage = m_stage[Mathf.FloorToInt(transform.position.z)][Mathf.FloorToInt(transform.position.x)].GetComponent<StageInfo>();
        m_onStage.possible = true;
=======
>>>>>>> origin/development
        m_aiType = CPAIType.sarchMove;
        TurnController.NextMoveCharacter();
    }

}
