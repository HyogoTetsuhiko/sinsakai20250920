using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 3;
    private int currentHP;

    public Slider hpSlider; // UI��Slider���C���X�y�N�^�Őݒ�

    void Start()
    {
        currentHP = maxHP;
        hpSlider.maxValue = maxHP;
        hpSlider.value = currentHP;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        hpSlider.value = currentHP;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("�v���C���[���S�I");
    }
}
