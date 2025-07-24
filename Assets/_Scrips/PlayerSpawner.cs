using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform spawnPoint;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && spawnPoint != null)
        {
            player.transform.position = spawnPoint.position;
        }
    }
}
