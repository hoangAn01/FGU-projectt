using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Thêm thư viện TextMeshPro

public class PlayAgainMenu : MonoBehaviour
{
    public TextMeshProUGUI lastScoreText; // Hiển thị điểm cuối cùng
    public TextMeshProUGUI highScoreText; // Hiển thị điểm cao nhất

    [Header("Unlock Notification")]
    public GameObject unlockNotificationPanel; // Panel thông báo
    public TextMeshProUGUI unlockNotificationText; // Text trong panel

    void Start()
    {
        // Lấy và hiển thị điểm cuối cùng
        if (lastScoreText != null)
        {
            int lastScore = PlayerPrefs.GetInt("LastScore", 0);
            lastScoreText.text = "Điểm: " + lastScore;
        }
        else
        {
            Debug.LogWarning("LastScoreText is not assigned in the PlayAgainMenu script!");
        }

        // Lấy và hiển thị điểm cao nhất
        if (highScoreText != null)
        {
            int highScore = PlayerPrefs.GetInt("HighScore", 0);
            highScoreText.text = "Điểm Cao: " + highScore;
        }
        else
        {
            Debug.LogWarning("HighScoreText is not assigned in the PlayAgainMenu script!");
        }

        CheckForUnlockedSkill();
    }

    private void CheckForUnlockedSkill()
    {
        // Kiểm tra xem có "cờ" báo hiệu skill vừa được mở không
        string unlockedSkillName = PlayerPrefs.GetString("JustUnlockedSkillName", "");

        if (!string.IsNullOrEmpty(unlockedSkillName) && unlockNotificationPanel != null)
        {
            // Nếu có, hiển thị thông báo
            if (unlockNotificationText != null)
                unlockNotificationText.text = $"Kỹ năng mới đã mở khóa!\n<color=yellow>{unlockedSkillName}</color>";
            
            unlockNotificationPanel.SetActive(true);

            // Xóa cờ đi để nó không hiển thị lại ở lần sau
            PlayerPrefs.DeleteKey("JustUnlockedSkillName");
        }
        else if (unlockNotificationPanel != null) unlockNotificationPanel.SetActive(false);
    }

    public void Replay()
    {
        SceneManager.LoadScene("Game"); // Hoặc tên scene chơi chính của bạn
    }

    public void Quit()
    {
        SceneManager.LoadScene("menu"); // Hoặc Application.Quit() nếu muốn thoát hẳn
    }
}
