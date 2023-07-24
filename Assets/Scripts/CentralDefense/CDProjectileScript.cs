using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDProjectileScript : MonoBehaviour
{
    private float minX = -25f;
    private float maxX = 25f;
    private float minY = -15f;
    private float maxY = 15f;
    private int speed = 10;
    private bool hasHit = false;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * Time.deltaTime * speed;

        float projX = transform.position.x;
        float projY = transform.position.y;

        // If projectile leaves bounds, destroy it
        if (projX < minX || projX > maxX || projY < minY || projY > maxY)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Trigger collision if it is an enemy and collision has not been triggered yet
        if (collision.gameObject.CompareTag("Enemy") && !hasHit)
        {
            // Projectile has hit
            hasHit = true;
            // Trigger enemy death
            collision.gameObject.GetComponent<CDEnemyScript>().Die();
            // Destroy projectile
            Destroy(gameObject);
        }
    }
}
