using UnityEngine;
using UnityEngine.SceneManagement;  // �� ����K�{

public class TitleUIController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject titlePanel;
    public GameObject howToPlayPanel;

    // How To Play �{�^��
    public void ShowHowToPlay()
    {
        titlePanel.SetActive(false);
        howToPlayPanel.SetActive(true);
    }

    // Back �{�^��
    public void BackToTitle()
    {
        howToPlayPanel.SetActive(false);
        titlePanel.SetActive(true);
    }

    // Game Start �{�^��
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");  // �� �J�ڐ�̃V�[����
    }
}
