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

    // ★ ゲームシーンへの移行（フェード付き）
    public void StartGame()
    {
        FadeManager fm = FindObjectOfType<FadeManager>();
        if (fm != null)
            fm.FadeToScene("GameScene");
        else
            Debug.LogError("FadeManager が見つかりません！");
    }
}
