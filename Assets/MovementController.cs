using UnityEngine;
using System;
using System.Collections;

public class MovementController : MonoBehaviour {

    private Vector3 velocity;
    private float speed;
    private Vector3 dir;

        void Start()
        {
            velocity = Vector3.zero;
            dir = transform.forward;
            speed = 1;
        }

        // Update is called once per frame
        void Update () {

        if (Input.GetKeyDown("w"))
            speed += 15f;
        else if (Input.GetKeyDown("s"))
            speed -= 15f;

        velocity = Vector3.zero;
        velocity = dir / dir.magnitude * Time.deltaTime * speed  * (float)(Math.Log(Convert.ToInt32(speed), 25)) / 35; //calculate velocity
        transform.position += velocity; //move character forward

    }
}
