using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;        // 上下移動スピード
    public GameObject bulletPrefab;    // 弾プレハブ
    public Transform shootPoint;       // 発射位置（子オブジェクトにEmptyを置くと便利）
    [SerializeField] float minY = -4f;
    [SerializeField] float maxY = 4f;
    private Rigidbody2D rb;
    private float moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 上下移動入力（W/S または ↑/↓）
        moveInput = Input.GetAxisRaw("Vertical");
        // ↑ Unity標準の InputManager で "Vertical" は W/S と ↑/↓ が紐づいている

        // 攻撃
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(0f, moveInput * moveSpeed);

        // 位置制限
        Vector2 clampedPos = rb.position;
        clampedPos.y = Mathf.Clamp(clampedPos.y, minY, maxY);
        rb.position = clampedPos;
    }

    void Shoot()
    {
        if (bulletPrefab != null && shootPoint != null)
        {
            Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("bulletPrefab または shootPoint が設定されていません！");
        }
    }
}
