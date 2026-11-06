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

    private void Start()
    {
		// Set the initial camera position to the first zone
		MoveCamera(0);
	}

    private void MoveCamera(int zone)
	{
		if (camera == null || cameraPositions == null) return;

		GameObject cameraPosition = cameraPositions.transform.GetChild(zone).gameObject;

		float x = cameraPosition.transform.position.x;
		float y = cameraPosition.transform.position.y;
		camera.transform.position = new Vector3(x, y, camera.transform.position.z);
	}
}
