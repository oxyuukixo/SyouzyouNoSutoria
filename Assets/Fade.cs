using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fade : MonoBehaviour {

    //フェードの速さ
    public float m_FadeSpeed = 0.3f;

    //フェードが完了したかどうかのフラグ
    [HideInInspector]
    public bool m_IsFadeFinish = false;

    //フェードするかどうかのフラグ
    private bool m_IsFade = false;

    //フェードインするかのフラグ
    private bool m_IsFadeIn = true;

    private Image m_Image;

	// Use this for initialization
	void Start () {

        m_Image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
	
        if(m_IsFade)
        {
            if(m_IsFadeIn)
            {
                m_Image.color -= new Color(0, 0, 0, m_FadeSpeed);

                if(m_Image.color.a <= 0)
                {
                    m_Image.color = new Color(0, 0, 0, 0);

                    m_IsFade = false;
                    m_IsFadeFinish = true;
                }
            }
            else
            {
                m_Image.color += new Color(0, 0, 0, m_FadeSpeed);

                if(m_Image.color.a >= 1)
                {
                    m_Image.color = new Color(0, 0, 0, 1);

                    m_IsFade = false;
                    m_IsFadeFinish = true;
                }

            }
        }

	}

    public void FadeIn()
    {
        m_IsFade = true;
        m_IsFadeIn = true;
        m_IsFadeFinish = false;
    }

    public void FadeOut()
    {
        m_IsFade = true;
        m_IsFadeIn = false;
        m_IsFadeFinish = false;
    }
}
