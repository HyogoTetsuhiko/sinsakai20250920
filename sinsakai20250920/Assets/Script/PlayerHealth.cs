using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("HP�ݒ�")]
    public int maxHP = 5;   // �v���C���[�ő�HP
    private int currentHP;  // ����HP

    [Header("UI�ݒ�")]
    public Slider hpSlider;           // ����ɕ\������HP�o�[
    public Image fillImage;           // HP�o�[�̐F����
    public Vector3 offset = new Vector3(0, 1.5f, 0); // HP�o�[�̕\���ʒu

    [Header("HP�o�[�̐F")]
    public Color fullColor = Color.green;   // HP�������Ƃ�
    public Color midColor = Color.yellow;   // HP�������ȉ�
    public Color lowColor = Color.red;      // HP�����Ȃ��Ƃ�

    [Header("���S���o")]
    public GameObject explosionPrefab; // ���S���ɏo�������G�t�F�N�g��Prefab

    // �������ݒ�
    [Header("���ʉ��ݒ�")]
    public AudioClip explosionSE;     // ������
    private AudioSource audioSource;  // ���ʉ��Đ��p

    void Start()
    {
        // �Q�[���J�n����HP���ő�l�ɃZ�b�g
        currentHP = maxHP;

        // HP�o�[���ݒ肳��Ă���Ώ����l�𔽉f
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
        }

        // HP�ɉ����ăo�[�̐F���X�V
        UpdateFillColor();

        // AudioSource ���擾
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource �� Player �ɃA�^�b�`����Ă��܂���B���ʉ��͍Đ�����܂���B");
        }
    }

    void Update()
    {
        // HP�o�[���v���C���[�̈ʒu�ɒǏ]������
        if (hpSlider != null)
        {
            hpSlider.transform.position = transform.position + offset;
        }
    }

    // �_���[�W���󂯂鏈��
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP); // HP��0�����ɂȂ�Ȃ��悤�ɐ���

        // HP�o�[���ݒ肳��Ă���Δ��f
        if (hpSlider != null)
        {
            hpSlider.value = currentHP;
        }

        // �F���X�V
        UpdateFillColor();

        // HP��0�ɂȂ����玀�S������
        if (currentHP <= 0)
        {
            StartCoroutine(Die()); // �R���[�`���Ŕ������o �� �V�[���ڍs
        }
    }

    // HP�o�[�̐F������HP�ɉ����čX�V
    void UpdateFillColor()
    {
        if (fillImage == null) return;

        float ratio = (float)currentHP / maxHP; // HP����

        if (ratio <= 0.2f)
            fillImage.color = lowColor;   // HP 20%�ȉ�
        else if (ratio <= 0.5f)
            fillImage.color = midColor;   // HP 50%�ȉ�
        else
            fillImage.color = fullColor;  // HP 50%�ȏ�
    }

    // �v���C���[���S���̉��o�ƃV�[���ڍs
    IEnumerator Die()
    {
        Debug.Log("�v���C���[���S�I �����A�j���[�V�����Đ�");

        // �����G�t�F�N�g���Đ�
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // �ǉ������F���������Đ�
        if (explosionSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(explosionSE);
        }

        // �ҋ@�i�G�t�F�N�g�Ɖ����Đ����鎞�ԁj
        yield return new WaitForSeconds(1.2f);

        // �v���C���[������
        gameObject.SetActive(false);

        // �Q�[���I�[�o�[�V�[���ֈڍs
        SceneManager.LoadScene("GameOverScene");
    }
}
