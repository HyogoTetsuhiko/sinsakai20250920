using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("HP設定")]
    public int maxHP = 5;             // プレイヤーの最大HP
    private int currentHP;            // 現在のHP

    [Header("UI設定")]
    public Slider hpSlider;           // World Space Canvas 上の Slider
    public Image fillImage;           // Slider の Fill Image
    public Vector3 offset = new Vector3(0, 1.5f, 0); // プレイヤー頭上に表示するオフセット

    [Header("HPバーの色")]
    public Color fullColor = Color.green;   // HPが50%以上の時の色
    public Color midColor = Color.yellow;   // HPが50%以下の時の色
    public Color lowColor = Color.red;      // HPが20%以下の時の色

    void Start()
    {
        // 初期HPを最大値に設定
        currentHP = maxHP;

        // Slider が設定されていれば初期化
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;      // Sliderの最大値を設定
            hpSlider.value = currentHP;     // 現在値を反映
        }

        // 初期色を設定
        UpdateFillColor();
    }

    void Update()
    {
        // World Space Canvas 上の HPバーをプレイヤーに追従させる
        if (hpSlider != null)
        {
            hpSlider.transform.position = transform.position + offset;
        }
    }

    // ダメージを受ける処理
    public void TakeDamage(int damage)
    {
        // 現在HPを減らす
        currentHP -= damage;

        // HPが0未満や最大HP以上にならないように制限
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        // Slider に HP を反映
        if (hpSlider != null)
        {
            hpSlider.value = currentHP;
        }

        // HPに応じて色を更新
        UpdateFillColor();

        // HPが0になった場合の処理
        if (currentHP <= 0)
        {
            Die();
        }
    }

    // HPバーの色を HP割合に応じて切り替える
    void UpdateFillColor()
    {
        if (fillImage == null) return; // Fill Image が未設定の場合は何もしない

        // 現在HPの割合を計算（0～1）
        float ratio = (float)currentHP / maxHP;

        // HP割合に応じて色を切り替える
        if (ratio <= 0.2f)
        {
            fillImage.color = lowColor;   // 20%以下 → 赤
        }
        else if (ratio <= 0.5f)
        {
            fillImage.color = midColor;   // 50%以下 → 黄
        }
        else
        {
            fillImage.color = fullColor;  // 50%以上 → 緑
        }
    }

    // プレイヤー死亡時の処理
    void Die()
    {
        Debug.Log("プレイヤー死亡！");
        // ゲームオーバー処理やリスポーン処理をここに追加可能
    }
}
