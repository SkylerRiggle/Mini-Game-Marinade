// Title: PlayerMovement
// Author: Skyler Riggle

using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{
    private Transform cameraTransform;

    private RoadSpline _roadSpline;
    public RoadSpline roadSpline { set { _roadSpline = value; } }

    private void Awake() => cameraTransform = Camera.main.transform;
}