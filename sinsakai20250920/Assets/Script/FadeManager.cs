using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    [Header("�t�F�[�h�p�ݒ�")]
    public Image fadeImage;           // ��ʑS�̂𕢂�����Image
    public float fadeDuration = 1f;   // �t�F�[�h�ɂ����鎞�ԁi�b�j

    public static FadeManager Instance { get; private set; } // �V���O���g���p
    private bool isFading = false;    // �t�F�[�h�����ǂ����̃t���O�i�A�Ŗh�~�j

    private void Awake()
    {
        // �V���O���g�����i�V�[���ؑւł��c���j
        if (Instance != null)
        {
            Destroy(gameObject); // ���ɑ��݂���Ȃ�폜
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // �V�[�����ׂ��ł��j�����Ȃ�
    }

    private void Start()
    {
        if (fadeImage == null)
            Debug.LogError("fadeImage ���ݒ肳��Ă��܂���I");

        // �Q�[���J�n���Ƀt�F�[�h�C��
        StartCoroutine(FadeIn());
    }

    private void OnEnable()
    {
        // �V�[���؂�ւ��������Ƀt�F�[�h�C������
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // �V�����V�[���ɐ؂�ւ��������Ƀt�F�[�h�C���J�n
        StartCoroutine(FadeIn());
    }

    public void FadeToScene(string sceneName)
    {
        if (!isFading) // �t�F�[�h���Ȃ牽�����Ȃ��i�A�ő΍�j
            StartCoroutine(FadeOutAndLoad(sceneName));
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        isFading = true; // �t�F�[�h���t���OON

        float t = 0f;
        Color c = fadeImage.color;
        c.a = 0f; // �ŏ��͓���
        fadeImage.color = c;

        // ���X�ɍ�������
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = t / fadeDuration; // 0 �� 1 �ɕω�
            fadeImage.color = c;
            yield return null;
        }

        c.a = 1f; // �Ō�͊��S�ɍ�
        fadeImage.color = c;

        // �����Ȃ������ƂɃV�[�����[�h�J�n
        yield return SceneManager.LoadSceneAsync(sceneName);

        isFading = false; // �t�F�[�h����
    }

    private IEnumerator FadeIn()
    {
        float t = 0f;
        Color c = fadeImage.color;
        c.a = 1f; // �^��������X�^�[�g
        fadeImage.color = c;

        // ���X�ɓ����ɂ���
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = 1f - (t / fadeDuration); // 1 �� 0 �ɕω�
            fadeImage.color = c;
            yield return null;
        }

        c.a = 0f; // �Ō�͊��S����
        fadeImage.color = c;
    }
}
