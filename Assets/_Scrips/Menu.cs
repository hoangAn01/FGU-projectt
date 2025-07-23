using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Thêm thư viện TextMeshPro

public class Menu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel; // Kéo Panel của menu chính vào đây
    public GameObject skillMenuPanel; // Kéo Panel của menu kỹ năng vào đây
    public TextMeshProUGUI highScoreText; // Tham chiếu đến UI Text để hiện High Score

    void Start()
    {
        if (highScoreText != null)
        {
            // Lấy điểm cao nhất từ PlayerPrefs, nếu chưa có thì mặc định là 0
            int highScore = PlayerPrefs.GetInt("HighScore", 0);
            highScoreText.text = "Điểm Cao: " + highScore;
        }
        else
        {
            Debug.LogWarning("HighScoreText is not assigned in the Menu script!");
        }

        // Đảm bảo trạng thái ban đầu của các panel là chính xác khi scene bắt đầu
        if (skillMenuPanel != null)
        {
            skillMenuPanel.SetActive(false);
        }
    }

    public void play()
    {
        SceneManager.LoadScene("Game");
    }
    public void quit()
    {
        Application.Quit();// thoat luon game
        Debug.Log("Quit Game");
    }

    public void OpenSkillMenu()
    {
        // Ẩn menu chính, hiện menu kỹ năng
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (skillMenuPanel != null) skillMenuPanel.SetActive(true);
    }

    public void CloseSkillMenu()
    {
        // Ẩn menu kỹ năng, hiện menu chính
        if (skillMenuPanel != null) skillMenuPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }
    public void back()
    {
        SceneManager.LoadScene(0);
    }

    // Hàm này dùng để demo, reset điểm và kỹ năng đã lưu
    public void ResetData()
    {
        // Xóa điểm cao đã lưu
        PlayerPrefs.DeleteKey("HighScore");
        // Xóa kỹ năng đã mở khóa (quan trọng!)
        PlayerPrefs.DeleteKey("FireballUnlocked");
        PlayerPrefs.Save(); // Lưu thay đổi

        // Cập nhật lại text trên màn hình
        highScoreText.text = "Điểm Cao: 0";
        Debug.Log("ĐÃ RESET: High Score và các kỹ năng đã mở khóa!");
    }
}