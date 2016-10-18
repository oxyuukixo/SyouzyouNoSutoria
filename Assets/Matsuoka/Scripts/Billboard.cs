using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {

    public Camera targetCamera;

    // Use this for initialization
    void Start () {
        if (this.targetCamera == null)
            targetCamera = Camera.main;
    }
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(this.targetCamera.transform.position);
    }
}
