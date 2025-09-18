using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("移動と接触ダメージ")]
    public float speed = 3f;          // 敵の移動速度
    public int damage = 1;            // プレイヤーに接触した際のダメージ量

    [Header("弾の発射設定")]
    public GameObject bulletPrefab;   // 敵が発射する弾のプレハブ
    public Transform shootPoint;      // 弾を発射する位置（Emptyオブジェクトなど）
    public float shootInterval = 2f;  // 弾を撃つ間隔（秒）

    private float shootTimer = 0f;    // 弾発射タイマー
    private Transform player;         // プレイヤーのTransformを保持

    void Start()
    {
        // 敵のスプライトが上向きの場合、左向きに回転
        transform.rotation = Quaternion.Euler(0, 0, 90);

        // シーン内のプレイヤーを探してTransformを取得
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        // 左方向に移動（ワールド座標基準）
        transform.Translate(Vector2.left * speed * Time.deltaTime, Space.World);

        // 画面外に出た場合に破棄
        CheckOffScreen();

        // プレイヤーが存在する場合のみ弾を発射
        if (player != null)
        {
            shootTimer += Time.deltaTime;  // タイマー更新

            if (shootTimer >= shootInterval) // 間隔が経過したら発射
            {
                ShootAtPlayer();   // プレイヤー方向に弾を発射
                shootTimer = 0f;   // タイマーリセット
            }
        }
    }

    // プレイヤー方向に弾を発射する関数
    void ShootAtPlayer()
    {
        // 弾プレハブと発射位置が設定されているか確認
        if (bulletPrefab != null && shootPoint != null)
        {
            // 弾を発射位置に生成
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

            // プレイヤー方向のベクトルを計算して正規化
            Vector2 dir = (player.position - shootPoint.position).normalized;

            // 弾のスクリプトに方向をセット（EnemyBulletスクリプトが必要）
            bullet.GetComponent<EnemyBullet>()?.Init(dir);
        }
    }

    // プレイヤーや弾との接触判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーの弾に当たった場合
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject); // 弾を削除
            Destroy(gameObject);           // 敵を削除
            return;                        // 処理終了
        }

        // プレイヤーに接触した場合
        if (collision.CompareTag("Player"))
        {
            // PlayerHealth スクリプトを取得
            PlayerHealth ph = collision.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);     // プレイヤーにダメージ
            }

            Destroy(gameObject);           // 敵を削除
        }
    }

    // 敵が画面外に出た場合に自動で破棄する関数
    void CheckOffScreen()
    {
        // カメラの左端のワールド座標を取得
        float leftBound = Camera.main.ViewportToWorldPoint(Vector3.zero).x;

        // 敵の位置が左端よりさらに左に出たら破棄
        if (transform.position.x < leftBound - 1f)  // -1fは余裕を持たせる
        {
            Destroy(gameObject); // 画面外なので削除
        }
    }
}
