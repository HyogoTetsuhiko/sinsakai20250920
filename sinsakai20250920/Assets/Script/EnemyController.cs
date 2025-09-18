using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3f;

    void Start()
    {
        // �X�v���C�g��������̏ꍇ �� �������ɂȂ�悤�ɉ�]
        transform.rotation = Quaternion.Euler(0, 0, 90);
    }

    void Update()
    {
        // �������i���[���h���W��j�Ɉړ�
        transform.Translate(Vector2.left * speed * Time.deltaTime, Space.World);
    }

private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject); // �e���폜
            Destroy(gameObject);           // �G���폜
        }
    }
}
