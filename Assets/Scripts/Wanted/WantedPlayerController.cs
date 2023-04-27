using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WantedPlayerController : MonoBehaviour
{
    public GameEvent onWin;
    public GameEvent onLose;

    private Wanted wantedManager;

    // Boundaries for player movement
    private float rightBoundary = 12f;
    private float leftBoundary = -12f;
    private float ceilBoundary = 5f;
    private float floorBoundary = -5f;
    private float validTime = 1f;   // Player must wait motionless on fruit for 1 second to validate their victory

    private float stillTimer = 0f;  // Timer for how long the player is motionless while fruit has been found
    private bool found = false; // Whether fruit is currently in player's circle
    private bool gameOver = false;  // Used for game over sequence
    private bool canWin = false;    // Used to allow player to be able to win, deactivated until objective finishes displaying

    private Vector3 scaleChange = new Vector3(1.0f, 1.0f, 0f);

    [SerializeField] private float speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        wantedManager = GameObject.Find("Wanted Game").GetComponent<Wanted>();

        // Set player in center
        transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            // Get movement along x and y
            float xMov = Input.GetAxisRaw("Horizontal");
            float yMov = Input.GetAxisRaw("Vertical");

            PlayerMovement(xMov, yMov);

            // Activate win condition from the manager
            if (canWin)
            {
                // If object is found and player is still, increase timer
                if (found && xMov == 0 && yMov == 0)
                {
                    stillTimer += Time.deltaTime;

                    // If validTime is reached or if time is over and player is still, trigger win 
                    if (stillTimer >= validTime || wantedManager.getTime() < 0)
                    {
                        onWin?.Invoke();
                        // Disable player movement
                        gameOver = true;
                    }
                }
                else
                {
                    // Reset timer if player moves
                    stillTimer = 0;

                    // Trigger loss if player is still moving or has not found the fruit
                    if (wantedManager.getTime() < 0)
                    {
                        onLose?.Invoke();
                        // Disable player movement
                        gameOver = true;

                    }
                }
            }

        }
        else
        {
            transform.localScale += scaleChange;
        }
    }

    private void PlayerMovement(float xMov, float yMov)
    {
        Vector3 movement = new Vector3(xMov, yMov, 0).normalized;

        // Move the player
        transform.Translate(movement * speed * Time.deltaTime);

        // Contrain player movement if they moved out of bounds
        PlayerBoundaries();
    }

    private void PlayerBoundaries()
    {
        // Clamp the position if they are moving out of bounds
        float xClamp = Mathf.Clamp(transform.position.x, leftBoundary, rightBoundary);
        float yClamp = Mathf.Clamp(transform.position.y, floorBoundary, ceilBoundary);
        Vector3 boundedMovement = new Vector3(xClamp, yClamp, 0);

        // Set the position inside the bounds
        transform.position = boundedMovement;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        found = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        found = false;
    }

    public void AllowWin(bool input)
    {
        canWin = input;
    }
}
