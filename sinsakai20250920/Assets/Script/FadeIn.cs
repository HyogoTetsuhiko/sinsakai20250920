using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeIn : MonoBehaviour
{
    // フェード対象のCanvasGroup
    // CanvasGroupを操作すると、UI全体の透明度(alpha)を制御できる
    [SerializeField] private CanvasGroup canvasGroup;

    // フェードにかかる時間（秒）
    [SerializeField] private float duration = 1f;

    private void Start()
    {
        // ゲーム開始時にフェードイン処理を開始
        StartCoroutine(FadeInRoutine());
    }

    private IEnumerator FadeInRoutine()
    {
        float timer = 0f;       // 経過時間用タイマー

        // 初期状態を完全に不透明（alpha=1）に設定
        canvasGroup.alpha = 1f;

        // duration 秒間かけて alpha を0にして透明にする
        while (timer < duration)
        {
            timer += Time.deltaTime;                            // 経過時間を加算
            canvasGroup.alpha = 1f - (timer / duration);        // 線形補間で透明度を減らす
            yield return null;                                  // 1フレーム待機
        }

        // 最終的に完全透明にする（誤差調整）
        canvasGroup.alpha = 0f;
    }
}
