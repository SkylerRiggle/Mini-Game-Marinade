using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightCamMove : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 offset = new Vector3(0, 50, 0);
    private Vector2 limits = new Vector2(20, 0);
    private float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying)
        {
            transform.localPosition = offset;
        }
        FollowTarget(player);
    }

    private void LateUpdate()
    {
        Vector3 localPos = transform.localPosition;

        // Clamp camera movement within limits
        transform.localPosition = new Vector3(Mathf.Clamp(localPos.x, offset.x - limits.x, offset.x + limits.x),
            Mathf.Clamp(localPos.y, offset.y - limits.y, offset.y + limits.y), localPos.z);
    }

    public void FollowTarget(Transform t)
    {
        Vector3 localPos = transform.localPosition;
        Vector3 targetLocalPos = t.transform.localPosition;

        // Smoothly move camera to follow player within bounds
        transform.localPosition = Vector3.SmoothDamp(localPos, new Vector3(targetLocalPos.x + offset.x,
            targetLocalPos.y + offset.y, localPos.z), ref velocity, smoothTime);
    }
}
