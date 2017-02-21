using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EffectType
{
    dead,
    fire,
    soil,
    water,
    wind,
    attack,
    critical,
    guard,
    recover,
    saijutu,
}

public class CreateEffect : MonoBehaviour {

    public static List<GameObject> m_effect;

    void Awake()
    {
        string[][] fileData;
        fileData = TextSystems.ReadText("Effect");
        m_effect = new List<GameObject>();
        for (int i = 0; i < fileData.Length; i++)
        {
            for (int j = 0; j < fileData[i].Length; j++)
            {
                if (fileData[i][j] == "") continue;
                m_effect.Add(Resources.Load("Effect/" + fileData[i][j]) as GameObject);
            }
        }

    }

    public static GameObject EffectCreate(GameObject point, EffectType effect)
    {
        return Instantiate(m_effect[(int)effect], point.transform.position, point.transform.rotation) as GameObject;
    }
}
