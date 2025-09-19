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

    [Header("�j�󉉏o�ݒ�")]
    public GameObject explosionPrefab;   // �����G�t�F�N�gPrefab
    public AudioClip explosionSE;        // �������ʉ�
    public float explosionLifeTime = 1.5f; // �G�t�F�N�g���c�鎞�ԁi�b�j

    void Start()
    {
        currentHP = maxHP;
        startPosition = transform.position;
    }

    void Update()
    {
        // �������Ɉړ�
        transform.position += Vector3.left * speed * Time.deltaTime;

        // �g���i�㉺�ړ��j
        elapsedTime += Time.deltaTime;
        float yOffset = Mathf.Sin(elapsedTime * waveFrequency) * waveAmplitude;
        transform.position = new Vector3(transform.position.x, startPosition.y + yOffset, transform.position.z);

        // �e����
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            shootTimer = 0f;
            ShootAtPlayer();
        }

        // ��ʊO�ō폜
        if (Camera.main != null)
        {
            float leftBound = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
            if (transform.position.x + 0.5f < leftBound)
            {
                Destroy(gameObject);
            }
        }
    }

    // �v���C���[�Ɍ������Ēe��������
    void ShootAtPlayer()
    {
        // �e��Prefab�����ˈʒu�����ݒ�Ȃ牽�����Ȃ�
        if (bulletPrefab == null || shootPoint == null) return;

        // �V�[��������"Player"�^�O�̂����I�u�W�F�N�g��T��
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return; // �v���C���[�����Ȃ���Δ��˂��Ȃ�

        // �e�𐶐��i���ˈʒu�ɐ����A��]�͏����l�j
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

        // �v���C���[�̕������v�Z���ĒP�ʃx�N�g���ɕϊ�
        Vector2 direction = (player.transform.position - shootPoint.position).normalized;

        // �e��Rigidbody2D������Ȃ�A���x��ݒ肵�Ĕ���
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed; // �e���������Đi�s����������
        }

        // �e�̌������v���C���[�����ɉ�]������i�����ڂ����R�ɂȂ�j
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // �_���[�W���󂯂��Ƃ��̏���
    public void TakeDamage(int damage)
    {
        // ���݂�HP����_���[�W��������
        currentHP -= damage;

        // HP��0�ȉ��Ȃ玀�S�������Ăяo��
        if (currentHP <= 0)
        {
            Die();
        }
    }


    // �j�󏈗�
    void Die()
    {
        // �����G�t�F�N�g�𐶐��i��莞�Ԍ�Ɏ����폜�j
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, explosionLifeTime); // �w��b��ɍ폜
        }

        // ���ʉ����Đ��i�ʃI�u�W�F�N�g�ōĐ����Ă���폜�j
        if (explosionSE != null)
        {
            GameObject audioObj = new GameObject("ExplosionSound");
            AudioSource tempSource = audioObj.AddComponent<AudioSource>();
            tempSource.PlayOneShot(explosionSE);
            Destroy(audioObj, explosionSE.length);
        }

        Destroy(gameObject); // �G�{�̂��폜
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth player = collision.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(contactDamage);
            }

            Die(); // ����Destroy�ł͂Ȃ��A���o�t���j��
        }

        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            TakeDamage(2);
        }
    }
}
