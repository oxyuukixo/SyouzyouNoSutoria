using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Load : MonoBehaviour {

    public Text Loading;
    private AsyncOperation async;

    IEnumerator Start()
    {
        // 非同期でロード開始
        async = Application.LoadLevelAsync("Title");
        // デフォルトはtrue。ロード完了したら勝手にシーンきりかえ発生しないよう設定。
        async.allowSceneActivation = false;

        // 非同期読み込み中の処理
        while (!async.isDone)
        {
            Debug.Log(async.progress * 100 + "%");
            yield return new WaitForEndOfFrame();
        }
        yield return async;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // タッチしたら遷移する（検証用）
            async.allowSceneActivation = true;
        }
        Loading.text = async.progress * 100 + "%";
    }
}
