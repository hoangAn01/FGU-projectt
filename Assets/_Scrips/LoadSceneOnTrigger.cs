using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnTrigger : MonoBehaviour
{
	public string sceneName = "Greenpath"; // Tên scene muốn chuyển tới

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player")) {
			SceneManager.LoadScene(sceneName);
			Debug.Log("Player entered trigger, loading scene: " + sceneName);
		}
	}
}
