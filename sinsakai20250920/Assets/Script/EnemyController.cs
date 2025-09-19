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

    [Header("�j�󉉏o�ݒ�")]
    public GameObject explosionPrefab; // �����G�t�F�N�gPrefab
    public AudioClip explosionSE;      // �������ʉ�
    public float explosionLifeTime = 1.5f; // �����G�t�F�N�g�̐�������

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

    // --- ���S���� ---
    void Die()
    {
        // �@ �����G�t�F�N�g�𐶐��i��莞�Ԍ�Ɏ����폜�j
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, explosionLifeTime); // �w��b��ɏ�����
        }

        // �A ���ʉ����Đ��iAudioSource�p�̈ꎞ�I�u�W�F�N�g�𐶐��j
        if (explosionSE != null)
        {
            GameObject audioObj = new GameObject("ExplosionSound");
            AudioSource tempSource = audioObj.AddComponent<AudioSource>();
            tempSource.PlayOneShot(explosionSE);
            Destroy(audioObj, explosionSE.length); // �Đ��I����ɍ폜
        }

        Destroy(gameObject); // �G�{�̂��폜
    }

    // --- �v���C���[�Ƃ̐ڐG���� ---
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �v���C���[�ɐڐG�����ꍇ
        if (collision.CompareTag("Player"))
        {
            PlayerHealth ph = collision.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);
            }

            Die(); // ���o�t���Ŕj��
        }

        // �v���C���[�e�ɓ��������ꍇ
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject); // �e���폜
            TakeDamage(1);                // �G�Ƀ_���[�W
        }
    }
}
