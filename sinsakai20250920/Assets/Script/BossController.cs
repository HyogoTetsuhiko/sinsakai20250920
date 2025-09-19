using UnityEngine;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    [Header("ボス設定")]
    public int maxHP = 50;               // ボスの最大HP
    private int currentHP;               // 現在HP

    [Header("移動設定")]
    public Vector3 targetPosition;       // ボスが止まる位置
    public float moveSpeed = 3f;         // 移動速度
    private bool reachedPosition = false;// 定位置到達判定

    [Header("弾発射設定")]
    public GameObject bulletPrefab;      // 弾のプレハブ
    public Transform shootPoint;         // 弾の発射位置
    public float shootInterval = 2f;     // 発射間隔
    public int bulletCount = 5;          // 扇状に撃つ弾の数
    public float spreadAngle = 60f;      // 扇の角度

    private float shootTimer = 0f;
    private bool isPhase2 = false;       // HP半分で行動変化

    [Header("破壊演出設定")]
    public GameObject explosionPrefab;   // 爆発エフェクトPrefab
    public AudioClip explosionSE;        // 爆発効果音
    public int explosionCount = 5;       // 出す爆発の数
    public float explosionRadius = 2f;   // 爆発を出す範囲
    public float explosionLifeTime = 1.5f; // エフェクトが残る時間（秒）
    public float sceneChangeDelay = 2f;  // 演出後にシーン遷移するまでの待ち時間

    void Start()
    {
        currentHP = maxHP;

        // 上向きスプライトを左向きに回転
        transform.rotation = Quaternion.Euler(0f, 0f, 90f);

        // 出現位置を画面右外に設定（敵がスライドして入ってくる演出）
        Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(1.2f, 0.5f, 0));
        pos.z = 0;
        transform.position = pos;

        // 定位置（画面右端から少し左）を設定
        targetPosition = Camera.main.ViewportToWorldPoint(new Vector3(0.85f, 0.5f, 0));
        targetPosition.z = 0;
    }

    void Update()
    {
        // 移動処理
        if (!reachedPosition)
        {
            // スポーン位置からターゲット位置まで線形補間で移動
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // ターゲット位置に到達したら止まる
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                reachedPosition = true;
            }
        }
        else
        {
            // 弾発射処理
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootInterval)
            {
                shootTimer = 0f;
                ShootFan();
            }
        }
    }

    // 扇状に弾を撃つ処理
    void ShootFan()
    {
        if (bulletPrefab == null || shootPoint == null) return;

        // 扇状の開始角度と角度間隔を計算
        float startAngle = -spreadAngle / 2f;
        float angleStep = spreadAngle / (bulletCount - 1);

        // bulletCount の数だけ弾を生成
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + angleStep * i;

            // ボスの現在の回転を考慮して発射方向を決定
            Quaternion rot = Quaternion.Euler(0f, 0f, angle) * transform.rotation;

            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, rot);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.velocity = bullet.transform.up * 5f; // forward/up方向に速度を与える
        }
    }

    // ダメージを受ける処理
    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        // HPが半分以下になったら第2フェーズへ
        if (!isPhase2 && currentHP <= maxHP / 2)
            EnterPhase2();

        // HPが0以下になったら撃破
        if (currentHP <= 0)
            Die();
    }

    // フェーズ2突入処理
    void EnterPhase2()
    {
        isPhase2 = true;
        shootInterval *= 0.5f;  // 発射間隔を短縮
        bulletCount += 2;       // 撃つ弾の数を増加
        spreadAngle += 20f;     // 扇の角度を広げる
        Debug.Log("ボス フェーズ2突入！");
    }

    // ====================
    // ボス撃破処理
    // ====================
    void Die()
    {
        Debug.Log("ボス撃破！");

        // Coroutine を使って、撃破時の演出（爆発・効果音・待機・シーン遷移）を順番に処理
        StartCoroutine(DieRoutine());
    }

    private System.Collections.IEnumerator DieRoutine()
    {
        // 爆発エフェクト
        if (explosionPrefab != null)
        {
            for (int i = 0; i < explosionCount; i++)
            {
                // ボス周囲のランダム位置に爆発エフェクトを生成
                Vector3 randomOffset = new Vector3(
                    Random.Range(-explosionRadius, explosionRadius), // X方向ランダム
                    Random.Range(-explosionRadius, explosionRadius), // Y方向ランダム
                    0f // Zは2Dなので0
                );

                // エフェクトを生成
                GameObject explosion = Instantiate(
                    explosionPrefab,           // プレハブ
                    transform.position + randomOffset, // ボス位置にランダムオフセット
                    Quaternion.identity        // 回転なし
                );

                // 一定時間後に自動で破壊してメモリ解放
                Destroy(explosion, explosionLifeTime);
            }
        }

        // 効果音再生
        if (explosionSE != null)
        {
            // AudioSourceを新規生成せずにワンショットで再生
            // AudioSource.PlayClipAtPoint は3D空間の位置に音を鳴らす
            AudioSource.PlayClipAtPoint(explosionSE, transform.position);
        }

        // 一定時間待機
        // WaitForSecondsRealtime は Time.timeScale の影響を受けない
        // （ゲームが一時停止中でも待機時間が正確に経過）
        yield return new WaitForSecondsRealtime(sceneChangeDelay);

        // シーン遷移
        // ClearScene に遷移
        // この時点でボスオブジェクトは破棄されるか、シーン移動で自動破棄される
        SceneManager.LoadScene("ClearScene");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 当たったオブジェクトが "Bullet" タグか確認
        if (collision.CompareTag("Bullet"))
        {
            // 弾を破壊
            Destroy(collision.gameObject);

            // ボスにダメージ
            TakeDamage(1);
        }
    }
}