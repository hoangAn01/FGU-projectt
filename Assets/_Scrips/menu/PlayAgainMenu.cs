using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAgainMenu : MonoBehaviour
{
    public void Replay()
    {
        SceneManager.LoadScene("Game"); // Hoặc tên scene chơi chính của bạn
    }

    public void Quit()
    {
        SceneManager.LoadScene("menu"); // Hoặc Application.Quit() nếu muốn thoát hẳn
    }
}
