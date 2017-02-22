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

    private const float m_attackWaitTime = 0.3f;    //攻撃の最低保証時間

    public GameObject m_target;                     //狙う敵
    public Vector2 m_movePosition;                  //移動する座標
    public Animator m_anime;                        //アニメーション
    public PhysicalName m_physicalName;            //攻撃名
    public MagicName m_magicName;                  //魔法攻撃名
    public AudioController m_audio;

    private List<GameObject> m_targetCandidates;    //狙う敵の候補
    private GameObject[][] m_stage;                 //ステージ情報
    private GameObject m_effect;
    private Status m_myStatus;                      //自分のステータス
    private StageInfo m_onStage;                    //自分が乗っているステージ
    private CPAIType m_aiType;                      //AIの状態
    private AttackType m_attackType;
    private AttackProperty m_attackProperty;
    private int m_spendMP;
    private float m_attackTime;

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
        m_onStage = m_stage[Mathf.FloorToInt(transform.position.z)][Mathf.FloorToInt(transform.position.x)].GetComponent<StageInfo>();
        m_onStage.possible = true;
        m_onStage.charaCategory = gameObject;
        m_anime = GetComponent<Animator>();
        m_audio = GetComponent<AudioController>();
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
                    //m_anime.SetBool("walk", false);
                    if (m_target == null)
                    {
                        m_aiType = CPAIType.turnEnd;
                    }
                    else
                    {
                        m_aiType = CPAIType.attack;
                    //    m_anime.SetTrigger("attack");
                        if (m_attackType == AttackType.Physical)
                        {
                            m_audio.m_SEType = SoundController.SEType.attack;
                            m_effect = CreateEffect.EffectCreate(m_target, EffectType.attack);
                        }
                        else
                        {
                            switch (m_magicName)
                            {
                                case MagicName.fire:
                                    m_audio.m_SEType = SoundController.SEType.fire;
                                    m_effect = CreateEffect.EffectCreate(m_target, EffectType.fire);
                                    break;
                                case MagicName.water:
                                    m_audio.m_SEType = SoundController.SEType.water;
                                    m_effect = CreateEffect.EffectCreate(m_target, EffectType.water);
                                    break;
                                case MagicName.wind:
                                    m_audio.m_SEType = SoundController.SEType.wind;
                                    m_effect = CreateEffect.EffectCreate(m_target, EffectType.wind);
                                    break;
                                case MagicName.soil:
                                    m_audio.m_SEType = SoundController.SEType.soil;
                                    m_effect = CreateEffect.EffectCreate(m_target, EffectType.soil);
                                    break;
                            }
                        }
                        m_audio.ChangeSound();
                        m_audio.m_audioSource.Play();
                    }
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
        if (m_target != null)
        {
            SelectAttackType();
            SarchMovePoint();
        }
        else NearMovePlayer();
    //    m_anime.SetBool("walk", true);
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
            m_targetCandidates[i].GetComponent<CharaControl>().ChangeOnStagePossible(false);
            moveRoute = SarchRange.SarchMoveRoute(m_stage,
                gameObject, m_targetCandidates[i], m_myStatus.MOV + 1, true);
            m_targetCandidates[i].GetComponent<CharaControl>().ChangeOnStagePossible(true);
            if (moveRoute == null) m_targetCandidates.Remove(m_targetCandidates[i]);
        }
        if (m_targetCandidates.Count == 0) return;
        //体力の少ない順に並び替える
        m_targetCandidates.Sort((a, b) => a.GetComponent<Status>().HP - b.GetComponent<Status>().HP);
        //ターゲットを決定
        m_target = m_targetCandidates[0];

    }

    //攻撃の種類を決定する
    private void SelectAttackType()
    {
        Status otherStatus;     //相手のステータス
        int physicalRate;       //物理攻撃価値
        int magicRate;          //魔法攻撃価値
        otherStatus = m_target.GetComponent<Status>();
        m_aiType = CPAIType.attack;

        //魔法を打てるか調べる
        if (m_magicName == MagicName.number)
        {
            m_attackType = AttackType.Physical;
            m_attackProperty = AttackDataList.m_physicalData[(int)m_physicalName].m_attackProperty;
            m_spendMP = AttackDataList.m_physicalData[(int)m_physicalName].m_spendMP;
            return;
        }
        else if(AttackDataList.m_magicData[(int)m_magicName].m_spendMP < m_myStatus.MP)
        {
            m_attackType = AttackType.Physical;
            m_attackProperty = AttackDataList.m_physicalData[(int)m_physicalName].m_attackProperty;
            m_spendMP = AttackDataList.m_physicalData[(int)m_physicalName].m_spendMP;
            return;
        }
        physicalRate = m_myStatus.AT - otherStatus.DF;
        magicRate = m_myStatus.MAT - otherStatus.MDF;
        //効果的な攻撃を選ぶ
        if(physicalRate > magicRate)
        {
            m_attackType = AttackType.Physical;
            m_attackProperty = AttackDataList.m_physicalData[(int)m_physicalName].m_attackProperty;
            m_spendMP = AttackDataList.m_physicalData[(int)m_physicalName].m_spendMP;
        }
        else
        {
            m_attackType = AttackType.Magic;
            m_attackProperty = AttackDataList.m_magicData[(int)m_magicName].m_attackProperty;
            m_spendMP = AttackDataList.m_magicData[(int)m_magicName].m_spendMP;
        }
    }

    //移動ルートを決める
    private void SarchMovePoint()
    {
        Status targetStatus;        //ターゲットのステータス
        GameObject target;          //移動地点
        Vector2[] moveRoute;         //移動ルート
        Direction targetDirection;  //ターゲットとの向き
        int count;                  //カウンター
        int attackLenge;            //攻撃距離
        if (m_targetCandidates.Count == 0) return;
        targetStatus = m_target.GetComponent<Status>();
        count = 0;
        attackLenge = 0;
        moveRoute = null;
        switch(m_attackType)
        {
            case AttackType.Physical:
                attackLenge = AttackDataList.m_physicalData[(int)m_physicalName].m_centerXRange;
                break;
            case AttackType.Magic:
                attackLenge = AttackDataList.m_magicData[(int)m_magicName].m_centerXRange;
                break;
        }
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
            for (int i = 0; i < attackLenge; i++)
            {
                Vector2[] damyRoute;
                switch (targetDirection)
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
                damyRoute = SarchRange.SarchMoveRoute(m_stage, gameObject, target, m_myStatus.MOV, false);
                if(damyRoute != null)moveRoute = damyRoute;
            }
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
            playerLength = (int)Mathf.Abs(player[i].transform.position.x - transform.position.x) +
                (int)Mathf.Abs(player[i].transform.position.z - transform.position.z);
            if (playerLength > moveLength && moveLength != -1) continue;
            target = player[i];
            moveLength = playerLength;
        }

        moveRoute = new List<Vector2>(SarchRange.SarchMoveRoute(m_stage,
            gameObject, target, moveLength, true));
        if (moveRoute.Count == 0)
        {
            moveRoute = new List<Vector2>(SarchRange.NearCharacterMove(m_stage,
            gameObject, target, moveLength));
            if (moveRoute.Count == 0) return;
        }
        moveNumber = target.GetComponent<Status>().MOV;
        moveRoute.RemoveRange(moveNumber, moveRoute.Count - moveNumber);
        gameObject.GetComponent<CharacterMove>().m_moveRoute = moveRoute.ToArray();
        gameObject.GetComponent<CharacterMove>().m_move = true;
        m_aiType = CPAIType.move;
    }

    //AIの攻撃
    private void Attack()
    {
        Status status;   //ステータス
        AnimatorStateInfo anime;    //アニメーターの状態
        int damage;      //ダメージ

        anime = m_anime.GetCurrentAnimatorStateInfo(0);
        if (m_effect != null) return;
        if ((m_attackTime += Time.deltaTime) < m_attackWaitTime) return;
        if (anime.fullPathHash != Animator.StringToHash(CharacterDirection.AnimationLayerName(m_myStatus.DIRECTION) + "wait")) return;


        status = m_target.GetComponent<Status>();
        damage = DamageCalculations.Damege(gameObject, m_target,
            GameLevel.levelEasy, m_attackType, m_attackProperty);
        status.HP -= damage;
        status.DIRECTION = CharacterDirection.CharaDirection(m_target, gameObject);
        if (status.HP < 0)
        {
            status.HP = 0;
            m_target.GetComponent<CharaControl>().m_audio.m_SEType = SoundController.SEType.dead;
            m_target.GetComponent<CharaControl>().m_audio.ChangeSound();
            m_target.GetComponent<CharaControl>().m_audio.m_audioSource.Play();
            m_effect = CreateEffect.EffectCreate(m_target, EffectType.dead);
            Destroy(m_target.gameObject);
        }
        else
        {
            m_target.GetComponent<CharaControl>().m_anime.SetTrigger("damage");

        }

        m_myStatus.MP -= m_spendMP;
        m_aiType = CPAIType.turnEnd;
    }

    //ターンエンド
    private void TurnEnd()
    {
        m_onStage.possible = false;
        m_onStage.charaCategory = null;
        m_onStage = m_stage[Mathf.FloorToInt(transform.position.z)][Mathf.FloorToInt(transform.position.x)].GetComponent<StageInfo>();
        m_onStage.possible = true;
        m_onStage.charaCategory = gameObject;
        m_aiType = CPAIType.sarchMove;
        TurnController.NextMoveCharacter();
    }

}
