using System.Collections;
using UnityEngine;

public class SkeletonArcher : MonoBehaviour
{
    [Header("AI Settings")]
    public float detectRange = 10f; // tầm nhìn
    public float shootRange = 6f;   // tầm bắn
    public float moveSpeed = 5f; // tốc độ di chuyển

    [Header("Combat Settings")]
    public float shootCooldown = 2f;
    public GameObject arrowPrefab;
    public float arrowSpeed = 10f;

    private Transform player;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Animator animator;
    
    private float shootTimer = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (animator != null) {
            animator.SetBool("isMove", false);
            animator.SetBool("isShot", false);
        }
    }

    void Update()
    {
        if (player == null)
        {
            if (animator != null) {
                animator.SetBool("isMove", false);
                animator.SetBool("isShot", false);
            }
            if (rb != null) rb.velocity = new Vector2(0, rb.velocity.y);
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
        if (spriteRenderer != null)
            spriteRenderer.flipX = (player.position.x < transform.position.x);

        if (distance > shootRange)
        {
            // Move towards player
            if (animator != null) {
                animator.SetBool("isMove", true);
                animator.SetBool("isShot", false);
            }
            Vector2 direction = (player.position - transform.position).normalized;
            if (rb != null)
                rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
        }
        else
        {
            // Stop and shoot
            if (animator != null) {
                animator.SetBool("isMove", false);
                animator.SetBool("isShot", true);
            }
            if (rb != null)
                rb.velocity = new Vector2(0, rb.velocity.y);

            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                ShootArrow();
                shootTimer = shootCooldown;
            }
        }
    }

    void HandleOutOfRange()
    {
        if (animator != null) {
            animator.SetBool("isMove", false);
            animator.SetBool("isShot", false);
        }
        if (rb != null)
            rb.velocity = new Vector2(0, rb.velocity.y);
    }

    void ShootArrow()
{
    if (arrowPrefab == null) return;

    // Xác định đang quay mặt phải hay trái
    bool isFacingRight = (spriteRenderer != null) ? !spriteRenderer.flipX : true;

    // Tính vị trí spawn mũi tên bên phải/trái nhân vật
    float offsetX = 0.5f;
    Collider2D col = GetComponent<Collider2D>();
    if (col != null)
        offsetX = col.bounds.extents.x + 0.1f;

    Vector2 spawnPos = (Vector2)transform.position + new Vector2(isFacingRight ? offsetX : -offsetX, 0);

    // Tạo mũi tên
    GameObject arrow = Instantiate(arrowPrefab, spawnPos, Quaternion.identity);

    // Bắn theo hướng mà skeleton đang quay mặt
    Vector2 dir = isFacingRight ? Vector2.right : Vector2.left;

    // Gán velocity
    Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();
    if (arrowRb != null)
    {
        arrowRb.velocity = dir * arrowSpeed;
    }

    // Xoay mũi tên đúng hướng
    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    arrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    // Tự huỷ mũi tên sau 5 giây
    Destroy(arrow, 5f);
}

} 