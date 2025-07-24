using UnityEngine;
using UnityEngine.UI;

public class BoarHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public Image healthBarFill; // K�o Image Fill v�o ?�y trong Inspector

    // Th�m bi?n prefab b�nh m�u v� t? l? r?i
    public GameObject healthPotionPrefab;
    [Range(0f, 1f)]
    public float dropRate = 0.5f; // 50% t? l? r?i
    private Animator animator;
    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float amount)
    {
        Debug.Log("chem thanh cong");
        currentHealth -= amount;
        if (animator != null)
        {
            animator.SetTrigger("hit");
        }
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
        // T? l? r?i b�nh m�u
        if (healthPotionPrefab != null && Random.value < dropRate)
        {
            Instantiate(healthPotionPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);

        // T�m player v� c?ng ?i?m
        PlayerHealth player = FindObjectOfType<PlayerHealth>();
        if (player != null)
        {
            player.AddScore(1);
        }
    }
}