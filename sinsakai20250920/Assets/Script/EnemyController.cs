using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("HP�ݒ�")]
    public int maxHP = 3;
    private int currentHP;

    [Header("�ړ��ݒ�")]
    public float speed = 3f;

    [Header("�e���ːݒ�")]
    public GameObject bulletPrefab;   // �e�̃v���n�u
    public Transform shootPoint;      // �e�����ʒu
    public float shootInterval = 2f;  // ���ˊԊu

    [Header("�v���C���[�ڐG�ݒ�")]
    public int damage = 1;            // �v���C���[�ɗ^����_���[�W��

    private float shootTimer = 0f;

    void Start()
    {
        currentHP = maxHP;

        // �X�v���C�g��������̏ꍇ�A�������ɉ�]
        transform.rotation = Quaternion.Euler(0f, 0f, 90f);
    }

    void Update()
    {
        // �������Ɉړ�
        transform.Translate(Vector2.left * speed * Time.deltaTime, Space.World);

        // �e���ˏ���
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            shootTimer = 0f;
            Shoot();
        }
    }

    // --- �e�𔭎� ---
    void Shoot()
    {
        if (bulletPrefab == null || shootPoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // �e�̌����ɉ����Ĕ�΂�
            rb.velocity = bullet.transform.up * 5f;
        }
    }

    // --- �_���[�W���� ---
    public void TakeDamage(int damageAmount)
    {
        currentHP -= damageAmount;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        // �K�v�ł���΃X�R�A���Z��G�t�F�N�g����
    }

    // --- �v���C���[�Ƃ̐ڐG���� ---
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �v���C���[�ɐڐG�����ꍇ
        if (collision.CompareTag("Player"))
        {
            // PlayerHealth �X�N���v�g���擾
            PlayerHealth ph = collision.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);  // �v���C���[�Ƀ_���[�W
            }

            Destroy(gameObject); // �ڐG�����G���폜
        }

        // �v���C���[�e�ɓ��������ꍇ
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject); // �e���폜
            TakeDamage(1);                // �G�Ƀ_���[�W
        }
    }
}
