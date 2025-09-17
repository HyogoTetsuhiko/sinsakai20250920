using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;        // �㉺�ړ��X�s�[�h
    public GameObject bulletPrefab;    // �e�v���n�u
    public Transform shootPoint;       // ���ˈʒu�i�q�I�u�W�F�N�g��Empty��u���ƕ֗��j
    [SerializeField] float minY = -4f;
    [SerializeField] float maxY = 4f;
    private Rigidbody2D rb;
    private float moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // �㉺�ړ����́iW/S �܂��� ��/���j
        moveInput = Input.GetAxisRaw("Vertical");
        // �� Unity�W���� InputManager �� "Vertical" �� W/S �� ��/�� ���R�Â��Ă���

        // �U��
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(0f, moveInput * moveSpeed);

        // �ʒu����
        Vector2 clampedPos = rb.position;
        clampedPos.y = Mathf.Clamp(clampedPos.y, minY, maxY);
        rb.position = clampedPos;
    }

    void Shoot()
    {
        if (bulletPrefab != null && shootPoint != null)
        {
            Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("bulletPrefab �܂��� shootPoint ���ݒ肳��Ă��܂���I");
        }
    }
}
