using UnityEngine;
using System.Collections;

public class SkipButton : MonoBehaviour {

    public GameObject m_TextManager;

    public void OnClick()
    {
        m_TextManager.GetComponent<ConversationControll>().m_Skip = !m_TextManager.GetComponent<ConversationControll>().m_Skip;
    }
}
