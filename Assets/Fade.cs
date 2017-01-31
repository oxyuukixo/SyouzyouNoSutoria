using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fade : MonoBehaviour {

    //フェードの速さ
    public float m_FadeSpeed = 0.3f;

    public bool m_IsAutoFadeIn;

    //フェードが完了したかどうかのフラグ
    [HideInInspector]
    public bool m_IsFadeFinish = false;

    //フェードインするかのフラグ
    [HideInInspector]
    public bool m_IsFadeIn = true;

    //フェードするかどうかのフラグ
    private bool m_IsFade = false;

    private Image m_Image;

	// Use this for initialization
	void Start () {

        m_Image = GetComponent<Image>();

        if(m_IsAutoFadeIn)
        {
            FadeIn();
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
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

    public void FadeIn(float FadeSpeed = -0.1f)
    {
        m_IsFade = true;
        m_IsFadeIn = true;
        m_IsFadeFinish = false;

        if(FadeSpeed > 0)
        {
            m_FadeSpeed = FadeSpeed;
        }
    }

    public void FadeOut(float FadeSpeed = -0.1f)
    {
        m_IsFade = true;
        m_IsFadeIn = false;
        m_IsFadeFinish = false;

        if (FadeSpeed > 0)
        {
            m_FadeSpeed = FadeSpeed;
        }
    }
}
