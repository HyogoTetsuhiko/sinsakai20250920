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
    private bool isPhase2 = false;       // HP�����ōs���ω�

    [Header("�j�󉉏o�ݒ�")]
    public GameObject explosionPrefab;   // �����G�t�F�N�gPrefab
    public AudioClip explosionSE;        // �������ʉ�
    public int explosionCount = 5;       // �o�������̐�
    public float explosionRadius = 2f;   // �������o���͈�
    public float explosionLifeTime = 1.5f; // �G�t�F�N�g���c�鎞�ԁi�b�j
    public float sceneChangeDelay = 2f;  // ���o��ɃV�[���J�ڂ���܂ł̑҂�����

    void Start()
    {
        currentHP = maxHP;

        // ������X�v���C�g���������ɉ�]
        transform.rotation = Quaternion.Euler(0f, 0f, 90f);

        // �o���ʒu����ʉE�O�ɐݒ�i�G���X���C�h���ē����Ă��鉉�o�j
        Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(1.2f, 0.5f, 0));
        pos.z = 0;
        transform.position = pos;

        // ��ʒu�i��ʉE�[���班�����j��ݒ�
        targetPosition = Camera.main.ViewportToWorldPoint(new Vector3(0.85f, 0.5f, 0));
        targetPosition.z = 0;
    }

    void Update()
    {
        // �ړ�����
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
            // �e���ˏ���
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootInterval)
            {
                shootTimer = 0f;
                ShootFan();
            }
        }
    }

    // ���ɒe��������
    void ShootFan()
    {
        if (bulletPrefab == null || shootPoint == null) return;

        // ���̊J�n�p�x�Ɗp�x�Ԋu���v�Z
        float startAngle = -spreadAngle / 2f;
        float angleStep = spreadAngle / (bulletCount - 1);

        // bulletCount �̐������e�𐶐�
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + angleStep * i;

            // �{�X�̌��݂̉�]���l�����Ĕ��˕���������
            Quaternion rot = Quaternion.Euler(0f, 0f, angle) * transform.rotation;

            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, rot);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.velocity = bullet.transform.up * 5f; // forward/up�����ɑ��x��^����
        }
    }

    // �_���[�W���󂯂鏈��
    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        // HP�������ȉ��ɂȂ������2�t�F�[�Y��
        if (!isPhase2 && currentHP <= maxHP / 2)
            EnterPhase2();

        // HP��0�ȉ��ɂȂ����猂�j
        if (currentHP <= 0)
            Die();
    }

    // �t�F�[�Y2�˓�����
    void EnterPhase2()
    {
        isPhase2 = true;
        shootInterval *= 0.5f;  // ���ˊԊu��Z�k
        bulletCount += 2;       // ���e�̐��𑝉�
        spreadAngle += 20f;     // ��̊p�x���L����
        Debug.Log("�{�X �t�F�[�Y2�˓��I");
    }

    // ====================
    // �{�X���j����
    // ====================
    void Die()
    {
        Debug.Log("�{�X���j�I");

        // Coroutine ���g���āA���j���̉��o�i�����E���ʉ��E�ҋ@�E�V�[���J�ځj�����Ԃɏ���
        StartCoroutine(DieRoutine());
    }

    private System.Collections.IEnumerator DieRoutine()
    {
        // �����G�t�F�N�g
        if (explosionPrefab != null)
        {
            for (int i = 0; i < explosionCount; i++)
            {
                // �{�X���͂̃����_���ʒu�ɔ����G�t�F�N�g�𐶐�
                Vector3 randomOffset = new Vector3(
                    Random.Range(-explosionRadius, explosionRadius), // X���������_��
                    Random.Range(-explosionRadius, explosionRadius), // Y���������_��
                    0f // Z��2D�Ȃ̂�0
                );

                // �G�t�F�N�g�𐶐�
                GameObject explosion = Instantiate(
                    explosionPrefab,           // �v���n�u
                    transform.position + randomOffset, // �{�X�ʒu�Ƀ����_���I�t�Z�b�g
                    Quaternion.identity        // ��]�Ȃ�
                );

                // ��莞�Ԍ�Ɏ����Ŕj�󂵂ă��������
                Destroy(explosion, explosionLifeTime);
            }
        }

        // ���ʉ��Đ�
        if (explosionSE != null)
        {
            // AudioSource��V�K���������Ƀ����V���b�g�ōĐ�
            // AudioSource.PlayClipAtPoint ��3D��Ԃ̈ʒu�ɉ���炷
            AudioSource.PlayClipAtPoint(explosionSE, transform.position);
        }

        // ��莞�ԑҋ@
        // WaitForSecondsRealtime �� Time.timeScale �̉e�����󂯂Ȃ�
        // �i�Q�[�����ꎞ��~���ł��ҋ@���Ԃ����m�Ɍo�߁j
        yield return new WaitForSecondsRealtime(sceneChangeDelay);

        // �V�[���J��
        // ClearScene �ɑJ��
        // ���̎��_�Ń{�X�I�u�W�F�N�g�͔j������邩�A�V�[���ړ��Ŏ����j�������
        SceneManager.LoadScene("ClearScene");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���������I�u�W�F�N�g�� "Bullet" �^�O���m�F
        if (collision.CompareTag("Bullet"))
        {
            // �e��j��
            Destroy(collision.gameObject);

            // �{�X�Ƀ_���[�W
            TakeDamage(1);
        }
    }
}