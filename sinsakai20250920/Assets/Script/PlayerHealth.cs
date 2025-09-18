using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("HP�ݒ�")]
    public int maxHP = 5;             // �v���C���[�̍ő�HP
    private int currentHP;            // ���݂�HP

    [Header("UI�ݒ�")]
    public Slider hpSlider;           // World Space Canvas ��� Slider
    public Image fillImage;           // Slider �� Fill Image
    public Vector3 offset = new Vector3(0, 1.5f, 0); // �v���C���[����ɕ\������I�t�Z�b�g

    [Header("HP�o�[�̐F")]
    public Color fullColor = Color.green;   // HP��50%�ȏ�̎��̐F
    public Color midColor = Color.yellow;   // HP��50%�ȉ��̎��̐F
    public Color lowColor = Color.red;      // HP��20%�ȉ��̎��̐F

    void Start()
    {
        // ����HP���ő�l�ɐݒ�
        currentHP = maxHP;

        // Slider ���ݒ肳��Ă���Ώ�����
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;      // Slider�̍ő�l��ݒ�
            hpSlider.value = currentHP;     // ���ݒl�𔽉f
        }

        // �����F��ݒ�
        UpdateFillColor();
    }

    void Update()
    {
        // World Space Canvas ��� HP�o�[���v���C���[�ɒǏ]������
        if (hpSlider != null)
        {
            hpSlider.transform.position = transform.position + offset;
        }
    }

    // �_���[�W���󂯂鏈��
    public void TakeDamage(int damage)
    {
        // ����HP�����炷
        currentHP -= damage;

        // HP��0������ő�HP�ȏ�ɂȂ�Ȃ��悤�ɐ���
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        // Slider �� HP �𔽉f
        if (hpSlider != null)
        {
            hpSlider.value = currentHP;
        }

        // HP�ɉ����ĐF���X�V
        UpdateFillColor();

        // HP��0�ɂȂ����ꍇ�̏���
        if (currentHP <= 0)
        {
            Die();
        }
    }

    // HP�o�[�̐F�� HP�����ɉ����Đ؂�ւ���
    void UpdateFillColor()
    {
        if (fillImage == null) return; // Fill Image �����ݒ�̏ꍇ�͉������Ȃ�

        // ����HP�̊������v�Z�i0�`1�j
        float ratio = (float)currentHP / maxHP;

        // HP�����ɉ����ĐF��؂�ւ���
        if (ratio <= 0.2f)
        {
            fillImage.color = lowColor;   // 20%�ȉ� �� ��
        }
        else if (ratio <= 0.5f)
        {
            fillImage.color = midColor;   // 50%�ȉ� �� ��
        }
        else
        {
            fillImage.color = fullColor;  // 50%�ȏ� �� ��
        }
    }

    // �v���C���[���S���̏���
    void Die()
    {
        Debug.Log("�v���C���[���S�I");
        // �Q�[���I�[�o�[�����⃊�X�|�[�������������ɒǉ��\
    }
}
