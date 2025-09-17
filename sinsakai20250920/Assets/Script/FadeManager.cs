using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public Image fadeImage;           // 黒い全画面Image
    public float fadeDuration = 1f;   // フェード時間

    private static FadeManager instance;

    private void Awake()
    {
        // シングルトン化してシーンをまたいでも残す
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (fadeImage == null)
            Debug.LogError("fadeImage が設定されていません！");

        // 最初のシーン開始時はフェードインする
        StartCoroutine(FadeIn());
    }

    private void OnEnable()
    {
        // シーン切り替え時に呼ばれるイベント登録
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 新しいシーンに切り替わったらフェードイン開始
        StartCoroutine(FadeIn());
    }

    // フェードアウトしてシーン切替
    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    private IEnumerator FadeOut(string sceneName)
    {
        float t = 0f;
        Color c = fadeImage.color;
        c.a = 0f;  // 透明スタート
        fadeImage.color = c;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = t / fadeDuration;
            fadeImage.color = c;
            yield return null;
        }

        c.a = 1f;
        fadeImage.color = c;

        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator FadeIn()
    {
        float t = 0f;
        Color c = fadeImage.color;
        c.a = 1f;  // 真っ黒スタート
        fadeImage.color = c;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = 1f - (t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = 0f;
        fadeImage.color = c;
    }
}
