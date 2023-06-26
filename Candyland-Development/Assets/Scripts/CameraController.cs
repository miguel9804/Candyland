using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	Camera mainCamera;

	[SerializeField]
	Transform player;
	[SerializeField]
	Vector3 offset; //Offset for camera
	[SerializeField]
	float zoomedInValue, zoomedOutValue, zoomSpeed;
	[SerializeField]
	private bool zoomed;


	void Start()
	{
		zoomed = true;
		mainCamera = Camera.main;
		zoomSpeed = 4f;
	}

	// Update is called once per frame
	void Update()
	{
		if (zoomed)
		{
			mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, zoomedInValue, Time.deltaTime * zoomSpeed);
		}
		else
		{
			mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, zoomedOutValue, Time.deltaTime * zoomSpeed);
		}
		transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z); // Camera follows the player with specified offset position
	}

    public void ZoomIn()
    {
		zoomed = true;
		Debug.Log("Zoom is + "+zoomed);
	}
	public void ZoomOut()
    {
		zoomed = false;
		Debug.Log("Zoom is + " + zoomed);
	}
}