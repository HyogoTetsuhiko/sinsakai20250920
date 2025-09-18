using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3f;

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
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject); // 弾を削除
            Destroy(gameObject);           // 敵を削除
        }
    }
}
