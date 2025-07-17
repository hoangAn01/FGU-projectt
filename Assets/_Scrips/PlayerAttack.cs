using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] public float attackRange = 1.5f; // Phạm vi tấn công
    [SerializeField] public float attackDamage = 20f; // Sát thương gây ra
    public LayerMask enemyLayers;    // Layer của kẻ địch
    public Animator animator;        // Animator để trigger animation đánh

    void Start()
    {
        
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) // Nhấn phím J để đánh
        {
            Attack();
        }
    }

    void Attack()
    {
        // Trigger animation đánh nếu có
        
        animator.SetTrigger("Attack");
        Debug.Log("chém");
        // Phát hiện kẻ địch trong phạm vi tấn công
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            // Gây sát thương cho kẻ địch (giả sử kẻ địch có script PlayerHealth hoặc EnemyHealth)
            PlayerHealth enemyHealth = enemy.GetComponent<PlayerHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
            }
        }
    }

    // Vẽ phạm vi tấn công trong Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
} 