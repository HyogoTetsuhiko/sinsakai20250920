using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeIn : MonoBehaviour
{
    // �t�F�[�h�Ώۂ�CanvasGroup
    // CanvasGroup�𑀍삷��ƁAUI�S�̂̓����x(alpha)�𐧌�ł���
    [SerializeField] private CanvasGroup canvasGroup;

    // �t�F�[�h�ɂ����鎞�ԁi�b�j
    [SerializeField] private float duration = 1f;

    private void Start()
    {
        // �Q�[���J�n���Ƀt�F�[�h�C���������J�n
        StartCoroutine(FadeInRoutine());
    }

    private IEnumerator FadeInRoutine()
    {
        float timer = 0f;       // �o�ߎ��ԗp�^�C�}�[

        // ������Ԃ����S�ɕs�����ialpha=1�j�ɐݒ�
        canvasGroup.alpha = 1f;

        // duration �b�Ԃ����� alpha ��0�ɂ��ē����ɂ���
        while (timer < duration)
        {
            timer += Time.deltaTime;                            // �o�ߎ��Ԃ����Z
            canvasGroup.alpha = 1f - (timer / duration);        // ���`��Ԃœ����x�����炷
            yield return null;                                  // 1�t���[���ҋ@
        }

        // �ŏI�I�Ɋ��S�����ɂ���i�덷�����j
        canvasGroup.alpha = 0f;
    }
}
