using UnityEngine;

public class FireballBehavior : MonoBehaviour
{
    public float damage = 20f;
    public float lifetime = 2.5f;
    public GameObject hitEffect; // Optional: particle effect on hit

    private void Start()
    {
        // Destroy after lifetime
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if we hit an enemy
        if (other.CompareTag("Enemy"))
        {
            // Try to get the MiniBossHealth component (as used in PlayerAttack.cs)
            MiniBossHealth enemyHealth = other.GetComponent<MiniBossHealth>();
            if (enemyHealth != null)
                enemyHealth.TakeDamage(damage);

            // If we have a hit effect, spawn it
            if (hitEffect != null)
                Instantiate(hitEffect, transform.position, Quaternion.identity);

            // Destroy the fireball
            Destroy(gameObject);
        }
    }
}