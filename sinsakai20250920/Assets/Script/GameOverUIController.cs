using UnityEngine;

public class GameOverUIController : MonoBehaviour
{
    // ゲームを再スタート（今のシーンを再読み込み）
    public void OnRetryButton()
    {
        // 直接SceneManagerを呼ばず、FadeManager経由でシーン移動
        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.FadeToScene("GameScene");
        }
        else
        {
            Debug.LogWarning("FadeManagerが見つかりません。直接シーンをロードします。");
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        }
    }

    // メインメニューへ戻る
    public void OnTitleButton()
    {
        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.FadeToScene("TitleScene"); // ← タイトルシーン名に合わせて変更
        }
        else
        {
            Debug.LogWarning("FadeManagerが見つかりません。直接シーンをロードします。");
            UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
        }
    }
}
