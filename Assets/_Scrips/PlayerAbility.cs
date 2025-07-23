using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
	[SerializeField] public float attackRange = 1.5f; // Phạm vi tấn công
	[Header("Skill Data")]
	public SkillData fireballSkill; // Kéo ScriptableObject của skill vào đây

	[SerializeField] public float attackDamage = 80f; // Sát thương gây ra
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
		// Không cho phép dùng skill khi game đang pause
		if (PauseManager.isGamePaused) return;

		// Nhấn phím U để dùng chiêu đặc biệt
		if (Input.GetKeyDown(KeyCode.U))
		{
			// Kiểm tra xem kỹ năng đã được mở khóa chưa
			if (fireballSkill != null && fireballSkill.IsUnlocked())
			{
				SpecialAttack();
			} else {
				Debug.Log("Kỹ năng Chưởng Lửa đang bị khóa! Hãy đạt điểm cao hơn để mở.");
			}
		}
	}

    [SerializeField] private GameObject fireballPrefab; // Prefab for the fireball (Special skill.png)
    [SerializeField] private Transform firePoint; // Where the fireball spawns from
    public float fireballSpeed = 1300f; // Speed of the fireball

	void SpecialAttack()
	{
		if (audioManager != null && audioManager.月牙天衝 != null)
			audioManager.PlaySFX(audioManager.月牙天衝);
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