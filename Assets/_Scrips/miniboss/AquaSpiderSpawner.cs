using UnityEngine;
using System.Collections;

public class AquaSpiderSpawner : MonoBehaviour
{
    public GameObject aquaSpiderPrefab;
    public Vector2 spawnPosition;
    public float spawnDelay = 15f;

    void Start()
    {
        StartCoroutine(SpawnSpiderLoop());
    }

    IEnumerator SpawnSpiderLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);
            Instantiate(aquaSpiderPrefab, spawnPosition, Quaternion.identity);
        }
    }
}