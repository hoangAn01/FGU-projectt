using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkeletonHealth : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    public float maxHealth = 50f;
    public float currentHealth;
    public Image healthBarFill; // Kéo Image Fill vào đây trong Inspector

    public Animator animator;
    // Thêm biến prefab bình máu và tỉ lệ rơi
    public GameObject healthPotionPrefab;
    [Range(0f, 1f)]
    public float dropRate = 0.1f; // 10% tỉ lệ rơi

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

        Debug.Log("Player died!");
        animator.SetTrigger("Dead");
        StartCoroutine(WaitForDeathAnimation());
        // Tỉ lệ rơi bình máu
        if (healthPotionPrefab != null && Random.value < dropRate)
        {
            Instantiate(healthPotionPrefab, transform.position, Quaternion.identity);
        }
        // Tìm player và cộng điểm
        PlayerHealth player = FindObjectOfType<PlayerHealth>();
        if (player != null)
        {
            player.AddScore(1);
        }
    }

    private IEnumerator WaitForDeathAnimation()
    {
        // Giả sử animation chết dài 2 giây,
        
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
