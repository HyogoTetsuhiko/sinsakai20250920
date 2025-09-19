using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public AudioClip clickSound;      // インスペクタで音を設定
    private AudioSource audioSource;

    void Awake()
    {
        // 同じGameObjectにAudioSourceがあれば取得、なければ追加
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // ボタンから呼ぶ関数
    public void PlayClickSound()
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound); // 効果音を1回だけ再生
        }
    }
}
