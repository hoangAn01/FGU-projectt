using UnityEngine;

public class Move_Character : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5f;
    public float jumpCooldown = 0.1f; // Add cooldown time between jumps
    private float jumpTimeStamp; // Track last jump time

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private Vector3 originalScale;
    private static Move_Character instance;
    private AudioManager audioManager;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;
        jumpTimeStamp = -jumpCooldown; // Allow immediate first jump
    }

    void Update()
    {
        // Lấy input đi trái/phải
        float moveX = Input.GetAxisRaw("Horizontal"); // -1, 0, 1

        // Di chuyển nhân vật
        rb.velocity = new Vector2(moveX * speed, rb.velocity.y);

        // Lật mặt nhân vật
        if (moveX > 0)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else if (moveX < 0)
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);

        // Modified jump logic with cooldown
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && Time.time >= jumpTimeStamp + jumpCooldown)
        {
            if (audioManager != null && audioManager.Jump != null)
                audioManager.PlaySFX(audioManager.Jump);
            else Debug.LogWarning("AudioManager or Jump sound not properly set up!");

            rb.velocity = new Vector2(rb.velocity.x, 0f); // Reset vertical velocity before jump
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            jumpTimeStamp = Time.time;
        }

        // Update animator parameters
        animator.SetBool("isRunning", Mathf.Abs(moveX) > 0.01f);
        float verticalVelocity = rb.velocity.y;
        animator.SetBool("isJumping", verticalVelocity > 0.1f);
        animator.SetBool("isFalling", verticalVelocity < -0.1f);
        animator.SetBool("isGrounded", isGrounded);
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Ground")) isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Ground")) isGrounded = false;
    }
}