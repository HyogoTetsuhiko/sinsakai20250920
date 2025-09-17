using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeIn : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float duration = 1f;

    private void Start()
    {
        StartCoroutine(FadeInRoutine());
    }

    private IEnumerator FadeInRoutine()
    {
        float timer = 0f;
        canvasGroup.alpha = 1f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = 1f - (timer / duration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}
