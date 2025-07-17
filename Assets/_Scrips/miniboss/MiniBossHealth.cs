using UnityEngine;
using UnityEngine.UI;

public class MiniBossHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public Image healthBarFill; // Kéo Image Fill vào đây trong Inspector

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float amount)
    {
        Debug.Log("chem thanh cong");
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;
        UpdateHealthBar();

        if (currentHealth == 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        if (healthBarFill != null)
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
    }

    void Die()
    {
        // Xử lý khi quái chết (animation, destroy, v.v.)
        Destroy(gameObject);
    }
}