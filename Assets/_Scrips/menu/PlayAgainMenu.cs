using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Thêm thư viện TextMeshPro

public class PlayAgainMenu : MonoBehaviour
{
    public TextMeshProUGUI lastScoreText; // Hiển thị điểm cuối cùng
    public TextMeshProUGUI highScoreText; // Hiển thị điểm cao nhất

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
