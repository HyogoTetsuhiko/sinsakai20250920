using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 5f;      // 弾の速度
    public float lifeTime = 3f;   // 弾が自動で消えるまでの時間

    private Vector2 moveDirection; // 弾の移動方向

    // 弾が発射されるときに方向を設定
    public void Init(Vector2 direction)
    {
        moveDirection = direction.normalized; // 単位ベクトルに正規化
    }

    void Start()
    {
        // lifeTime 秒後に弾を自動破壊
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // 毎フレーム、設定した方向に速度を掛けて移動
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーに当たったらダメージを与える
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1); // ダメージ量を指定
            }

            Destroy(gameObject); // 弾を破壊
        }
    }
}
