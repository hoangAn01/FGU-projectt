using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FireBossHealth : MonoBehaviour, IDamageable
{
    public enum BossState { Alive, Dead }
    [Header("Health Settings")]
    public float maxHealth = 300;
    public float currentHealth;
    public Image healthBarFill;
    public GameObject healthPotionPrefab;
    [Range(0f, 1f)]
    public float dropRate = 0.7f;

    public Animator animator;
    public BossState bossState = BossState.Alive;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float amount)
    {
        if (bossState == BossState.Dead) return;
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
        Debug.Log("FireBoss died!");
        bossState = BossState.Dead;
        animator.SetBool("isDead", true);
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
            player.AddScore(50);
        }
    }
    private IEnumerator WaitForDeathAnimation()
    {
        yield return new WaitForSeconds(3.2f);
        Destroy(gameObject);
    }
} 