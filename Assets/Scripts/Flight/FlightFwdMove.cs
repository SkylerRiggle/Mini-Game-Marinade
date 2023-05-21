using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightFwdMove : MonoBehaviour
{
    private float speed = 190f;
    private bool hasEnded = false;

    // Update is called once per frame
    void Update()
    {
        // Move player and camera forward if game has not ended
        if (!hasEnded)
        {
            Vector3 newPos = transform.position;

            newPos.z = transform.position.z + (transform.forward * speed * Time.deltaTime).z;

            transform.position = newPos;
        }
    }

    public void OnEnd()
    {
        hasEnded = true;
    }
}
