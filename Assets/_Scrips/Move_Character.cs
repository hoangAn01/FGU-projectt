
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

    private static Move_Character instance; // CS0102: Only one instance declaration

    [SerializeField] private TrailRenderer tr;
    private AudioManager audioManager; // CS0102: Only one audioManager declaration
    private bool canDash = true;
    private bool isDashing;
    private readonly float dashingPower = 24f; // S2933: readonly
    private readonly float dashingTime = 0.2f; // S2933: readonly
    private readonly float dashingCooldown = 1f; // S2933: readonly

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
        if (PauseManager.isGamePaused) return;

        float moveX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveX * speed, rb.velocity.y);

        if (moveX > 0)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else if (moveX < 0)
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);

        if (isDashing) return;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            if (audioManager != null && audioManager.Jump != null)
                audioManager.PlaySFX(audioManager.Jump);
            else
                Debug.LogWarning("AudioManager or Jump sound not properly set up!");

            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }

        if (Input.GetKeyDown(KeyCode.L) && canDash)
            StartCoroutine(Dash());

        animator.SetBool("isRunning", Mathf.Abs(moveX) > 0.01f);

        float verticalVelocity = rb.velocity.y;
        animator.SetBool("isJumping", verticalVelocity > 0.1f);
        animator.SetBool("isFalling", verticalVelocity < -0.1f);
        animator.SetBool("isGrounded", isGrounded);
    }

    private void FixedUpdate()
    {
        // S3626: Remove redundant jump (no code needed here for dashing)
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}