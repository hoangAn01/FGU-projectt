using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
	public Transform spawnPoint;
	public Transform pointA; // Teleport point A
	public Transform pointB; // Teleport point B

	private GameObject player;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		if (player != null && spawnPoint != null)
		{
			player.transform.position = spawnPoint.position;
		}
	}

	void Update()
	{
		
	}
}
