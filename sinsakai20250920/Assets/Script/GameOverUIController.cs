using UnityEngine;

public class GameOverUIController : MonoBehaviour
{
    // �Q�[�����ăX�^�[�g�i���̃V�[�����ēǂݍ��݁j
    public void OnRetryButton()
    {
        // ����SceneManager���Ă΂��AFadeManager�o�R�ŃV�[���ړ�
        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.FadeToScene("GameScene");
        }
        else
        {
            Debug.LogWarning("FadeManager��������܂���B���ڃV�[�������[�h���܂��B");
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        }
    }

    // ���C�����j���[�֖߂�
    public void OnTitleButton()
    {
        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.FadeToScene("TitleScene"); // �� �^�C�g���V�[�����ɍ��킹�ĕύX
        }
        else
        {
            Debug.LogWarning("FadeManager��������܂���B���ڃV�[�������[�h���܂��B");
            UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
        }
    }
}
