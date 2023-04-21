// Title: PlayerMovement
// Author: Skyler Riggle

using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{
    private Transform cameraTransform;
    [SerializeField] private Vector3 cameraOffset = Vector3.zero;

    private RoadSpline roadSpline;

    private float currentPosition = 0;

    private void Awake() => cameraTransform = Camera.main.transform;

    public void SetDefaultPosition(RoadSpline newRoad)
    {
        roadSpline = newRoad;
        currentPosition = 0;
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        // Sample the road.
        Vector3 samplePoint = roadSpline.SampleSpline(currentPosition);
        Vector3 currentRoadTangent = roadSpline.SampleTangent3D(0);

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