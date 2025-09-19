using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("�ړ��ݒ�")]
    public float moveSpeed = 5f;        // �㉺�ړ��X�s�[�h
    [SerializeField] float minY = -4f;
    [SerializeField] float maxY = 4f;

    [Header("�U���ݒ�")]
    public GameObject bulletPrefab;     // �e�v���n�u
    public Transform shootPoint;        // ���ˈʒu�i�q�I�u�W�F�N�g�j

    [Header("���ʉ��ݒ�")]
    public AudioClip shootSE;           // �e���ˉ�
    private AudioSource audioSource;    // AudioSource�Q��

    private Rigidbody2D rb;
    private float moveInput;

    // HP�Ǘ��p�R���|�[�l���g
    private PlayerHealth playerHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        // PlayerHealth ���擾
        playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth �R���|�[�l���g���A�^�b�`����Ă��܂���I");
        }
    }

    void Update()
    {
        // �㉺�ړ����͎擾
        moveInput = Input.GetAxisRaw("Vertical");

        // �U��
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        // �㉺�ړ�
        rb.velocity = new Vector2(0f, moveInput * moveSpeed);

        // �ړ��͈͂̐���
        Vector2 clampedPos = rb.position;
        clampedPos.y = Mathf.Clamp(clampedPos.y, minY, maxY);
        rb.position = clampedPos;
    }

    void Shoot()
    {
        if (bulletPrefab != null && shootPoint != null)
        {
            // �e�𐶐��i��]�Ȃ��j
            Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            // ���ˉ���炷����
            if (shootSE != null && audioSource != null)
            {
                audioSource.PlayOneShot(shootSE);
            }
        }
        else
        {
            Debug.LogWarning("bulletPrefab �܂��� shootPoint ���ݒ肳��Ă��܂���I");
        }
    }

    // ----------------------------
    // �G��G�e�Ƃ̏Փ˔���
    // ----------------------------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerHealth == null) return;

        // �G�e�ɐڐG�����ꍇ
        if (collision.CompareTag("EnemyBullet"))
        {
            Destroy(collision.gameObject);       // �e���폜
            playerHealth.TakeDamage(1);         // PlayerHealth�Ƀ_���[�W���Ϗ�
        }

        // �G�ɐڐG�����ꍇ�i�ڐG�_���[�W�j
        if (collision.CompareTag("Enemy"))
        {
            playerHealth.TakeDamage(1);         // PlayerHealth�Ƀ_���[�W���Ϗ�
        }
    }
}
