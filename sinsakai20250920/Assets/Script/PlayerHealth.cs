using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class PlayerHealth : MonoBehaviour
{
    [Header("HP�ݒ�")]
    public int maxHP = 5;             // �v���C���[�̍ő�HP
    private int currentHP;            // ���݂�HP

    [Header("UI�ݒ�")]
    public Slider hpSlider;           // World Space Canvas ��� HP�o�[�iSlider�j
    public Image fillImage;           // Slider �� Fill Image ����
    public Vector3 offset = new Vector3(0, 1.5f, 0); // ����ɕ\������ʒu

    [Header("HP�o�[�̐F")]
    public Color fullColor = Color.green;   // HP50%�ȏ�
    public Color midColor = Color.yellow;   // HP50%�ȉ�
    public Color lowColor = Color.red;      // HP20%�ȉ�

    void Start()
    {
        // �Q�[���J�n���� HP ���ő�l�ɐݒ�
        currentHP = maxHP;

        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
        }

        UpdateFillColor();
    }

    void Update()
    {
        // HP�o�[���v���C���[�̓���ɒǏ]
        if (hpSlider != null)
        {
            hpSlider.transform.position = transform.position + offset;
        }
    }

    // �_���[�W���󂯂鏈��
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        if (hpSlider != null)
        {
            hpSlider.value = currentHP;
        }

        UpdateFillColor();

        if (currentHP <= 0)
        {
            Die();
        }
    }

    // HP�����ɉ����ĐF��ς���
    void UpdateFillColor()
    {
        if (fillImage == null) return;

        float ratio = (float)currentHP / maxHP;

        if (ratio <= 0.2f)
            fillImage.color = lowColor;
        else if (ratio <= 0.5f)
            fillImage.color = midColor;
        else
            fillImage.color = fullColor;
    }

    // �v���C���[���S���̏���
    void Die()
    {
        Debug.Log("�v���C���[���S�I �Q�[���I�[�o�[�V�[���Ɉڍs���܂�");

        // �� �Q�[���I�[�o�[�V�[���֐؂�ւ�
        SceneManager.LoadScene("GameOverScene");
    }
}
