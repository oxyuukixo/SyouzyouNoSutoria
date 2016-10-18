using UnityEngine;
using System.Collections;

public class PushButton : MonoBehaviour {

    CharaControl cc;

	// Use this for initialization
	void Start () {
        cc = GameObject.Find("CharacterRobotBoy").GetComponent<CharaControl>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ButtonPush()
    {
        cc.CommandUIFalse(0);

        if (cc.transform.position.x < 9)
        {
            cc.transform.position += new Vector3(1, 0, 0);
        }
    }
}
