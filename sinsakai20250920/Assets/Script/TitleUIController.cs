using UnityEngine;
using UnityEngine.SceneManagement;  // ← これ必須

public class TitleUIController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject titlePanel;
    public GameObject howToPlayPanel;

    // How To Play ボタン
    public void ShowHowToPlay()
    {
        titlePanel.SetActive(false);
        howToPlayPanel.SetActive(true);
    }

    // Back ボタン
    public void BackToTitle()
    {
        howToPlayPanel.SetActive(false);
        titlePanel.SetActive(true);
    }

    // Game Start ボタン
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");  // ← 遷移先のシーン名
    }
}
