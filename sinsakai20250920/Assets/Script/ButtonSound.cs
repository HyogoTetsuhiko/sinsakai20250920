using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public AudioClip clickSound;      // �C���X�y�N�^�ŉ���ݒ�
    private AudioSource audioSource;

    void Awake()
    {
        // ����GameObject��AudioSource������Ύ擾�A�Ȃ���Βǉ�
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // �{�^������ĂԊ֐�
    public void PlayClickSound()
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound); // ���ʉ���1�񂾂��Đ�
        }
    }
}
