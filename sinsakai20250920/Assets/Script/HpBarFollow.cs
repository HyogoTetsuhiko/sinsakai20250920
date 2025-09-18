using UnityEngine;

public class HPBarFollow : MonoBehaviour
{
    public Transform player;     // プレイヤーの Transform
    public Vector3 offset = new Vector3(0, 1f, 0); // プレイヤーの頭上にずらす量

    void Update()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
        }
    }
}
