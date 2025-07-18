using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHp = 100f;
    public float currentHp;

    public ThanhHp thanhHp; // Tham chiếu đến script và thanh HP UI

    public Animator animator;

    private void Start()
    {
        currentHp = maxHp;
        animator = GetComponent<Animator>();
        if (thanhHp != null)
        {
            thanhHp.capNhatHp(currentHp, maxHp);
        }
    }

    // Hàm trừ HP khi bị trúng
    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);

        if (thanhHp != null)
        {
            thanhHp.capNhatHp(currentHp, maxHp);
        }

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Xử lý khi nhân vật chết (vd: disable, respawn, reload scene)
        Debug.Log("Player died!");
        animator.SetBool("isDead", true);
        // Chuyển sang scene PlayAgain
        UnityEngine.SceneManagement.SceneManager.LoadScene("PlayAgain");
    }

    public void Heal(int amount)
    {
        currentHp += amount;
        if (currentHp > maxHp) currentHp = maxHp;
        if (thanhHp != null)
        {
            thanhHp.capNhatHp(currentHp, maxHp);
        }
        Debug.Log("Đã hồi máu: " + amount + " | Máu hiện tại: " + currentHp);
    }
}