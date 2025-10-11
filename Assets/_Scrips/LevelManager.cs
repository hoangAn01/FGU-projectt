using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    // Ordered list of gameplay levels included in Build Settings
    private static readonly string[] LevelOrder = new string[]
    {
        "Game",
        "Greenpath",
        "mini boss",
        "To The Top Platform"
    };

    private const string CurrentLevelIndexKey = "CurrentLevelIndex";
    private int currentLevelIndex;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        if (Instance != null) return;
        var go = new GameObject("LevelManager");
        DontDestroyOnLoad(go);
        Instance = go.AddComponent<LevelManager>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        currentLevelIndex = Mathf.Clamp(
            PlayerPrefs.GetInt(CurrentLevelIndexKey, 0),
            0,
            LevelOrder.Length - 1
        );
    }

    private static void SaveIndex(int index)
    {
        PlayerPrefs.SetInt(CurrentLevelIndexKey, index);
        PlayerPrefs.Save();
    }

    private static int GetIndex()
    {
        return Instance != null
            ? Instance.currentLevelIndex
            : PlayerPrefs.GetInt(CurrentLevelIndexKey, 0);
    }

    private static void SetIndex(int index)
    {
        if (Instance != null) Instance.currentLevelIndex = index;
        SaveIndex(index);
    }

    private static void LoadScene(string sceneName)
    {
        Time.timeScale = 1f; // ensure unpaused on load
        SceneManager.LoadScene(sceneName);
    }

    public static void StartNewGame()
    {
        SetIndex(0);
        LoadScene(LevelOrder[0]);
    }

    public static void RestartLevel()
    {
        LoadScene(LevelOrder[GetIndex()]);
    }

    public static void LoadNextLevel()
    {
        int next = GetIndex() + 1;
        if (next < LevelOrder.Length)
        {
            SetIndex(next);
            LoadScene(LevelOrder[next]);
        }
        else
        {
            // End of campaign: return to menu
            LoadScene("menu");
        }
    }

    public static void LoadLevelByName(string sceneName)
    {
        int idx = Array.IndexOf(LevelOrder, sceneName);
        if (idx >= 0) SetIndex(idx);
        LoadScene(sceneName);
    }

    public static bool IsLevelScene(string sceneName)
    {
        return LevelOrder.Contains(sceneName);
    }

    public static string[] GetLevelOrder()
    {
        return (string[])LevelOrder.Clone();
    }
}
