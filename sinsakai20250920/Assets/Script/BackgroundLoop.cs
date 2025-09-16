using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 2f; // スクロール速度
    float width; // 背景画像の横幅

    void Start()
    {
        // SpriteRenderer から横幅を自動取得
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        width = sr.bounds.size.x;
    }

    void Update()
    {
        // 左に移動
        transform.Translate(Vector2.left * scrollSpeed * Time.deltaTime);

        // 左端に消えたら右端に再配置
        if (transform.position.x < -width)
        {
            Vector2 newPos = new Vector2(transform.position.x + 2 * width, transform.position.y);
            transform.position = newPos;
        }
    }
}
