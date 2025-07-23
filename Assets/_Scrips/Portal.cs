using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{
	public Transform destination; // Assign the other portal's transform in the Inspector
	private Collider2D portalCollider;

	private void Awake(){
		portalCollider = GetComponent<Collider2D>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && destination != null && portalCollider.enabled)
		{
			other.transform.position = destination.position;
			StartCoroutine(DeactivateTemporarily());
			DeactivateAllPortals.DeactivateForSeconds(1f);
		}
	}

	private IEnumerator DeactivateTemporarily()
	{
		portalCollider.enabled = false;
		yield return new WaitForSeconds(2f);
		portalCollider.enabled = true;
	}
}