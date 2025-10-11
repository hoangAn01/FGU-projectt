using UnityEngine;

public class LoadNextLevelOnTrigger : MonoBehaviour
{
    [SerializeField] private string requiredTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!string.IsNullOrEmpty(requiredTag) && !other.CompareTag(requiredTag)) return;
        LevelManager.LoadNextLevel();
    }
}
