using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }// duy nhất ngã độc tôn

    // Biến static để các script khác có thể kiểm tra game có đang pause không
    public static bool isGamePaused { get; private set; } 
     // Biến tĩnh status game pause 
     // mở rộng để các script khác có thể kiểm tra trạng thái pause
     

    [Header("UI")]
    [Tooltip("Kéo Panel chứa menu pause vào đây.")]
    public GameObject pauseMenuUI; // Panel chứa menu pause

    private void Awake()
    {
        // Singleton pattern cho scene hiện tại
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Một PauseManager khác đã tồn tại. Hủy đối tượng mới.");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        // Đảm bảo menu pause được ẩn khi bắt đầu và game chạy bình thường
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        // Reset trạng thái khi bắt đầu scene
        isGamePaused = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        // Dùng phím Escape để pause/resume nhanh
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
    }

    
    private void OnDestroy()
    {
        if (Instance == this)
        {
            // Quan trọng: Đảm bảo time scale được reset nếu game bị pause khi chuyển scene
            Time.timeScale = 1f;
            Instance = null;
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

    public void TogglePause()
    {
        if (isGamePaused) Resume();
        else Pause();
    }

    public void LoadMenu()
    {
        // Quan trọng: Reset time scale trước khi chuyển scene
        Time.timeScale = 1f;
        SceneManager.LoadScene("menu");
    }
}