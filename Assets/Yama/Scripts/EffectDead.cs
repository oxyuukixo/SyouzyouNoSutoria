using UnityEngine;
using System.Collections;

public class EffectDead : MonoBehaviour {

    private Animator m_anime;

	// Use this for initialization
	void Start ()
    {
        m_anime = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        AnimatorStateInfo anime;    //アニメーターの状態
        anime = m_anime.GetCurrentAnimatorStateInfo(0);
        if (anime.fullPathHash != Animator.StringToHash("Base Layer.EffectEnd")) return;
        Destroy(gameObject);
    }
}
