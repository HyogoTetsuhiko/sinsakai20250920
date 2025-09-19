using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("HP設定")]
    public int maxHP = 3;
    private int currentHP;

    [Header("移動設定")]
    public float speed = 3f;

    [Header("弾発射設定")]
    public GameObject bulletPrefab;   // 弾のプレハブ
    public Transform shootPoint;      // 弾を撃つ位置
    public float shootInterval = 2f;  // 発射間隔

    [Header("プレイヤー接触設定")]
    public int damage = 1;            // プレイヤーに与えるダメージ量

    private float shootTimer = 0f;

    void Start()
    {
        currentHP = maxHP;

        // スプライトが上向きの場合、左向きに回転
        transform.rotation = Quaternion.Euler(0f, 0f, 90f);
    }

    void Update()
    {
        // 左方向に移動
        transform.Translate(Vector2.left * speed * Time.deltaTime, Space.World);

        // 弾発射処理
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            shootTimer = 0f;
            Shoot();
        }
    }

    // --- 弾を発射 ---
    void Shoot()
    {
        if (bulletPrefab == null || shootPoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // 弾の向きに沿って飛ばす
            rb.velocity = bullet.transform.up * 5f;
        }
    }

    // --- ダメージ処理 ---
    public void TakeDamage(int damageAmount)
    {
        currentHP -= damageAmount;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        // 必要であればスコア加算やエフェクト生成
    }

    // --- プレイヤーとの接触判定 ---
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーに接触した場合
        if (collision.CompareTag("Player"))
        {
            // PlayerHealth スクリプトを取得
            PlayerHealth ph = collision.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);  // プレイヤーにダメージ
            }

            Destroy(gameObject); // 接触した敵を削除
        }

        // プレイヤー弾に当たった場合
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject); // 弾を削除
            TakeDamage(1);                // 敵にダメージ
        }
    }
}
