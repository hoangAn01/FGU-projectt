using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Thêm thư viện TextMeshPro

public class Menu : MonoBehaviour
{
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
    public void back()
    {
        SceneManager.LoadScene(0);
    }
}