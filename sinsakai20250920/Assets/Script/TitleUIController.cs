using UnityEngine;

public class TitleUIController : MonoBehaviour
{
    public GameObject titlePanel;
    public GameObject howToPlayPanel;

    public void ShowHowToPlay()
    {
        titlePanel.SetActive(false);
        howToPlayPanel.SetActive(true);
    }

    public void BackToTitle()
    {
        howToPlayPanel.SetActive(false);
        titlePanel.SetActive(true);
    }

    // �� �Q�[���V�[���ւ̈ڍs�i�t�F�[�h�t���j
    public void StartGame()
    {
        FadeManager fm = FindObjectOfType<FadeManager>();
        if (fm != null)
            fm.FadeToScene("GameScene");
        else
            Debug.LogError("FadeManager ��������܂���I");
    }
}
