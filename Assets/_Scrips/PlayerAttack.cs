using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
	[SerializeField] public float attackRange = 1.5f; // Phạm vi tấn công
	[SerializeField] public float attackDamage = 20f; // Sát thương gây ra
	public LayerMask enemyLayers;    // Layer của kẻ địch
	public Animator animator;        // Animator để trigger animation đánh

	AudioManager audioManager;

	private void Awake() { 
		audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
	}

	void Start(){
		animator = GetComponent<Animator>();
	}

	void Update()
	{
		// Nhấn phím J để đánh
		if (Input.GetKeyDown(KeyCode.J)) Attack();
		// Nhấn phím U để dùng chiêu đặc biệt
		if (Input.GetKeyDown(KeyCode.U)) SpecialAttack();
	}

	void Attack()
	{
		if (audioManager != null && audioManager.Attack != null) 
			audioManager.PlaySFX(audioManager.Attack);

		else Debug.LogWarning("AudioManager or Attack sound not properly set up!");
		

		animator.SetTrigger("Attack");
		Debug.Log("chém");
		// Phát hiện kẻ địch trong phạm vi tấn công
		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayers);

		foreach (Collider2D enemy in hitEnemies)
		{
			// Gây sát thương cho kẻ địch (giả sử kẻ địch có script PlayerHealth hoặc EnemyHealth)
			MiniBossHealth enemyHealth = enemy.GetComponent<MiniBossHealth>();
			if (enemyHealth != null)
				enemyHealth.TakeDamage(attackDamage);
			
		}
	}

    [SerializeField] private GameObject fireballPrefab; // Prefab for the fireball (Special skill.png)
    [SerializeField] private Transform firePoint; // Where the fireball spawns from
    public float fireballSpeed = 1300f; // Speed of the fireball

	void SpecialAttack()
	{
		if (fireballPrefab == null || firePoint == null)
		{
			Debug.LogWarning("Fireball prefab or fire point not assigned!");
			return;
		}
		// Determine direction (assuming player faces right by default)
		float direction = transform.localScale.x > 0 ? 1f : -1f;
		GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
        Destroy(fireball, 2.5f);
		Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
		if (rb != null)
			rb.velocity = new Vector2(fireballSpeed * direction, 0f);
		
		// Optionally flip sprite if facing left
		if (direction < 0)
		{
			Vector3 scale = fireball.transform.localScale;
			scale.x *= -1;
			fireball.transform.localScale = scale;
		}
	}

	// Vẽ phạm vi tấn công trong Scene view
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, attackRange);
	}
} 