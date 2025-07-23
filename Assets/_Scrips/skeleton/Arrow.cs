using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float damage = 30f;
    public float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra nếu va chạm với layer Player
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerHealth player = collision.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
} 