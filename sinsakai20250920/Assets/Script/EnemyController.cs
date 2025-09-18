using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("�ړ��ƐڐG�_���[�W")]
    public float speed = 3f;          // �G�̈ړ����x
    public int damage = 1;            // �v���C���[�ɐڐG�����ۂ̃_���[�W��

    [Header("�e�̔��ːݒ�")]
    public GameObject bulletPrefab;   // �G�����˂���e�̃v���n�u
    public Transform shootPoint;      // �e�𔭎˂���ʒu�iEmpty�I�u�W�F�N�g�Ȃǁj
    public float shootInterval = 2f;  // �e�����Ԋu�i�b�j

    private float shootTimer = 0f;    // �e���˃^�C�}�[
    private Transform player;         // �v���C���[��Transform��ێ�

    void Start()
    {
        // �G�̃X�v���C�g��������̏ꍇ�A�������ɉ�]
        transform.rotation = Quaternion.Euler(0, 0, 90);

        // �V�[�����̃v���C���[��T����Transform���擾
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        // �������Ɉړ��i���[���h���W��j
        transform.Translate(Vector2.left * speed * Time.deltaTime, Space.World);

        // ��ʊO�ɏo���ꍇ�ɔj��
        CheckOffScreen();

        // �v���C���[�����݂���ꍇ�̂ݒe�𔭎�
        if (player != null)
        {
            shootTimer += Time.deltaTime;  // �^�C�}�[�X�V

            if (shootTimer >= shootInterval) // �Ԋu���o�߂����甭��
            {
                ShootAtPlayer();   // �v���C���[�����ɒe�𔭎�
                shootTimer = 0f;   // �^�C�}�[���Z�b�g
            }
        }
    }

    // �v���C���[�����ɒe�𔭎˂���֐�
    void ShootAtPlayer()
    {
        // �e�v���n�u�Ɣ��ˈʒu���ݒ肳��Ă��邩�m�F
        if (bulletPrefab != null && shootPoint != null)
        {
            // �e�𔭎ˈʒu�ɐ���
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

            // �v���C���[�����̃x�N�g�����v�Z���Đ��K��
            Vector2 dir = (player.position - shootPoint.position).normalized;

            // �e�̃X�N���v�g�ɕ������Z�b�g�iEnemyBullet�X�N���v�g���K�v�j
            bullet.GetComponent<EnemyBullet>()?.Init(dir);
        }
    }

    // �v���C���[��e�Ƃ̐ڐG����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �v���C���[�̒e�ɓ��������ꍇ
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject); // �e���폜
            Destroy(gameObject);           // �G���폜
            return;                        // �����I��
        }

        // �v���C���[�ɐڐG�����ꍇ
        if (collision.CompareTag("Player"))
        {
            // PlayerHealth �X�N���v�g���擾
            PlayerHealth ph = collision.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);     // �v���C���[�Ƀ_���[�W
            }

            Destroy(gameObject);           // �G���폜
        }
    }

    // �G����ʊO�ɏo���ꍇ�Ɏ����Ŕj������֐�
    void CheckOffScreen()
    {
        // �J�����̍��[�̃��[���h���W���擾
        float leftBound = Camera.main.ViewportToWorldPoint(Vector3.zero).x;

        // �G�̈ʒu�����[��肳��ɍ��ɏo����j��
        if (transform.position.x < leftBound - 1f)  // -1f�͗]�T����������
        {
            Destroy(gameObject); // ��ʊO�Ȃ̂ō폜
        }
    }
}
