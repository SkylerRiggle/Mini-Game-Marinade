// Title: PlayerMovement
// Author: Skyler Riggle

using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{
    private Transform cameraTransform;
    [SerializeField] private float cameraYOffset = 0.5f;
    [SerializeField] private float smoothing = 2;

    private RoadSpline roadSpline;
    private const float START_SPEED = 1f;
    private float currentPosition = 0, currentSpeed = START_SPEED;
    private bool canMove = false;

    private void Awake() => cameraTransform = Camera.main.transform;

    public void SetMovement(bool status) => canMove = status;

    private void Update()
    {
        if (canMove)
        {
            currentPosition += currentSpeed * Time.deltaTime;
            UpdatePosition(smoothing * Time.deltaTime);
        }
    }

    public void SetDefaultPosition(RoadSpline newRoad)
    {
        roadSpline = newRoad;
        currentPosition = 0;
        currentSpeed = START_SPEED;
        UpdatePosition(1);
    }

    private void UpdatePosition(float currentSmoothing)
    {
        // Sample the road.
        Vector3 samplePoint = roadSpline.SampleSpline(currentPosition);
        Vector3 currentRoadTangent = roadSpline.SampleTangent3D(currentPosition);

        // Set the camera's position and rotation values.
        cameraTransform.forward = Vector3.Lerp(cameraTransform.forward, currentRoadTangent, currentSmoothing);
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, samplePoint + (Vector3.up * cameraYOffset), currentSmoothing);
    }
}