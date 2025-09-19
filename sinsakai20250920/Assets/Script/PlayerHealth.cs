using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("HP設定")]
    public int maxHP = 5;             // 最大HP
    private int currentHP;            // 現在のHP

    [Header("UI設定")]
    public Slider hpSlider;           // HPバーのSlider
    public Image fillImage;           // HPバーのFill部分
    public Vector3 offset = new Vector3(0, 1.5f, 0); // プレイヤー頭上表示位置

    [Header("HPバー色")]
    public Color fullColor = Color.green;
    public Color midColor = Color.yellow;
    public Color lowColor = Color.red;

    void Start()
    {
        currentHP = maxHP;            // HP初期化

        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
        }

        UpdateFillColor();            // 色を初期化
    }

    void Update()
    {
        // HPバーをプレイヤーの頭上に追従
        if (hpSlider != null)
        {
            hpSlider.transform.position = transform.position + offset;
        }
    }

    // ----------------------------
    // ダメージ処理
    // ----------------------------
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP); // HP範囲制限

        if (hpSlider != null)
        {
            hpSlider.value = currentHP;  // UI更新
        }

        UpdateFillColor();               // 色更新

        // HPが0になったら死亡処理
        if (currentHP <= 0)
        {
            Die();
        }
    }

    // ----------------------------
    // HPに応じてバーの色を変える
    // ----------------------------
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

    // ----------------------------
    // プレイヤー死亡処理
    // ----------------------------
    void Die()
    {
        Debug.Log("プレイヤー死亡！ゲームオーバーシーンへ移行");
        SceneManager.LoadScene("GameOverScene");
    }
}
