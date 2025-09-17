using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public Image fadeImage;           // �����S���Image
    public float fadeDuration = 1f;   // �t�F�[�h����

    private static FadeManager instance;

    private void Awake()
    {
        // �V���O���g�������ăV�[�����܂����ł��c��
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
            Debug.LogError("fadeImage ���ݒ肳��Ă��܂���I");

        // �ŏ��̃V�[���J�n���̓t�F�[�h�C������
        StartCoroutine(FadeIn());
    }

    private void OnEnable()
    {
        // �V�[���؂�ւ����ɌĂ΂��C�x���g�o�^
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // �V�����V�[���ɐ؂�ւ������t�F�[�h�C���J�n
        StartCoroutine(FadeIn());
    }

    // �t�F�[�h�A�E�g���ăV�[���ؑ�
    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    private IEnumerator FadeOut(string sceneName)
    {
        float t = 0f;
        Color c = fadeImage.color;
        c.a = 0f;  // �����X�^�[�g
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
        c.a = 1f;  // �^�����X�^�[�g
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
