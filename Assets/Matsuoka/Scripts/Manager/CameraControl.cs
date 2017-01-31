using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

public class CameraControl : MonoBehaviour {

    public GameObject m_CenterObj;  // カメラの中心のオブジェクト

    private Camera m_Camera;
    private Vector3 m_Offset;   //カメラの距離 
    private float wheel;        // ホイールの回転量

    // Use this for initialization
    void Start () {
        m_Camera = transform.FindChild("Main Camera").GetComponent<Camera>();
        m_Offset = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = m_CenterObj.transform.position + m_Offset;

        if(wheel > 0)
        {
            if (m_Camera.orthographicSize > 2)
            {
                m_Camera.orthographicSize--;
            }
        }
        else if(wheel < 0)
        {
            if (m_Camera.orthographicSize < 10)
            {
                m_Camera.orthographicSize++;
            }
        }

        InputWheel();
    }

    void InputWheel()
    {
        wheel = CrossPlatformInputManager.GetAxis("Mouse ScrollWheel");
    }
}
