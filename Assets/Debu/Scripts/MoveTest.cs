using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MoveTest : MonoBehaviour {

    public float m_AniSpeed = 1f;

    float m_CurrentTime;

    List<Sprite> m_ImageList = new List<Sprite>();

    int m_CurrentNum = 0;

    Image m_Image;

    bool IsEnd = false;

	// Use this for initialization
	void Start () {

        int currentNum = 0;

        while(true)
        {
            int ZeroNum = 4;

            for(int i = 1; i < 5;i++ )
            {
                if(currentNum / (int)Mathf.Pow(10,i) > 0)
                {
                    ZeroNum--;

                    continue;
                }

                break;
            }

            string FileNum = "TitleMovie/test_";

            for(int i = 0; i < ZeroNum;i++)
            {
                FileNum += 0;
            }

            FileNum += currentNum;

            Sprite sprite = Resources.Load<Sprite>(FileNum);

            if(sprite)
            {
                m_ImageList.Add(sprite);
                currentNum++;
                continue;
            }

            break;
        }

        m_Image = GetComponent<Image>();
        m_Image.sprite = m_ImageList[m_CurrentNum];
	}
	
	// Update is called once per frame
	void Update () {
	
        if((m_CurrentTime += Time.deltaTime) > 1f / m_AniSpeed && !IsEnd)
        {
            if((m_CurrentNum += 1) < m_ImageList.Count)
            {
                m_CurrentTime = 0;
                m_Image.sprite = m_ImageList[m_CurrentNum];
            }
            else
            {
                IsEnd = true;
            }
        }

	}
}
