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

    private float shootTimer = 0f;

    void Start()
    {
        currentHP = maxHP;

        // �G�X�v���C�g��������̏ꍇ�A�������ɉ�]
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

    void Shoot()
    {
        if (bulletPrefab == null || shootPoint == null) return;

        // �e�𐶐�
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

        // Rigidbody2D ������Βe��i�܂���
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = bullet.transform.up * 5f;  // �������ɔ�΂�
        }
    }

    // �_���[�W����
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        // �X�R�A���Z��G�t�F�N�g�����������ŉ\
    }
}
