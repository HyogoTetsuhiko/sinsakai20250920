using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 2f; // �X�N���[�����x
    float width; // �w�i�摜�̉���

    void Start()
    {
        // SpriteRenderer ���牡���������擾
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        width = sr.bounds.size.x;
    }

    void Update()
    {
        // ���Ɉړ�
        transform.Translate(Vector2.left * scrollSpeed * Time.deltaTime);

        // ���[�ɏ�������E�[�ɍĔz�u
        if (transform.position.x < -width)
        {
            Vector2 newPos = new Vector2(transform.position.x + 2 * width, transform.position.y);
            transform.position = newPos;
        }
    }
}
