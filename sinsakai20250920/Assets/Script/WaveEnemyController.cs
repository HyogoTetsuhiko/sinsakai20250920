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

    [Header("破壊演出設定")]
    public GameObject explosionPrefab;   // 爆発エフェクトPrefab
    public AudioClip explosionSE;        // 爆発効果音
    public float explosionLifeTime = 1.5f; // エフェクトが残る時間（秒）

    void Start()
    {
        currentHP = maxHP;
        startPosition = transform.position;
    }

    void Update()
    {
        // 左方向に移動
        transform.position += Vector3.left * speed * Time.deltaTime;

        // 波動（上下移動）
        elapsedTime += Time.deltaTime;
        float yOffset = Mathf.Sin(elapsedTime * waveFrequency) * waveAmplitude;
        transform.position = new Vector3(transform.position.x, startPosition.y + yOffset, transform.position.z);

        // 弾発射
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            shootTimer = 0f;
            ShootAtPlayer();
        }

        // 画面外で削除
        if (Camera.main != null)
        {
            float leftBound = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
            if (transform.position.x + 0.5f < leftBound)
            {
                Destroy(gameObject);
            }
        }
    }

    // プレイヤーに向かって弾を撃つ処理
    void ShootAtPlayer()
    {
        // 弾のPrefabか発射位置が未設定なら何もしない
        if (bulletPrefab == null || shootPoint == null) return;

        // シーン内から"Player"タグのついたオブジェクトを探す
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return; // プレイヤーがいなければ発射しない

        // 弾を生成（発射位置に生成、回転は初期値）
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

        // プレイヤーの方向を計算して単位ベクトルに変換
        Vector2 direction = (player.transform.position - shootPoint.position).normalized;

        // 弾にRigidbody2Dがあるなら、速度を設定して発射
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed; // 弾速をかけて進行方向を決定
        }

        // 弾の向きをプレイヤー方向に回転させる（見た目が自然になる）
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // ダメージを受けたときの処理
    public void TakeDamage(int damage)
    {
        // 現在のHPからダメージ分を引く
        currentHP -= damage;

        // HPが0以下なら死亡処理を呼び出す
        if (currentHP <= 0)
        {
            Die();
        }
    }


    // 破壊処理
    void Die()
    {
        // 爆発エフェクトを生成（一定時間後に自動削除）
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, explosionLifeTime); // 指定秒後に削除
        }

        // 効果音を再生（別オブジェクトで再生してから削除）
        if (explosionSE != null)
        {
            GameObject audioObj = new GameObject("ExplosionSound");
            AudioSource tempSource = audioObj.AddComponent<AudioSource>();
            tempSource.PlayOneShot(explosionSE);
            Destroy(audioObj, explosionSE.length);
        }

        Destroy(gameObject); // 敵本体を削除
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth player = collision.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(contactDamage);
            }

            Die(); // 直接Destroyではなく、演出付き破壊
        }

        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            TakeDamage(2);
        }
    }
}
