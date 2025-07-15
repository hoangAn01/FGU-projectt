using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHp = 100f;
    public float currentHp;

    public ThanhHp thanhHp; // Tham chiếu đến script và thanh HP UI

    private void Start()
    {
        currentHp = maxHp;

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
        // Ví dụ:

        //Destroy(this.gameObject); // Xóa đối tượng nhân vật
                            // gameObject.SetActive(false);

    }
}