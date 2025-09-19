using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 5f;        // 上下移動スピード
    [SerializeField] float minY = -4f;
    [SerializeField] float maxY = 4f;

    [Header("攻撃設定")]
    public GameObject bulletPrefab;     // 弾プレハブ
    public Transform shootPoint;        // 発射位置（子オブジェクト）

    [Header("効果音設定")]
    public AudioClip shootSE;           // 弾発射音
    private AudioSource audioSource;    // AudioSource参照

    private Rigidbody2D rb;
    private float moveInput;

    // HP管理用コンポーネント
    private PlayerHealth playerHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        // PlayerHealth を取得
        playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth コンポーネントがアタッチされていません！");
        }
    }

    void Update()
    {
        // 上下移動入力取得
        moveInput = Input.GetAxisRaw("Vertical");

        // 攻撃
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        // 上下移動
        rb.velocity = new Vector2(0f, moveInput * moveSpeed);

        // 移動範囲の制限
        Vector2 clampedPos = rb.position;
        clampedPos.y = Mathf.Clamp(clampedPos.y, minY, maxY);
        rb.position = clampedPos;
    }

    void Shoot()
    {
        if (bulletPrefab != null && shootPoint != null)
        {
            // 弾を生成（回転なし）
            Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            // 発射音を鳴らす処理
            if (shootSE != null && audioSource != null)
            {
                audioSource.PlayOneShot(shootSE);
            }
        }
        else
        {
            Debug.LogWarning("bulletPrefab または shootPoint が設定されていません！");
        }
    }

    // ----------------------------
    // 敵や敵弾との衝突判定
    // ----------------------------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerHealth == null) return;

        // 敵弾に接触した場合
        if (collision.CompareTag("EnemyBullet"))
        {
            Destroy(collision.gameObject);       // 弾を削除
            playerHealth.TakeDamage(1);         // PlayerHealthにダメージを委譲
        }

        // 敵に接触した場合（接触ダメージ）
        if (collision.CompareTag("Enemy"))
        {
            playerHealth.TakeDamage(1);         // PlayerHealthにダメージを委譲
        }
    }
}
