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
		// Không cho phép tấn công khi game đang pause
		if (PauseManager.isGamePaused) return;

		// Nhấn phím J để đánh
		if (Input.GetKeyDown(KeyCode.J)) Attack();
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
			
			BoarHealth boar = enemy.GetComponent<BoarHealth>();
			if (boar != null)
				boar.TakeDamage(attackDamage);
			
		}
	}

	// Vẽ phạm vi tấn công trong Scene view
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, attackRange);
	}
} 