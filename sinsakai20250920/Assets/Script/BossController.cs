using UnityEngine;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    [Header("�{�X�ݒ�")]
    public int maxHP = 50;               // �{�X�̍ő�HP
    private int currentHP;               // ����HP

    [Header("�ړ��ݒ�")]
    public Vector3 targetPosition;       // �{�X���~�܂�ʒu
    public float moveSpeed = 3f;         // �ړ����x
    private bool reachedPosition = false;// ��ʒu���B����

    [Header("�e���ːݒ�")]
    public GameObject bulletPrefab;      // �e�̃v���n�u
    public Transform shootPoint;         // �e�̔��ˈʒu
    public float shootInterval = 2f;     // ���ˊԊu
    public int bulletCount = 5;          // ���Ɍ��e�̐�
    public float spreadAngle = 60f;      // ��̊p�x

    private float shootTimer = 0f;
    private bool isPhase2 = false;

    void Start()
    {
        currentHP = maxHP;

        // ������X�v���C�g���������ɉ�]
        transform.rotation = Quaternion.Euler(0f, 0f, 90f);

        // �o���ʒu����ʉE���ɐݒ�
        Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(1.2f, 0.5f, 0)); // ��ʊO�E����o��
        pos.z = 0;
        transform.position = pos;

        // ��ʒu�i��ʉE�[���班�����j��ݒ�
        targetPosition = Camera.main.ViewportToWorldPoint(new Vector3(0.85f, 0.5f, 0));
        targetPosition.z = 0;
    }

    void Update()
    {
        // --- �ړ����� ---
        if (!reachedPosition)
        {
            // �X�|�[���ʒu����^�[�Q�b�g�ʒu�܂Ő��`��Ԃňړ�
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // �^�[�Q�b�g�ʒu�ɓ��B������~�܂�
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                reachedPosition = true;
            }
        }
        else
        {
            // --- �e���ˏ��� ---
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootInterval)
            {
                shootTimer = 0f;
                ShootFan();
            }
        }
    }

    // ���ɒe������
    void ShootFan()
    {
        if (bulletPrefab == null || shootPoint == null) return;

        float startAngle = -spreadAngle / 2f;
        float angleStep = spreadAngle / (bulletCount - 1);

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + angleStep * i;

            // �{�X��]���l��
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
        Debug.Log("�{�X �t�F�[�Y2�˓��I");
    }

    void Die()
    {
        Debug.Log("�{�X���j�I");
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
