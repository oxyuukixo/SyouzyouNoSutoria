using UnityEngine;
using System.Collections;

public class ChoiceButton : MonoBehaviour {

    //ボタンが押されたかのフラグ
    [HideInInspector]
    public bool m_IsPush = false;

    //ボタンが押された時に読み込むテキスト
    [HideInInspector]
    public string m_ReadText;

    public void ButtonPush()
    {
        m_IsPush = true;
    }
}
