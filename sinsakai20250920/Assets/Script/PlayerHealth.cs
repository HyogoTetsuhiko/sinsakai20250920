using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("HP設定")]
    public int maxHP = 5;   // プレイヤー最大HP
    private int currentHP;  // 現在HP

    [Header("UI設定")]
    public Slider hpSlider;           // 頭上に表示するHPバー
    public Image fillImage;           // HPバーの色部分
    public Vector3 offset = new Vector3(0, 1.5f, 0); // HPバーの表示位置

    [Header("HPバーの色")]
    public Color fullColor = Color.green;   // HPが多いとき
    public Color midColor = Color.yellow;   // HPが半分以下
    public Color lowColor = Color.red;      // HPが少ないとき

    [Header("死亡演出")]
    public GameObject explosionPrefab; // 死亡時に出す爆発エフェクトのPrefab

    // 爆発音設定
    [Header("効果音設定")]
    public AudioClip explosionSE;     // 爆発音
    private AudioSource audioSource;  // 効果音再生用

    void Start()
    {
        // ゲーム開始時にHPを最大値にセット
        currentHP = maxHP;

        // HPバーが設定されていれば初期値を反映
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
        }

        // HPに応じてバーの色を更新
        UpdateFillColor();

        // AudioSource を取得
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource が Player にアタッチされていません。効果音は再生されません。");
        }
    }

    void Update()
    {
        // HPバーをプレイヤーの位置に追従させる
        if (hpSlider != null)
        {
            hpSlider.transform.position = transform.position + offset;
        }
    }

    // ダメージを受ける処理
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP); // HPが0未満にならないように制限

        // HPバーが設定されていれば反映
        if (hpSlider != null)
        {
            hpSlider.value = currentHP;
        }

        // 色を更新
        UpdateFillColor();

        // HPが0になったら死亡処理へ
        if (currentHP <= 0)
        {
            StartCoroutine(Die()); // コルーチンで爆発演出 → シーン移行
        }
    }

    // HPバーの色を現在HPに応じて更新
    void UpdateFillColor()
    {
        if (fillImage == null) return;

        float ratio = (float)currentHP / maxHP; // HP割合

        if (ratio <= 0.2f)
            fillImage.color = lowColor;   // HP 20%以下
        else if (ratio <= 0.5f)
            fillImage.color = midColor;   // HP 50%以下
        else
            fillImage.color = fullColor;  // HP 50%以上
    }

    // プレイヤー死亡時の演出とシーン移行
    IEnumerator Die()
    {
        Debug.Log("プレイヤー死亡！ 爆発アニメーション再生");

        // 爆発エフェクトを再生
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // 追加部分：爆発音を再生
        if (explosionSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(explosionSE);
        }

        // 待機（エフェクトと音を再生する時間）
        yield return new WaitForSeconds(1.2f);

        // プレイヤーを消す
        gameObject.SetActive(false);

        // ゲームオーバーシーンへ移行
        SceneManager.LoadScene("GameOverScene");
    }
}
