using UnityEngine;

public class Boar : MonoBehaviour
{

    [SerializeField] private float speed = 2f;         // Tốc độ di chuyển
    [SerializeField] private float distance = 5f;    // Khoảng cách di chuyển qua lại
    public float damage = 10f;       // Số HP trừ khi chạm vào
    private bool movingRight = true; // Biến để xác định hướng di chuyển
    private Vector3 startPos;          // Điểm đang hướng đến
    private bool isIdle = false;
    private float idleTimer = 0f;
    [SerializeField] private float idleDuration = 3f; // Thời gian idle
    private Animator animator;

    void Start()
    {
        startPos = transform.position; // Lưu vị trí ban đầu
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isIdle)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleDuration)
            {
                isIdle = false;
                idleTimer = 0f;
                animator.SetBool("idle", false); // Quay lại trạng thái walk
            }
            return;
        }

        float leftBound = startPos.x - distance; // Tính biên trái
        float rightBound = startPos.x + distance; // Tính biên phải
        if (movingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime); // Di chuyển sang phải
            if (transform.position.x >= rightBound)
            {
                movingRight = false; // Đổi hướng khi chạm biên phải
                StartIdle();
                Flip(); // Lật hướng quả bóng
            }
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime); // Di chuyển sang phải
            if (transform.position.x <= leftBound)
            {
                movingRight = true; // Đổi hướng khi chạm biên phải
                StartIdle();
                Flip(); // Lật hướng quả bóng
            }
        }

    }

    void Flip()
    {         // Lật hướng của quả bóng
        Vector3 scale = transform.localScale;
        scale.x *= -1; // Đảo ngược trục X 
        transform.localScale = scale;
    }

    void StartIdle()
    {
        isIdle = true;
        idleTimer = 0f;
        animator.SetBool("idle", true); // Chuyển sang trạng thái idle
    }

    // Hàm này được gọi khi vật thể khác chạm vào collider quả bóng (đảm bảo collider và isTrigger đúng)
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check xem đối tượng va chạm có tag "Player" không
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }


}