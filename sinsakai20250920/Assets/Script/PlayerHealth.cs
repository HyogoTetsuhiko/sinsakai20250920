using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("HP�ݒ�")]
    public int maxHP = 5;             // �ő�HP
    private int currentHP;            // ���݂�HP

    [Header("UI�ݒ�")]
    public Slider hpSlider;           // HP�o�[��Slider
    public Image fillImage;           // HP�o�[��Fill����
    public Vector3 offset = new Vector3(0, 1.5f, 0); // �v���C���[����\���ʒu

    [Header("HP�o�[�F")]
    public Color fullColor = Color.green;
    public Color midColor = Color.yellow;
    public Color lowColor = Color.red;

    void Start()
    {
        currentHP = maxHP;            // HP������

        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
        }

        UpdateFillColor();            // �F��������
    }

    void Update()
    {
        // HP�o�[���v���C���[�̓���ɒǏ]
        if (hpSlider != null)
        {
            hpSlider.transform.position = transform.position + offset;
        }
    }

    // ----------------------------
    // �_���[�W����
    // ----------------------------
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP); // HP�͈͐���

        if (hpSlider != null)
        {
            hpSlider.value = currentHP;  // UI�X�V
        }

        UpdateFillColor();               // �F�X�V

        // HP��0�ɂȂ����玀�S����
        if (currentHP <= 0)
        {
            Die();
        }
    }

    // ----------------------------
    // HP�ɉ����ăo�[�̐F��ς���
    // ----------------------------
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

    // ----------------------------
    // �v���C���[���S����
    // ----------------------------
    void Die()
    {
        Debug.Log("�v���C���[���S�I�Q�[���I�[�o�[�V�[���ֈڍs");
        SceneManager.LoadScene("GameOverScene");
    }
}
