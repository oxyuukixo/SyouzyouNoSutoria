using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Load : MonoBehaviour {

    public Text Loading;
    private AsyncOperation async;

    IEnumerator Start()
    {
        // 非同期でロード開始
        async = SceneManager.LoadSceneAsync("Title");
        // デフォルトはtrue。ロード完了したら勝手にシーンきりかえ発生しないよう設定。
        async.allowSceneActivation = false;

        // 非同期読み込み中の処理
        while (async.progress < 0.9f)
        {
            Loading.text = (async.progress * 100).ToString("F0") + "%";
            yield return new WaitForEndOfFrame();
        }

        Loading.text = "100%";

        yield return new WaitForSeconds(1);

        async.allowSceneActivation = true;    // シーン遷移許可
    }

    // Update is called once per frame
    void Update()
    {
        // タッチしたら遷移する(検証用)*********************************
        //if (Input.GetMouseButtonDown(0))
        //{
        //    async.allowSceneActivation = true;
        //}
        //**********************************************************
    }
}
