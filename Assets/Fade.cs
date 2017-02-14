using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fade : MonoBehaviour {

    //フェードの速さ
    public float m_FadeSpeed = 0.3f;

<<<<<<< HEAD
    public bool m_IsAutoFadeIn;
=======
    public bool m_autofadein = false;
>>>>>>> origin/development

    //フェードが完了したかどうかのフラグ
    [HideInInspector]
    public bool m_IsFadeFinish = false;

<<<<<<< HEAD
=======
    //フェードするかどうかのフラグ
    private bool m_IsFade = false;

>>>>>>> origin/development
    //フェードインするかのフラグ
    [HideInInspector]
    public bool m_IsFadeIn = true;

<<<<<<< HEAD
    //フェードするかどうかのフラグ
    private bool m_IsFade = false;

=======
>>>>>>> origin/development
    private Image m_Image;

	// Use this for initialization
	void Start () {

        m_Image = GetComponent<Image>();

<<<<<<< HEAD
        if(m_IsAutoFadeIn)
=======
        if (m_autofadein)
>>>>>>> origin/development
        {
            FadeIn();
        }
	}
	
	// Update is called once per frame
<<<<<<< HEAD
	void FixedUpdate () {
=======
	void Update () {
>>>>>>> origin/development
	
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

<<<<<<< HEAD
    public void FadeIn(float FadeSpeed = -0.1f)
=======
    public void FadeIn()
>>>>>>> origin/development
    {
        m_IsFade = true;
        m_IsFadeIn = true;
        m_IsFadeFinish = false;
<<<<<<< HEAD

        if(FadeSpeed > 0)
        {
            m_FadeSpeed = FadeSpeed;
        }
    }

    public void FadeOut(float FadeSpeed = -0.1f)
=======
    }

    public void FadeOut()
>>>>>>> origin/development
    {
        m_IsFade = true;
        m_IsFadeIn = false;
        m_IsFadeFinish = false;
<<<<<<< HEAD

        if (FadeSpeed > 0)
        {
            m_FadeSpeed = FadeSpeed;
        }
=======
>>>>>>> origin/development
    }
}
