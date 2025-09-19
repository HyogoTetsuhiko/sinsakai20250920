using UnityEngine;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    [Header("�{�X�ݒ�")]
    public int maxHP = 50;               // �{�X�̍ő�HP
    private int currentHP;               // ����HP

    [Header("�e���ːݒ�")]
    public GameObject bulletPrefab;      // ���˂���e�̃v���n�u
    public Transform shootPoint;         // �e�̔��ˈʒu
    public float shootInterval = 2f;     // ���ˊԊu
    public int bulletCount = 5;          // ��x�Ɍ��e�̐�
    public float spreadAngle = 60f;      // ���ɍL����p�x

    private float shootTimer = 0f;       // ���˃^�C�}�[
    private bool isPhase2 = false;       // �t�F�[�Y2�ڍs����

    void Start()
    {
        // HP������
        currentHP = maxHP;

        // �{�X�̃X�v���C�g��������Ȃ獶�����ɉ�]
        transform.rotation = Quaternion.Euler(0f, 0f, 90f);

        // �o���ʒu����ʉE���ɌŒ�
        Vector3 pos = transform.position;
        pos.x = Camera.main.ViewportToWorldPoint(new Vector3(0.85f, 0.5f, 0)).x;
        transform.position = pos;
    }

    void Update()
    {
        // �e���˃^�C�}�[�X�V
        shootTimer += Time.deltaTime;

        // ���ˊԊu�𒴂�������ɒe������
        if (shootTimer >= shootInterval)
        {
            shootTimer = 0f;
            ShootFan();
        }
    }

    // --- ���ɕ����̒e�𔭎� ---
    void ShootFan()
    {
        if (bulletPrefab == null || shootPoint == null) return;

        // ��̍��[�̊p�x
        float startAngle = -spreadAngle / 2f;
        // �e���m�̊p�x��
        float angleStep = spreadAngle / (bulletCount - 1);

        for (int i = 0; i < bulletCount; i++)
        {
            // ���݂̒e�̊p�x
            float angle = startAngle + angleStep * i;

            // �{�X�̉�]���l�����Ēe�̊p�x��ݒ�
            // ����Ń{�X���������ł��e�����������
            Quaternion rot = Quaternion.Euler(0f, 0f, angle) * transform.rotation;

            // �e�𐶐�
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, rot);

            // Rigidbody2D ������Α��x��ݒ�
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // �e�͎��g�̏�����itransform.up�j�ɐi��
                rb.velocity = bullet.transform.up * 5f;
            }
        }
    }

    // --- �_���[�W���󂯂����� ---
    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        // HP�����ȉ��Ńt�F�[�Y2��
        if (!isPhase2 && currentHP <= maxHP / 2)
        {
            EnterPhase2();
        }

        // HP0�ȉ��Ō��j
        if (currentHP <= 0)
        {
            Die();
        }
    }

    void EnterPhase2()
    {
        isPhase2 = true;

        // �U�������i���ˊԊu�Z�k�A�e�����A��p�g��j
        shootInterval *= 0.5f;
        bulletCount += 2;
        spreadAngle += 20f;

        Debug.Log("�{�X �t�F�[�Y2�˓��I");
    }

    void Die()
    {
        Debug.Log("�{�X���j�I");
        SceneManager.LoadScene("ClearScene"); // �N���A�V�[����
    }

    // --- �Փ˔��� ---
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject); // �e�폜
            TakeDamage(1);                // �_���[�W��^����
        }
    }
}
