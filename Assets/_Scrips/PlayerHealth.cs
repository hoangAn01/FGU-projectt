using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro; // Thêm dòng này
 // Thêm dòng này

public class PlayerHealth : MonoBehaviour
{
    public float maxHp = 100f;
    public float currentHp;

    public ThanhHp thanhHp; // Tham chiếu đến script và thanh HP UI

    public Animator animator;

    public int score = 0;

    public TextMeshProUGUI scoreText; // Thêm biến này

    private void Start()
    {
        currentHp = maxHp;
        animator = GetComponent<Animator>();
        if (thanhHp != null)
        {
            thanhHp.capNhatHp(currentHp, maxHp);
        }
        UpdateScoreUI(); // Cập nhật UI khi bắt đầu
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
        Debug.Log("Player died!");
        animator.SetBool("isDead", true);
        StartCoroutine(WaitForDeathAnimation());
    }

    private IEnumerator WaitForDeathAnimation()
    {
        // Giả sử animation chết dài 2 giây,
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("PlayAgain");
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

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI(); // Cập nhật UI mỗi khi cộng điểm
        Debug.Log("Điểm hiện tại: " + score);
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Điểm: " + score;
        }
    }
}