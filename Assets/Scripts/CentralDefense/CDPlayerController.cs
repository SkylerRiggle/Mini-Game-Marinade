using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDPlayerController : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private AudioClip explosion;

    public GameEvent onLose;

    private bool canShoot = true;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        // Retrieve input magnitude, needs to be at least 0.6 to register
        float inputMag = Mathf.Clamp01(new Vector2(inputX, inputY).sqrMagnitude);

        if (inputMag >= 0.6f && canShoot)
        {
            // Retrieve input angle
            float inputAngle = GetInputAngle(inputX, inputY);

            float rotAngle;

            // Set rotation of player based on input angle
            if (inputAngle < 22.5 || inputAngle > 337.5)
            {
                rotAngle = 0;
            }
            else if (22.5 <= inputAngle && inputAngle <= 67.5)
            {
                rotAngle = 45;
            }
            else if (67.5 < inputAngle && inputAngle < 112.5)
            {
                rotAngle = 90;
            }
            else if (112.5 <= inputAngle && inputAngle <= 157.5)
            {
                rotAngle = 135;
            }
            else if (157.5 < inputAngle && inputAngle < 202.5)
            {
                rotAngle = 180;
            }
            else if (202.5 <= inputAngle && inputAngle <= 247.5)
            {
                rotAngle = 225;
            }
            else if (247.5 < inputAngle && inputAngle < 292.5)
            {
                rotAngle = 270;
            }
            else
            {
                rotAngle = 315;
            }

            // Set angle of player, need to subtract 90 because it starts facing 90 degrees
            transform.localRotation = Quaternion.Euler(0, 0, rotAngle - 90);

            // Spawn and shoot a projectile
            Instantiate(projectile, Vector2.zero, Quaternion.Euler(0, 0, rotAngle - 90));

            // Disable shoot until player resets the joystick to neutral
            canShoot = false;
        }
        else if (inputMag == 0)
        {
            // Reenable shoot
            canShoot = true;
        }
    }

    private float GetInputAngle(float inputX, float inputY)
    {
        // Angle in radians
        float radAngle = Mathf.Atan2(inputY, inputX);
        // Angle in degrees
        float degAngle = radAngle * Mathf.Rad2Deg;

        // Convert degrees to positive value
        if (degAngle < 0)
        {
            degAngle += 360;
        }

        return degAngle;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If hit by enemy, die and destory all enemies
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GetComponent<BoxCollider2D>().enabled = false;
            AudioSource.PlayClipAtPoint(explosion, transform.position, 1.5f);
            animator.enabled = true;
            onLose?.Invoke();
            Destroy(gameObject, 1f);
        }
    }
}
