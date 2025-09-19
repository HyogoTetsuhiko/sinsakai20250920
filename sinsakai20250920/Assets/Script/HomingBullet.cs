using UnityEngine;

public class HomingBullet : MonoBehaviour
{
    public float speed = 5f;             // �e�̑��x
    public float rotateSpeed = 200f;     // �e�̉�]���x�i�ǔ��̑����j
    public float lifeTime = 5f;          // �e�̎���

    private Transform target;            // �ǔ��Ώ�

    void Start()
    {
        // 5�b��Ɏ����폜
        Destroy(gameObject, lifeTime);

        // Player�^�O�����I�u�W�F�N�g��T��
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
        }
    }

    void Update()
    {
        if (target == null)
        {
            // �^�[�Q�b�g�����Ȃ���Β��i
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            return;
        }

        // �ǔ������̃x�N�g�����v�Z
        Vector2 direction = (Vector2)(target.position - transform.position);
        direction.Normalize();

        // �e�̌������X���[�Y�Ƀ^�[�Q�b�g�����ɉ�]
        float rotateAmount = Vector3.Cross(direction, transform.right).z;
        transform.Rotate(0, 0, -rotateAmount * rotateSpeed * Time.deltaTime);

        // �e��O�i�i�E�������O�����j
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �v���C���[�ɐڐG������_���[�W
        if (collision.CompareTag("Player"))
        {
            PlayerHealth player = collision.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(1); // �_���[�W�ʂ͒���
            }

            Destroy(gameObject); // �e��j��
        }
    }
}
