using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

    public GameObject[][] m_test;
    public int lange;

	// Use this for initialization
	void Start () {

        m_test = new GameObject[5][];
        for (int i = 0; i < 5; i++)
        {
            m_test[i] = new GameObject[5];
            for (int j = 0; j < 5; j++)
            {
                m_test[i][j] = this.gameObject;
            }
        }
    }

    // Update is called once per frame
    void Update () {
        Test();
    }

    void Test()
    {
        bool[][] test;
        if (!Input.GetMouseButtonDown(0)) return;
        test = SarchRange.PermitSarchRange( m_test, this.gameObject, lange);
    }
}
