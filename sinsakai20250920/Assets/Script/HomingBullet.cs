using UnityEngine;

public class HomingBullet : MonoBehaviour
{
    public float speed = 5f;             // 弾の速度
    public float rotateSpeed = 200f;     // 弾の回転速度（追尾の速さ）
    public float lifeTime = 5f;          // 弾の寿命

    private Transform target;            // 追尾対象

    void Start()
    {
        // 5秒後に自動削除
        Destroy(gameObject, lifeTime);

        // Playerタグを持つオブジェクトを探す
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
        }
    }

    void Update()
    {
        if (target == null)
        {
            // ターゲットがいなければ直進
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            return;
        }

        // 追尾方向のベクトルを計算
        Vector2 direction = (Vector2)(target.position - transform.position);
        direction.Normalize();

        // 弾の向きをスムーズにターゲット方向に回転
        float rotateAmount = Vector3.Cross(direction, transform.right).z;
        transform.Rotate(0, 0, -rotateAmount * rotateSpeed * Time.deltaTime);

        // 弾を前進（右向きが前方向）
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーに接触したらダメージ
        if (collision.CompareTag("Player"))
        {
            PlayerHealth player = collision.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(1); // ダメージ量は調整
            }

            Destroy(gameObject); // 弾を破壊
        }
    }
}
