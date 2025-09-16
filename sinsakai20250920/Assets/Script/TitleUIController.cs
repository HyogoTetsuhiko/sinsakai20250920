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
}
