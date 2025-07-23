using UnityEngine;
using System.Collections;

public class DeactivateAllPortals : MonoBehaviour
{
	public static void DeactivateForSeconds(float seconds)
	{
		// Start the coroutine from a temporary GameObject
		var temp = new GameObject("PortalDeactivator");
		var deactivator = temp.AddComponent<PortalDeactivator>();
		deactivator.StartDeactivation(seconds);
	}

	// Internal helper MonoBehaviour to run the coroutine
	private class PortalDeactivator : MonoBehaviour
	{
		public void StartDeactivation(float seconds)
		{
			StartCoroutine(DeactivateAllPortalsCoroutine(seconds));
		}

		private IEnumerator DeactivateAllPortalsCoroutine(float seconds)
		{
			// Find all portals and disable their colliders
			var portals = FindObjectsOfType<Portal>();
			foreach (var portal in portals)
			{
				if (portal.TryGetComponent<Collider2D>(out var col))
					col.enabled = false;
			}

			yield return new WaitForSeconds(seconds);

			// Re-enable all portal colliders
			foreach (var portal in portals){
				if (portal.TryGetComponent<Collider2D>(out var col))
					col.enabled = true;
			}

			Destroy(gameObject); // Clean up the temporary object
		}
	}
}