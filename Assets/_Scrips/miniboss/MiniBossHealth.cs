using UnityEngine;
using UnityEngine.UI;

public class MiniBossHealth : MonoBehaviour, IDamageable
{
    public float maxHealth = 100;
    public float currentHealth;
    public Image healthBarFill; // Kéo Image Fill vào đây trong Inspector

    // Thêm biến prefab bình máu và tỉ lệ rơi
    public GameObject healthPotionPrefab;
    [Range(0f, 1f)]
    public float dropRate = 0.5f; // 50% tỉ lệ rơi

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    private bool isDead = false;

    public void TakeDamage(float amount)
    {
        if (isDead) return;
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
        isDead = true;
        // Tỉ lệ rơi bình máu
        if (healthPotionPrefab != null && Random.value < dropRate)
        {
            Instantiate(healthPotionPrefab, transform.position, Quaternion.identity);
        }
        // Tìm player và cộng điểm
        PlayerHealth player = FindObjectOfType<PlayerHealth>();
        if (player != null)
        {
            player.AddScore(10);
        }
        Destroy(gameObject);
    }
}