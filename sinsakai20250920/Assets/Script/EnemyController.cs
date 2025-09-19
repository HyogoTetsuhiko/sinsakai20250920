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

    private float shootTimer = 0f;

    void Start()
    {
        currentHP = maxHP;

        // 敵スプライトが上向きの場合、左向きに回転
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

    void Shoot()
    {
        if (bulletPrefab == null || shootPoint == null) return;

        // 弾を生成
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

        // Rigidbody2D があれば弾を進ませる
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = bullet.transform.up * 5f;  // 左向きに飛ばす
        }
    }

    // ダメージ処理
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        // スコア加算やエフェクト生成もここで可能
    }
}
