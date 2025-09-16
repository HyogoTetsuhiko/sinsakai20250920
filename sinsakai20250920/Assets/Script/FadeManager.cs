using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public Image fadeImage;           // �����S���Image
    public float fadeDuration = 1f;   // �t�F�[�h����

    private void Start()
    {
        if (fadeImage == null)
            Debug.LogError("fadeImage ���ݒ肳��Ă��܂���I");
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
            c.a = t / fadeDuration;  // ���`��0��1
            fadeImage.color = c;
            yield return null;
        }

        c.a = 1f;
        fadeImage.color = c;

        SceneManager.LoadScene(sceneName);
    }
}
