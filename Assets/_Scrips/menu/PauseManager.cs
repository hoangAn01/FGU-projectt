using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    // Biến static để các script khác có thể kiểm tra game có đang pause không
    public static bool isGamePaused = false;

    [Header("UI")]
    public GameObject pauseMenuUI; // Panel chứa menu pause

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ lại đối tượng này khi chuyển scene
        }
        else
        {
            Destroy(gameObject); // Hủy bản thân nếu đã có một instance khác
        }
    }

    void Start()
    {
        // Đảm bảo menu pause được ẩn khi bắt đầu và game chạy bình thường
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        isGamePaused = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        // Dùng phím Escape để pause/resume nhanh
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void Pause()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void LoadMenu()
    {
        // Quan trọng: Luôn reset time scale trước khi chuyển scene
        Time.timeScale = 1f;
        isGamePaused = false;
        SceneManager.LoadScene("menu");
    }
}