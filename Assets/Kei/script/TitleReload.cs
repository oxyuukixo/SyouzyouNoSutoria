using UnityEngine;
using System.Collections;

public class TitleReload : MonoBehaviour {

    void Start()
    {
        Invoke("Reload", 18.0f);
    }

    // Update is called once per frame
    void Update()
    {
    }
    void Reload()
    {
        FadeManager.Instance.LoadLevel("Title", 1.0f);
    }
}
