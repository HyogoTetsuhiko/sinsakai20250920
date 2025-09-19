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
    private bool isPhase2 = false;

    void Start()
    {
        currentHP = maxHP;

        // 上向きスプライトを左向きに回転
        transform.rotation = Quaternion.Euler(0f, 0f, 90f);

        // 出現位置を画面右側に設定
        Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(1.2f, 0.5f, 0)); // 画面外右から出現
        pos.z = 0;
        transform.position = pos;

        // 定位置（画面右端から少し左）を設定
        targetPosition = Camera.main.ViewportToWorldPoint(new Vector3(0.85f, 0.5f, 0));
        targetPosition.z = 0;
    }

    void Update()
    {
        // --- 移動処理 ---
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
            // --- 弾発射処理 ---
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootInterval)
            {
                shootTimer = 0f;
                ShootFan();
            }
        }
    }

    // 扇状に弾を撃つ
    void ShootFan()
    {
        if (bulletPrefab == null || shootPoint == null) return;

        float startAngle = -spreadAngle / 2f;
        float angleStep = spreadAngle / (bulletCount - 1);

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + angleStep * i;

            // ボス回転を考慮
            Quaternion rot = Quaternion.Euler(0f, 0f, angle) * transform.rotation;

            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, rot);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.velocity = bullet.transform.up * 5f;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (!isPhase2 && currentHP <= maxHP / 2)
            EnterPhase2();

        if (currentHP <= 0)
            Die();
    }

    void EnterPhase2()
    {
        isPhase2 = true;
        shootInterval *= 0.5f;
        bulletCount += 2;
        spreadAngle += 20f;
        Debug.Log("ボス フェーズ2突入！");
    }

    void Die()
    {
        Debug.Log("ボス撃破！");
        SceneManager.LoadScene("ClearScene");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            TakeDamage(1);
        }
    }
}
