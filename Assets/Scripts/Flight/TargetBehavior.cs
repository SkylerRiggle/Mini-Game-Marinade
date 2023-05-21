using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBehavior : MonoBehaviour
{
    [SerializeField] private GameEvent onHit;

    private Vector3 startPos = new Vector3(0, 50, 1500);
    private bool hit = false;

    // Speed of curved movement
    private float radius = 1;
    // Amount of curved movement
    private float xAmp = 100f;
    private float yAmp = 25f;

    // Movement pattern, randomized per play
    private int pattern = 0;
    // Initial radian value to randomize target starting position
    private float initRadian = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = startPos;
        // Randomize movement pattern
        pattern = Random.Range(0, 6);
        // Randomize starting position
        initRadian = Random.Range(0f, 6.28f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!hit)
        {
            float newX = startPos.x;
            float newY = startPos.y;

            switch (pattern)
            {
                // Infinity pattern
                case 0:
                    newX += (transform.right * Mathf.Cos((initRadian + Time.fixedTime) * radius) * xAmp).x;
                    newY += (transform.up * Mathf.Sin((initRadian + Time.fixedTime) * 2 * radius) * yAmp).y;
                    break;
                // 8 pattern
                case 1:
                    newX += (transform.right * Mathf.Cos((initRadian + Time.fixedTime) * 2 * radius) * xAmp).x;
                    newY += (transform.up * Mathf.Sin((initRadian + Time.fixedTime) * radius) * yAmp).y;
                    break;
                // Counterclockwise oval pattern
                case 2:
                    newX += (transform.right * Mathf.Cos((initRadian + Time.fixedTime) * radius) * xAmp).x;
                    newY += (transform.up * Mathf.Sin((initRadian + Time.fixedTime) * radius) * yAmp).y;
                    break;
                // Clockwise oval pattern
                case 3:
                    newX += (transform.right * Mathf.Cos((initRadian + Time.fixedTime) * radius) * xAmp).x;
                    newY += (transform.up * Mathf.Sin((initRadian + Time.fixedTime) * -radius) * yAmp).y;
                    break;
                // Vertical zigzag pattern
                case 4:
                    newX += (transform.right * Mathf.Cos((initRadian + Time.fixedTime) * radius) * xAmp).x;
                    newY += (transform.up * Mathf.Sin((initRadian + Time.fixedTime) * 4 * radius) * yAmp).y;
                    break;
                // Horizontal zigzag pattern
                case 5:
                    newX += (transform.right * Mathf.Cos((initRadian + Time.fixedTime) * radius) * xAmp).x;
                    newY += (transform.up * Mathf.Sin((initRadian + Time.fixedTime) / 3 * radius) * yAmp).y;
                    break;
                // Default to infinity pattern
                default:
                    newX += (transform.right * Mathf.Cos((initRadian + Time.fixedTime) * radius) * xAmp).x;
                    newY += (transform.up * Mathf.Sin((initRadian + Time.fixedTime) * 2 * radius) * yAmp).y;
                    break;
            }


            Vector3 newPos = startPos;

            newPos.x = newX;
            newPos.y = newY;
            newPos.z = transform.position.z;


            transform.position = newPos;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hit = true;
            onHit?.Invoke();
        }
    }
}
