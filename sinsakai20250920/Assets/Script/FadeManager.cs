using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    [Header("フェード用設定")]
    public Image fadeImage;           // 画面全体を覆う黒いImage
    public float fadeDuration = 1f;   // フェードにかける時間（秒）

    public static FadeManager Instance { get; private set; } // シングルトン用
    private bool isFading = false;    // フェード中かどうかのフラグ（連打防止）

    private void Awake()
    {
        // シングルトン化（シーン切替でも残す）
        if (Instance != null)
        {
            Destroy(gameObject); // 既に存在するなら削除
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // シーンを跨いでも破棄しない
    }

    private void Start()
    {
        if (fadeImage == null)
            Debug.LogError("fadeImage が設定されていません！");

        // ゲーム開始時にフェードイン
        StartCoroutine(FadeIn());
    }

    private void OnEnable()
    {
        // シーン切り替え完了時にフェードインする
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 新しいシーンに切り替わった直後にフェードイン開始
        StartCoroutine(FadeIn());
    }

    public void FadeToScene(string sceneName)
    {
        if (!isFading) // フェード中なら何もしない（連打対策）
            StartCoroutine(FadeOutAndLoad(sceneName));
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        isFading = true; // フェード中フラグON

        float t = 0f;
        Color c = fadeImage.color;
        c.a = 0f; // 最初は透明
        fadeImage.color = c;

        // 徐々に黒くする
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = t / fadeDuration; // 0 → 1 に変化
            fadeImage.color = c;
            yield return null;
        }

        c.a = 1f; // 最後は完全に黒
        fadeImage.color = c;

        // 黒くなったあとにシーンロード開始
        yield return SceneManager.LoadSceneAsync(sceneName);

        isFading = false; // フェード完了
    }

    private IEnumerator FadeIn()
    {
        float t = 0f;
        Color c = fadeImage.color;
        c.a = 1f; // 真っ黒からスタート
        fadeImage.color = c;

        // 徐々に透明にする
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = 1f - (t / fadeDuration); // 1 → 0 に変化
            fadeImage.color = c;
            yield return null;
        }

        c.a = 0f; // 最後は完全透明
        fadeImage.color = c;
    }
}
