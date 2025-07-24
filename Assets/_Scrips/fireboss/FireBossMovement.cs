using UnityEngine;

public class FireBossMovement : MonoBehaviour
{
    [Header("AI Settings")]
    public float detectRange = 10f; // tầm phát hiện
    public float attackRange = 2f;  // tầm đánh gần
    public float moveSpeed = 2f;
    public float attackCooldown = 3f + 2f;// 3 giây attack + X giây cooldown
    public int attackDamage = 20;

    private Transform player;
    private Animator animator;
    private Rigidbody2D rb;
    private float attackTimer = 0f;
    private bool isFacingRight = true;
    private FireBossHealth health;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<FireBossHealth>();
        if (animator != null)
        {
            animator.SetBool("isMove", false);
            animator.SetBool("isAttack", false);
        }
    }

    void Update()
    {
        if (health != null && health.bossState == FireBossHealth.BossState.Dead)
        {
            if (rb != null)
                rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }
        if (player == null)
        {
            if (animator != null)
            {
                animator.SetBool("isMove", false);
                animator.SetBool("isAttack", false);
            }
            if (rb != null)
                rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectRange)
        {
            HandleDetection(distance);
        }
        else
        {
            HandleOutOfRange();
        }
    }

    void HandleDetection(float distance)
    {
        // Lật hướng
        if (player.position.x > transform.position.x && !isFacingRight)
            Flip();
        else if (player.position.x < transform.position.x && isFacingRight)
            Flip();

        if (distance > attackRange)
        {
            // Di chuyển tới player
            if (animator != null)
            {
                animator.SetBool("isMove", true);
                animator.SetBool("isAttack", false);
            }
            Vector2 direction = (player.position - transform.position).normalized;
            if (rb != null)
                rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
        }
        else
        {
            // Đánh
            if (animator != null)
            {
                animator.SetBool("isMove", false);
                animator.SetBool("isAttack", true);
            }
            if (rb != null)
                rb.velocity = new Vector2(0, rb.velocity.y);
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                Attack();
                attackTimer = attackCooldown;
            }
        }
    }

    void HandleOutOfRange()
    {
        if (animator != null)
        {
            animator.SetBool("isMove", false);
            animator.SetBool("isAttack", false);
        }
        if (rb != null)
            rb.velocity = new Vector2(0, rb.velocity.y);
    }

    void Attack()
    {
        StartCoroutine(AttackOverTime());
    }

    System.Collections.IEnumerator AttackOverTime()
    {
        int ticks = 3; // 3 lần đánh
        for (int i = 0; i < ticks; i++)
        {
            if (animator != null)
                animator.SetTrigger("attack");
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance <= attackRange)
            {
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(attackDamage);
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
} 