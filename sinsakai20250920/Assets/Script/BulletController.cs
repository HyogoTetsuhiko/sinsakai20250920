using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [Header("弾の設定")]
    public float speed = 10f;      // 弾の速度
    public float lifeTime = 2f;    // 弾の寿命（秒）
    public int damage = 1;         // 敵に与えるダメージ量

    void Start()
    {
        // lifeTime 秒後に自動破棄
        Destroy(gameObject, lifeTime);

        // 弾のスプライトを右向きに回転
        transform.rotation = Quaternion.Euler(0f, 0f, 90f);
    }

    void Update()
    {
        // 弾を右方向に移動
        transform.position += Vector3.right * speed * Time.deltaTime;
    }

    // 敵との衝突判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 衝突対象が Enemy タグならダメージ処理
        if (collision.CompareTag("Enemy"))
        {
            // EnemyController スクリプトを取得してダメージ
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            // 弾を破棄
            Destroy(gameObject);
        }
    }
}
