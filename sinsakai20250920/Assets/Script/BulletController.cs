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
        // 右方向に移動
        transform.position += Vector3.right * speed * Time.deltaTime;
        // スプライトを右向きに
        transform.rotation = Quaternion.Euler(0f, 0f, 90f);
    }
}
