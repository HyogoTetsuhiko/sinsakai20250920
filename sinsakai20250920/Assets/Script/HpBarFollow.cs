using UnityEngine;

public class HPBarFollow : MonoBehaviour
{
    public Transform player;     // �v���C���[�� Transform
    public Vector3 offset = new Vector3(0, 1f, 0); // �v���C���[�̓���ɂ��炷��

    void Update()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
        }
    }
}
