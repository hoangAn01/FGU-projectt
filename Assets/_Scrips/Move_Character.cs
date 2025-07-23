
using System.Collections;
using UnityEngine;
public class Move_Character : MonoBehaviour
{
	public float speed = 5f;
	public float jumpForce = 5f;

	private Rigidbody2D rb;
	private Animator animator;

	private bool isGrounded;

	private Vector3 originalScale;

	private static Move_Character instance;

	private AudioManager audioManager; // Move declaration inside the class
	private bool canDash = true;
	private bool isDashing;
	private float dashingPower = 24f;
	private float dashingTime = 0.2f;
	private float dashingCooldown = 1f;

	[SerializeField] private TrailRenderer tr; // Khoảng cách dashes

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
		// Initialize audioManager here
		audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
	}

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		originalScale = transform.localScale;
	}

	void Update()
	{
		// Không cho phép di chuyển khi game đang pause
        if (PauseManager.isGamePaused) return;
		
		// Lấy input đi trái/phải
		float moveX = Input.GetAxisRaw("Horizontal"); // -1, 0, 1

		// Di chuyển nhân vật
		rb.velocity = new Vector2(moveX * speed, rb.velocity.y);

		// Lật mặt nhân vật
		if (moveX > 0)
			transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
		else if (moveX < 0)
			transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);

		if (isDashing) return;
		
		// Nhảy
		if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
		{
			if (audioManager != null && audioManager.Jump != null)
				audioManager.PlaySFX(audioManager.Jump); // Play jump sound if available
			else Debug.LogWarning("AudioManager or Jump sound not properly set up!");

			rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
			isGrounded = false;
		}
		
		if (Input.GetKeyDown(KeyCode.L) && canDash)
			StartCoroutine(Dash());

		// Cập nhật các biến cho Animator

		// isRunning: khi có input trái/phải
		animator.SetBool("isRunning", Mathf.Abs(moveX) > 0.01f);

		float verticalVelocity = rb.velocity.y;

		// isJumping: đang đi lên (velocity y dương lớn)
		animator.SetBool("isJumping", verticalVelocity > 0.1f);

		// isFalling: đang rơi xuống (velocity y âm)
		animator.SetBool("isFalling", verticalVelocity < -0.1f);

		// isGrounded: thử giữ để animator có thể dùng nếu cần
		animator.SetBool("isGrounded", isGrounded);
	}
	
	private void FixedUpdate()
	{
		if (isDashing) return;
	}

	private IEnumerator Dash()
	{
		canDash = false;
		isDashing = true;
		float originalGravity = rb.gravityScale;
		rb.gravityScale = 0f;
		rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
		tr.emitting = true;
		yield return new WaitForSeconds(dashingTime);
		tr.emitting = false;
		rb.gravityScale = originalGravity;
		isDashing = false;
		yield return new WaitForSeconds(dashingCooldown);
		canDash = true;
	}
}