using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 5f;      // �e�̑��x
    public float lifeTime = 3f;   // �e�������ŏ�����܂ł̎���

    private Vector2 moveDirection; // �e�̈ړ�����

    // �e�����˂����Ƃ��ɕ�����ݒ�
    public void Init(Vector2 direction)
    {
        moveDirection = direction.normalized; // �P�ʃx�N�g���ɐ��K��
    }

    void Start()
    {
        // lifeTime �b��ɒe�������j��
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // ���t���[���A�ݒ肵�������ɑ��x���|���Ĉړ�
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �v���C���[�ɓ���������_���[�W��^����
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1); // �_���[�W�ʂ��w��
            }

            Destroy(gameObject); // �e��j��
        }
    }
}
