using UnityEngine;

public class ballMove : MonoBehaviour
{
    public Transform pointA;         // Điểm bắt đầu
    public Transform pointB;         // Điểm kết thúc
    public float speed = 2f;         // Tốc độ di chuyển

    public float damage = 10f;       // Số HP trừ khi chạm vào

    private Vector3 target;          // Điểm đang hướng đến

    void Start()
    {
        target = pointB.position;    // Bắt đầu hướng đến B
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            target = (target == pointA.position) ? pointB.position : pointA.position;
        }
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