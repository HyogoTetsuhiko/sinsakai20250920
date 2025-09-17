using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // �E�����Ɉړ�
        transform.position += Vector3.right * speed * Time.deltaTime;
        // �X�v���C�g���E������
        transform.rotation = Quaternion.Euler(0f, 0f, 90f);
    }
}
