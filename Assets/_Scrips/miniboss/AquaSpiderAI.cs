using UnityEngine;

public class AquaSpiderAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float attackRange = 3f;
    public float moveRange = 5f; // phạm vi di chuyển ngẫu nhiên quanh vị trí gốc
    public Transform player;

    private Animator animator;
    private Vector2 originPos;
    private Vector2 targetPos;
    private bool isAttacking = false;
    public float attackCooldown = 1f;
    private float lastAttackTime = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        originPos = transform.position;
        ChooseNewTarget();
        FindPlayer();
    }

    void Update()
    {
        if (player == null)
        {
            FindPlayer();
            if (player == null) return; // Không làm gì nếu vẫn chưa có player
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                animator.Play("attack");
            }
            // Gây sát thương theo cooldown
            if (Time.time - lastAttackTime > attackCooldown)
            {
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(10);
                    lastAttackTime = Time.time;
                }
            }
        }
        else
        {
            if (isAttacking)
            {
                isAttacking = false;
                animator.Play("run");
            }
            MoveRandomly();
        }
    }

    void MoveRandomly()
    {
        // Nếu gần tới điểm đích thì chọn điểm mới
        if (Vector2.Distance(transform.position, targetPos) < 0.2f)
        {
            ChooseNewTarget();
        }
        // Di chuyển tới điểm đích
        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

    void ChooseNewTarget()
    {
        float randomX = Random.Range(originPos.x - moveRange, originPos.x + moveRange);
        float randomY = originPos.y; // Nếu chỉ muốn di chuyển ngang, giữ nguyên Y
        targetPos = new Vector2(randomX, randomY);
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning("Không tìm thấy player trong scene!");
    }
}
