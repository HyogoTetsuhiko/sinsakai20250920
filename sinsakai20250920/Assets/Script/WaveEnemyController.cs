using UnityEngine;

public class WaveEnemyController : MonoBehaviour
{
    [Header("�ړ��ݒ�")]
    public float speed = 3f;            // �������ւ̑O�i���x
    public float waveAmplitude = 1f;    // �㉺�h��̐U��
    public float waveFrequency = 2f;    // �㉺�h��̑����i���g���j

    private Vector3 startPosition;      // �o�����̈ʒu��ێ�
    private float elapsedTime = 0f;     // �o�ߎ���

    [Header("HP�ݒ�")]
    public int maxHP = 3;               // �G�̍ő�HP
    private int currentHP;              // ���݂�HP

    [Header("�ڐG�_���[�W")]
    public int contactDamage = 1;       // �v���C���[�ɐڐG�����Ƃ��̃_���[�W��

    [Header("�U���ݒ�")]
    public GameObject bulletPrefab;     // �G�����e�̃v���n�u
    public Transform shootPoint;        // �e�̔��ˈʒu�i�G�̌���C�䕔���j
    public float shootInterval = 2f;    // �e�����Ԋu�i�b�j
    public float bulletSpeed = 5f;      // �e�̈ړ����x

    private float shootTimer = 0f;      // �e���˗p�^�C�}�[

    void Start()
    {
        // HP��������
        currentHP = maxHP;

        // �o�����̈ʒu��ۑ��i�g���̊�ɂȂ�j
        startPosition = transform.position;
    }

    void Update()
    {
        // --- �������ɑO�i ---
        transform.position += Vector3.left * speed * Time.deltaTime;

        // --- �g���i�㉺�ړ��j ---
        elapsedTime += Time.deltaTime;
        float yOffset = Mathf.Sin(elapsedTime * waveFrequency) * waveAmplitude;
        transform.position = new Vector3(transform.position.x, startPosition.y + yOffset, transform.position.z);

        // --- �e���˃^�C�}�[ ---
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            shootTimer = 0f;
            ShootAtPlayer(); // �v���C���[�Ɍ������Ēe�𔭎�
        }

        // --- ��ʊO����i���[��ʉ߂�����폜�j ---
        if (Camera.main != null)
        {
            // ���[���h���W�̍��[���擾
            float leftBound = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;

            // �G�̉E�[�����[��荶�ɏo����폜
            if (transform.position.x + 0.5f < leftBound) // 0.5f�͓G�̔�������
            {
                Destroy(gameObject);
            }
        }
    }

    // --- �v���C���[�Ɍ������Ēe�𔭎˂��鏈�� ---
    void ShootAtPlayer()
    {
        // �ePrefab�܂��͔��ˈʒu���ݒ肳��Ă��Ȃ���Ή������Ȃ�
        if (bulletPrefab == null || shootPoint == null) return;

        // �V�[������Player�I�u�W�F�N�g���擾
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return; // Player�����݂��Ȃ���Δ��˂��Ȃ�

        // �e�𐶐�
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

        // �v���C���[�����ւ̒P�ʃx�N�g�����v�Z
        Vector2 direction = (player.transform.position - shootPoint.position).normalized;

        // Rigidbody2D������Α��x��ݒ�
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
        }

        // �e�̌������v���C���[�����ɉ�]
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // --- �_���[�W���󂯂鏈�� ---
    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        // HP��0�ȉ��ɂȂ�����j��
        if (currentHP <= 0)
        {
            Die();
        }
    }

    // --- �G�j�󏈗� ---
    void Die()
    {
        Destroy(gameObject);
        // �X�R�A���Z��j��G�t�F�N�g�������ɒǉ��\
    }

    // --- �Փ˔��� ---
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �v���C���[�ƐڐG�����ꍇ
        if (collision.CompareTag("Player"))
        {
            PlayerHealth player = collision.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(1); // �_���[�W�ʂ͒���
            }

            Destroy(gameObject); // �e��j��
        }
    
        // �v���C���[�̒e�ƐڐG�����ꍇ
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject); // �e���폜
            TakeDamage(2);                 // �G���_���[�W���󂯂�
        }
    }
}
