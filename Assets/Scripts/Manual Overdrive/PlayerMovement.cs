// Title: PlayerMovement
// Author: Skyler Riggle

using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{
    private Transform cameraTransform;
    [SerializeField] private Vector3 cameraOffset = Vector3.zero;

    private RoadSpline roadSpline;
    private const float START_SPEED = 0.1f;
    private float currentPosition = 0, currentSpeed = START_SPEED;
    private bool canMove = false;

    private void Awake() => cameraTransform = Camera.main.transform;

    public void SetMovement(bool status) => canMove = status;

    private void Update()
    {
        if (canMove)
        {
            currentPosition += currentSpeed * Time.deltaTime;
            UpdatePosition();
        }
    }

    public void SetDefaultPosition(RoadSpline newRoad)
    {
        roadSpline = newRoad;
        currentPosition = 0;
        currentSpeed = START_SPEED;
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        // Sample the road.
        Vector3 samplePoint = roadSpline.SampleSpline(currentPosition);
        Vector3 currentRoadTangent = roadSpline.SampleTangent3D(currentPosition);

        // Set the player's position and rotation values.
        transform.position = samplePoint;
        transform.forward = currentRoadTangent;

        // Set the camera's position and rotation values.
        cameraTransform.forward = currentRoadTangent;
        cameraTransform.position = transform.position + 
            (transform.right * cameraOffset.x) +
            (transform.up * cameraOffset.y) +
            (transform.forward * cameraOffset.z);
    }
}