using UnityEngine;

public class WaveEnemyController : MonoBehaviour
{
    [Header("移動設定")]
    public float speed = 3f;            // 左方向への前進速度
    public float waveAmplitude = 1f;    // 上下揺れの振幅
    public float waveFrequency = 2f;    // 上下揺れの速さ（周波数）

    private Vector3 startPosition;      // 出現時の位置を保持
    private float elapsedTime = 0f;     // 経過時間

    [Header("HP設定")]
    public int maxHP = 3;               // 敵の最大HP
    private int currentHP;              // 現在のHP

    [Header("接触ダメージ")]
    public int contactDamage = 1;       // プレイヤーに接触したときのダメージ量

    [Header("攻撃設定")]
    public GameObject bulletPrefab;     // 敵が撃つ弾のプレハブ
    public Transform shootPoint;        // 弾の発射位置（敵の口や砲台部分）
    public float shootInterval = 2f;    // 弾を撃つ間隔（秒）
    public float bulletSpeed = 5f;      // 弾の移動速度

    private float shootTimer = 0f;      // 弾発射用タイマー

    void Start()
    {
        // HPを初期化
        currentHP = maxHP;

        // 出現時の位置を保存（波動の基準になる）
        startPosition = transform.position;
    }

    void Update()
    {
        // --- 左方向に前進 ---
        transform.position += Vector3.left * speed * Time.deltaTime;

        // --- 波動（上下移動） ---
        elapsedTime += Time.deltaTime;
        float yOffset = Mathf.Sin(elapsedTime * waveFrequency) * waveAmplitude;
        transform.position = new Vector3(transform.position.x, startPosition.y + yOffset, transform.position.z);

        // --- 弾発射タイマー ---
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            shootTimer = 0f;
            ShootAtPlayer(); // プレイヤーに向かって弾を発射
        }

        // --- 画面外判定（左端を通過したら削除） ---
        if (Camera.main != null)
        {
            // ワールド座標の左端を取得
            float leftBound = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;

            // 敵の右端が左端より左に出たら削除
            if (transform.position.x + 0.5f < leftBound) // 0.5fは敵の半幅調整
            {
                Destroy(gameObject);
            }
        }
    }

    // --- プレイヤーに向かって弾を発射する処理 ---
    void ShootAtPlayer()
    {
        // 弾Prefabまたは発射位置が設定されていなければ何もしない
        if (bulletPrefab == null || shootPoint == null) return;

        // シーン内のPlayerオブジェクトを取得
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return; // Playerが存在しなければ発射しない

        // 弾を生成
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

        // プレイヤー方向への単位ベクトルを計算
        Vector2 direction = (player.transform.position - shootPoint.position).normalized;

        // Rigidbody2Dがあれば速度を設定
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
        }

        // 弾の向きをプレイヤー方向に回転
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // --- ダメージを受ける処理 ---
    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        // HPが0以下になったら破壊
        if (currentHP <= 0)
        {
            Die();
        }
    }

    // --- 敵破壊処理 ---
    void Die()
    {
        Destroy(gameObject);
        // スコア加算や破壊エフェクトもここに追加可能
    }

    // --- 衝突判定 ---
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーと接触した場合
        if (collision.CompareTag("Player"))
        {
            PlayerHealth player = collision.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(1); // ダメージ量は調整
            }

            Destroy(gameObject); // 弾を破壊
        }
    
        // プレイヤーの弾と接触した場合
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject); // 弾を削除
            TakeDamage(2);                 // 敵がダメージを受ける
        }
    }
}
