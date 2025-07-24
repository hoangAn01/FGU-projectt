//using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchingDirection : MonoBehaviour
{
	CapsuleCollider2D TouchingColider;
	public ContactFilter2D castFilter;
	public float groundDistance = 0.05f;

	RaycastHit2D[] groundHits = new RaycastHit2D[5];

	private void Awake(){
		TouchingColider = GetComponent<CapsuleCollider2D>();
	}

	private void Update()
	{
		//Debug.Log("Touching Direction");
		int hitCount = TouchingColider.Cast(Vector2.down, castFilter, groundHits, groundDistance);
		if(hitCount > 0)
			for(int i = 0; i < hitCount; i++){
				Debug.DrawLine(TouchingColider.bounds.center, groundHits[i].point, Color.red);
				Debug.Log("Hit: " + groundHits[i].collider.name);
			}
		else Debug.Log("No Ground Detected");
	}
}
