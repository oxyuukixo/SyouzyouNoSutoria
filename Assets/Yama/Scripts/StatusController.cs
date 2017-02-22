﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class StatusController : MonoBehaviour {


    CameraControl m_CamCtrlClass;
    UICtrl m_UIClass;
    BattleMain m_BtlClass;
    GameObject turnPlayer;
    private int CharactorCount;

    // Use this for initialization
    void Start ()
    {
        m_CamCtrlClass = GameObject.Find("Camera").GetComponent<CameraControl>();
        m_UIClass = GameObject.Find("SceneManager/GameManager").GetComponent<UICtrl>();
        m_BtlClass = GameObject.Find("SceneManager/GameManager").GetComponent<BattleMain>();
        CharactorCount = 6;
    }

    // Update is called once per frame
    void Update ()
    {
        turnPlayer = TurnController.m_turnCharacter;
        if (turnPlayer == null) return;
        SelectCharacter();
    }

    public void SelectCharacter()
    {
        RaycastHit hit;  // 光線に当たったオブジェクトを受け取るクラス
        Ray ray;  // 光線クラス
        GameObject selectCharacter; //選択オブジェクト
        int turnPlayerState;    //ターンプレイヤーの状態
        int number; //オブジェクト番号
        // スクリーン座標に対してマウスの位置の光線を取得
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // マウスの光線の先にオブジェクトが存在していたら hit に入る
        if (Physics.Raycast(ray, out hit))
        {
            selectCharacter = hit.collider.gameObject;
            // 当たったオブジェクトを取得
            number = CharactorNumber(selectCharacter);
            switch (number)
            {
                case 0: // ステージ
                    if (turnPlayer.tag != "Player") return;
                        if (CrossPlatformInputManager.GetButtonUp("Fire1"))
                    {
                        m_CamCtrlClass.m_CenterObj = hit.collider.gameObject;
                        for (int i = 0; i < CharactorCount; i++)
                        {
                            turnPlayer.GetComponent<CharaControl>().CommandUIFalse(i);
                            m_UIClass.m_Cover[0].SetActive(false);
                        }
                    }
                    for (int i = 0; i < CharactorCount; i++)
                    {
                        if (!m_UIClass.m_Command[i].activeInHierarchy)
                        {
                            m_UIClass.m_Status[i].SetActive(false);
                        }
                    }
                    break;
                case 1: // レオ
                case 2: // マシラ
                case 3: // リュンヌ
                case 4: // 紅音
                case 5: // シエル
                case 6: // イーグニズル
                    m_UIClass.m_Status[number - 1].SetActive(true);
                    if (turnPlayer.tag != "Player") return;
                    if (selectCharacter != turnPlayer) return;
                    UIStatus(selectCharacter);
                    break;
                case -1:
                    break;
            }
        }
    }

    private void UIStatus(GameObject selectCharacter)
    {
        if (CrossPlatformInputManager.GetButtonUp("Fire1"))
        {
            m_CamCtrlClass.m_CenterObj = selectCharacter;
            for (int i = 0; i < CharactorCount; i++)
            {
                turnPlayer.GetComponent<CharaControl>().CommandUIFalse(i);
                if (!m_UIClass.m_Command[i].activeInHierarchy)
                {
                    m_UIClass.m_Status[i].SetActive(false);
                }
            }
            turnPlayer.GetComponent<CharaControl>().CommandUITrue(1);
            m_UIClass.m_Cover[0].SetActive(true);
        }
    }

    // 選択されたキャラクター
    public static int CharactorNumber(GameObject obj)
    {
        if (obj.tag == "Stage")
        {
            return (int)PlayerNumber.Default;
        }
        if (obj.name == "Leo")
        {
            return (int)PlayerNumber.Leo;
        }
        if (obj.name == "Mashira")
        {
            return (int)PlayerNumber.Mashira;
        }
        if (obj.name == "Ryunne")
        {
            return (int)PlayerNumber.Ryunne;
        }
        if (obj.name == "Akane")
        {
            return (int)PlayerNumber.Akane;
        }
        if (obj.name == "Ciel")
        {
            return (int)PlayerNumber.Ciel;
        }
        if (obj.name == "Ignizure")
        {
            return (int)PlayerNumber.Ignir;
        }
        if (obj.name == "Enemy1")
        {
            return (int)EnemyNumber.Enemy1;
        }
        if (obj.name == "Enemy2")
        {
            return (int)EnemyNumber.Enemy2;
        }
        if (obj.name == "Enemy3")
        {
            return (int)EnemyNumber.Enemy3;
        }
        return -1;
    }

    private void HP_MP()
    {
        for(int i= 0; i < 9; i++)
        {
            m_UIClass.m_HP[i].rectTransform.sizeDelta = new Vector2(100 / m_BtlClass.Player[i].GetComponent<Status>().HP, m_UIClass.m_HP[i].rectTransform.sizeDelta.y);
            m_UIClass.m_MP[i].rectTransform.sizeDelta = new Vector2(100 / m_BtlClass.Player[i].GetComponent<Status>().MP, m_UIClass.m_MP[i].rectTransform.sizeDelta.y);
        }
    }


}
