using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
	[System.Serializable]
	public class ParallaxLayer
	{
		public Transform layerTransform;
		[Range(0f, 1f)]
		public float parallaxFactor = 0.5f; // How much this layer moves relative to camera movement
	}

	public ParallaxLayer[] layers;          // Array of layers to affect
	public float smoothing = 1f;            // Smoothing factor for movement
	
	private Transform mainCamera;            // Reference to the main camera
	private Vector3 lastCameraPosition;      // Previous frame camera position

	private void Start()
	{
		mainCamera = Camera.main.transform;
		lastCameraPosition = mainCamera.position;
	}

	private void LateUpdate()
	{
		Vector3 deltaMovement = mainCamera.position - lastCameraPosition;

		foreach (ParallaxLayer layer in layers)
		{
			if (layer.layerTransform != null)
			{
				Vector3 parallaxPosition = layer.layerTransform.position;
				// Move each layer based on its parallax factor
				parallaxPosition.x -= deltaMovement.x * layer.parallaxFactor;
				parallaxPosition.y -= deltaMovement.y * layer.parallaxFactor;
				
				layer.layerTransform.position = Vector3.Lerp(
					layer.layerTransform.position,
					parallaxPosition,
					smoothing * Time.deltaTime
				);
			}
		}

		lastCameraPosition = mainCamera.position;
	}
}