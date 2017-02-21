using UnityEngine;
using System;
using System.Threading;
using System.Collections;

public class MovementController : MonoBehaviour {

    private Vector3 velocity;
    private float speed;
    private Vector3 dir;
    
    private int x;
    private int y;
    private int z;
    

        void Start()
        {
            velocity = Vector3.zero;
            dir = transform.forward;
            speed = 1;
            
            //Creates Child Thread to retrieve Postional Information
            ThreadStart childref_getXYZ = new ThreadStart(getXYZ);
            Thread childThread_getXYZ = new Thread(childref_getXYZ);
            childThread_getXYZ.Start();
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
    
        void getXYZ ()
        {
            //here is where the TCP Client will run to communicate with the Control Application
        }
}
