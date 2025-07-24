using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StoryIntroManager : MonoBehaviour
{
    [Header("Components")]
    [Tooltip("Kéo đối tượng Text chứa cốt truyện vào đây.")]
    public RectTransform storyTextTransform;

    [Header("Animation Settings")]
    [Tooltip("Tốc độ chữ chạy lên.")]
    public float scrollSpeed = 50f;
    [Tooltip("Vị trí Y mà khi chữ vượt qua thì sẽ chuyển scene.")]
    public float endYPosition = 1500f;

    [Header("Scene Management")]
    [Tooltip("Tên của scene sẽ được tải sau khi giới thiệu xong (ví dụ: 'menu').")]
    public string nextSceneName = "menu";

    private bool isSkipped = false;

    void Start()
    {
        StartCoroutine(ScrollText());
    }

    void Update()
    {
        // Cho phép người chơi nhấn phím bất kỳ để bỏ qua
        if (Input.anyKeyDown && !isSkipped)
        {
            SkipIntro();
        }
    }

    private IEnumerator ScrollText()
    {
        // Di chuyển text lên trên cho đến khi nó ra khỏi màn hình
        while (storyTextTransform.anchoredPosition.y < endYPosition)
        {
            storyTextTransform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
            yield return null;
        }

        // Tự động chuyển scene khi chữ đã chạy xong
        SkipIntro();
    }

    public void SkipIntro()
    {
        if (isSkipped) return;
        isSkipped = true;
        Debug.Log("Skipping intro, loading next scene: " + nextSceneName);
        SceneManager.LoadScene(nextSceneName);
    }
}