using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class PlayerHealth : MonoBehaviour
{
    [Header("HP設定")]
    public int maxHP = 5;             // プレイヤーの最大HP
    private int currentHP;            // 現在のHP

    [Header("UI設定")]
    public Slider hpSlider;           // World Space Canvas 上の HPバー（Slider）
    public Image fillImage;           // Slider の Fill Image 部分
    public Vector3 offset = new Vector3(0, 1.5f, 0); // 頭上に表示する位置

    [Header("HPバーの色")]
    public Color fullColor = Color.green;   // HP50%以上
    public Color midColor = Color.yellow;   // HP50%以下
    public Color lowColor = Color.red;      // HP20%以下

    void Start()
    {
        // ゲーム開始時に HP を最大値に設定
        currentHP = maxHP;

        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
        }

        UpdateFillColor();
    }

    void Update()
    {
        // HPバーをプレイヤーの頭上に追従
        if (hpSlider != null)
        {
            hpSlider.transform.position = transform.position + offset;
        }
    }

    // ダメージを受ける処理
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        if (hpSlider != null)
        {
            hpSlider.value = currentHP;
        }

        UpdateFillColor();

        if (currentHP <= 0)
        {
            Die();
        }
    }

    // HP割合に応じて色を変える
    void UpdateFillColor()
    {
        if (fillImage == null) return;

        float ratio = (float)currentHP / maxHP;

        if (ratio <= 0.2f)
            fillImage.color = lowColor;
        else if (ratio <= 0.5f)
            fillImage.color = midColor;
        else
            fillImage.color = fullColor;
    }

    // プレイヤー死亡時の処理
    void Die()
    {
        Debug.Log("プレイヤー死亡！ ゲームオーバーシーンに移行します");

        // ★ ゲームオーバーシーンへ切り替え
        SceneManager.LoadScene("GameOverScene");
    }
}
