using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3f;
    public int damage = 1; // プレイヤーに与えるダメージ量

    void Start()
    {
        // スプライトが上向きの場合 → 左向きになるように回転
        transform.rotation = Quaternion.Euler(0, 0, 90);
    }

    void Update()
    {
        // 左方向（ワールド座標基準）に移動
        transform.Translate(Vector2.left * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 弾に当たった場合
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject); // 弾を削除
            Destroy(gameObject);           // 敵を削除
            return; // ここで処理終了
        }

        // プレイヤーに当たった場合
        if (collision.CompareTag("Player"))
        {
            PlayerHealth ph = collision.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage); // プレイヤーにダメージ
            }

            Destroy(gameObject); // 敵を削除
        }
    }
}
