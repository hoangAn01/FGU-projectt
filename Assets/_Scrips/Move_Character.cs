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
	private float dashingPower = 40f;
	private float dashingTime = 1f;
	private float dashingCooldown = 1f;
	private float defaultGravityScale; // Gravity scale to restore after dash
	private Coroutine dashCoroutine;   // Reference to active dash coroutine
	private bool dashCancelled;        // Flag to cancel dash early
	[SerializeField] private float momentumCarryDuration = 0.15f; // Preserve dash momentum after dash/jump
	private float momentumCarryTimer = 0f;

	[SerializeField] private TrailRenderer tr; // Khoảng cách dashes
	[SerializeField] private Joystick joystick;
	
	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else Destroy(gameObject);
		
		// Initialize audioManager here
		audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
		tr = GetComponent<TrailRenderer>();
	}

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		originalScale = transform.localScale;
		defaultGravityScale = rb.gravityScale;
	}

	void Update()
	{
		// Không cho phép di chuyển khi game đang pause
		if (PauseManager.isGamePaused) return;
		
		// Lấy input đi trái/phải
		float moveX = Input.GetAxisRaw("Horizontal"); // -1, 0, 1
		if (joystick != null && joystick.Horizontal != 0)
		{
			moveX = joystick.Horizontal;
		}
	

		// Di chuyển nhân vật (không ghi đè khi đang dash)
		if (!isDashing)
		{
			if (momentumCarryTimer > 0f)
			{
				// Trong thời gian giữ quán tính, chỉ thay đổi khi input vượt quá vận tốc hiện tại
				float desiredVelX = moveX * speed;
				if (Mathf.Abs(desiredVelX) > Mathf.Abs(rb.velocity.x))
					rb.velocity = new Vector2(desiredVelX, rb.velocity.y);
				// Giảm dần thời gian giữ quán tính
				momentumCarryTimer -= Time.deltaTime;
			}
			else
			{
				rb.velocity = new Vector2(moveX * speed, rb.velocity.y);
			}
		}

		// Lật mặt nhân vật
		if (moveX > 0)
			transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
		else if (moveX < 0)
			transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);

		// Nhảy (cho phép nhảy khi đang dash để thực hiện dash jump)
		if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || isDashing))
		{
			Debug.Log("Jump button pressed");
			if (audioManager != null && audioManager.Jump != null)
				audioManager.PlaySFX(audioManager.Jump); // Play jump sound if available
			else Debug.LogWarning("AudioManager or Jump sound not properly set up!");

			// Nếu đang dash, hủy dash sớm và giữ quán tính ngang một thời gian ngắn
			if (isDashing)
			{
				CancelDash();
				momentumCarryTimer = momentumCarryDuration;
			}

			rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
			isGrounded = false;
		}
		
		// Dash chỉ khi đang đứng trên mặt đất (Ground Dash)
		if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && isGrounded)
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
		dashCancelled = false;
		// Tắt trọng lực tạm thời và phóng về phía đang đối mặt
		rb.gravityScale = 0f;
		float dashDirection = Mathf.Sign(transform.localScale.x);
		rb.velocity = new Vector2(dashDirection * dashingPower, 0f);
		if (tr != null) tr.emitting = true;
		else Debug.LogWarning("TrailRenderer component not found on the character. Please add one for the dash effect.");

		float elapsed = 0f;
		while (!dashCancelled && elapsed < dashingTime)
		{
			elapsed += Time.deltaTime;
			yield return null;
		}

		if (tr != null) tr.emitting = false;
		rb.gravityScale = defaultGravityScale;
		isDashing = false;
		// Giữ quán tính ngang trong thời gian ngắn sau khi kết thúc dash
		momentumCarryTimer = momentumCarryDuration;

		yield return new WaitForSeconds(dashingCooldown);
		canDash = true;
	}

	private void CancelDash()
	{
		if (!isDashing) return;
		dashCancelled = true;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground")) isGrounded = true;
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
			isGrounded = false;
	}
}