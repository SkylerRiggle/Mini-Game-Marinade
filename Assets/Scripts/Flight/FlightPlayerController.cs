using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightPlayerController : MonoBehaviour
{
    private Rigidbody rb;

    // Use camera for relative movement
    [SerializeField] private Camera cam = null;

    // Speed of player movement
    private float speed = 70f;
    // Speed of player rotation
    private float lookSpeed = 4000f;
    // Speed of forward movement, only used for lose sequence
    private float fwdSpeed = 190f;
    // Limit for horizontal tilt
    private float leanLimit = 40f;

    // Used to determine when to do lose sequence
    private bool hasLost = false;

    public Transform lookObject;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasLost)
        {
            // Get movement along x and y, no input along z
            float xMov = Input.GetAxis("Horizontal");
            float yMov = Input.GetAxis("Vertical");

            PlayerMovement(xMov, yMov);
        }
        else
        {
            // Move player forward while camera is stopped
            transform.Translate(transform.forward * fwdSpeed * Time.deltaTime);
        }
    }

    private void PlayerMovement(float xMov, float yMov)
    {
        // Move the player
        // Need to move locally to be able to rotate along z
        LocalMove(xMov, yMov);
        // Rotate player based on movement
        RotationLook(xMov, yMov);
        // Rotate player along z if moving horizontally
        HorizontalLean(xMov, leanLimit, 0.1f);
    }

    private void LocalMove(float xMov, float yMov)
    {
        Vector3 relX = cam.transform.right * xMov;
        Vector3 relY = cam.transform.up * yMov;

        transform.localPosition += new Vector3(relX.x, relY.y, 0) * speed * Time.deltaTime;
        // Constrain player movement
        ConstrainPlayer();
    }

    private void ConstrainPlayer()
    {
        // Stores player position as relative to viewport
        Vector3 constrainedPos = Camera.main.WorldToViewportPoint(transform.position);
        // Clamps x and y values of position
        constrainedPos.x = Mathf.Clamp01(constrainedPos.x);
        constrainedPos.y = Mathf.Clamp01(constrainedPos.y);

        // Convert back relative to world and clamp player movement
        transform.position = Camera.main.ViewportToWorldPoint(constrainedPos);
    }

    private void RotationLook(float xMov, float yMov)
    {
        lookObject.parent.position = Vector3.zero;

        // Set reference object for which way to point the arrow towards
        // Higher z value results in lower/tighter rotation limit
        lookObject.localPosition = new Vector3(xMov, yMov, 3);

        // Rotate the arrow towards lookObject's rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookObject.position),
            Mathf.Deg2Rad * lookSpeed * Time.deltaTime);
    }

    private void HorizontalLean(float axis, float leanLimit, float lerpTime)
    {
        Vector3 playerEulerAngles = transform.localEulerAngles;

        // Lean the player depending on direction of input
        transform.localEulerAngles = new Vector3(playerEulerAngles.x, playerEulerAngles.y, Mathf.LerpAngle(playerEulerAngles.z, -axis * leanLimit,
            lerpTime));
    }

    public void OnHit()
    {
        // Prevent arrow from rotating after it hits
        rb.freezeRotation = true;
        // Disable movement of entire arrow
        enabled = false;
    }

    public void OnLose()
    {
        // Enable lose sequence
        hasLost = true;
        // Set player facing straight forward
        transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
    }
}
