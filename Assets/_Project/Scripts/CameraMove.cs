using UnityEngine;

public class CameraMove : MonoBehaviour
{
	[Header("Camera References")]
    [SerializeField] private new Camera camera;
    [Header("Zone Settings")]
	[SerializeField] private GameObject cameraPositions;

	private void OnEnable() => GameEvents.OnZoneChanged += MoveCamera;
    private void OnDisable() => GameEvents.OnZoneChanged -= MoveCamera;

    private void Awake() 
	{
		if (camera == null) camera = Camera.main;
	}

    private void MoveCamera(int zone)
	{
		if (camera == null || cameraPositions == null) return;

		float x = cameraPositions.transform.GetChild(zone).position.x;
		float y = cameraPositions.transform.GetChild(zone).position.y;
		camera.transform.position = new Vector3(x, y, camera.transform.position.z);
	}
}
