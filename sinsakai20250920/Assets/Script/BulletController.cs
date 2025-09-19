using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [Header("�e�̐ݒ�")]
    public float speed = 10f;      // �e�̑��x
    public float lifeTime = 2f;    // �e�̎����i�b�j
    public int damage = 1;         // �G�ɗ^����_���[�W��

    void Start()
    {
        // lifeTime �b��Ɏ����j��
        Destroy(gameObject, lifeTime);

        // �e�̃X�v���C�g���E�����ɉ�]
        transform.rotation = Quaternion.Euler(0f, 0f, 90f);
    }

    void Update()
    {
        // �e���E�����Ɉړ�
        transform.position += Vector3.right * speed * Time.deltaTime;
    }

    // �G�Ƃ̏Փ˔���
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �ՓˑΏۂ� Enemy �^�O�Ȃ�_���[�W����
        if (collision.CompareTag("Enemy"))
        {
            // EnemyController �X�N���v�g���擾���ă_���[�W
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            // �e��j��
            Destroy(gameObject);
        }
    }
}
