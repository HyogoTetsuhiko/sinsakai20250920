using UnityEngine;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    [Header("ボス設定")]
    public int maxHP = 50;               // ボスの最大HP
    private int currentHP;               // 現在HP

    [Header("弾発射設定")]
    public GameObject bulletPrefab;      // 発射する弾のプレハブ
    public Transform shootPoint;         // 弾の発射位置
    public float shootInterval = 2f;     // 発射間隔
    public int bulletCount = 5;          // 一度に撃つ弾の数
    public float spreadAngle = 60f;      // 扇状に広がる角度

    private float shootTimer = 0f;       // 発射タイマー
    private bool isPhase2 = false;       // フェーズ2移行判定

    void Start()
    {
        // HP初期化
        currentHP = maxHP;

        // ボスのスプライトが上向きなら左向きに回転
        transform.rotation = Quaternion.Euler(0f, 0f, 90f);

        // 出現位置を画面右側に固定
        Vector3 pos = transform.position;
        pos.x = Camera.main.ViewportToWorldPoint(new Vector3(0.85f, 0.5f, 0)).x;
        transform.position = pos;
    }

    void Update()
    {
        // 弾発射タイマー更新
        shootTimer += Time.deltaTime;

        // 発射間隔を超えたら扇状に弾を撃つ
        if (shootTimer >= shootInterval)
        {
            shootTimer = 0f;
            ShootFan();
        }
    }

    // --- 扇状に複数の弾を発射 ---
    void ShootFan()
    {
        if (bulletPrefab == null || shootPoint == null) return;

        // 扇の左端の角度
        float startAngle = -spreadAngle / 2f;
        // 弾同士の角度差
        float angleStep = spreadAngle / (bulletCount - 1);

        for (int i = 0; i < bulletCount; i++)
        {
            // 現在の弾の角度
            float angle = startAngle + angleStep * i;

            // ボスの回転を考慮して弾の角度を設定
            // これでボスが左向きでも弾が正しく飛ぶ
            Quaternion rot = Quaternion.Euler(0f, 0f, angle) * transform.rotation;

            // 弾を生成
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, rot);

            // Rigidbody2D があれば速度を設定
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // 弾は自身の上方向（transform.up）に進む
                rb.velocity = bullet.transform.up * 5f;
            }
        }
    }

    // --- ダメージを受けた処理 ---
    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        // HP半分以下でフェーズ2へ
        if (!isPhase2 && currentHP <= maxHP / 2)
        {
            EnterPhase2();
        }

        // HP0以下で撃破
        if (currentHP <= 0)
        {
            Die();
        }
    }

    void EnterPhase2()
    {
        isPhase2 = true;

        // 攻撃強化（発射間隔短縮、弾増加、扇角拡大）
        shootInterval *= 0.5f;
        bulletCount += 2;
        spreadAngle += 20f;

        Debug.Log("ボス フェーズ2突入！");
    }

    void Die()
    {
        Debug.Log("ボス撃破！");
        SceneManager.LoadScene("ClearScene"); // クリアシーンへ
    }

    // --- 衝突判定 ---
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject); // 弾削除
            TakeDamage(1);                // ダメージを与える
        }
    }
}
