﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FeetScript : MonoBehaviour {
	public GameObject leftFoot;
	public GameObject rightFoot;
    public MovementController playerMovement;

	Vector3 up = new Vector3(0,.5f,0);
	Vector3 down = new Vector3(0,-.5f,0);
    Vector3 left = new Vector3(.5f, 0, 0);
    Vector3 right = new Vector3(-.5f, 0, 0);
    Vector3 forward = new Vector3(0, 0, -.5f);
    Vector3 backward = new Vector3(0, 0, .5f);

    bool isLeftUp = false;
	bool isRightup = false;

    public GameObject Player;

    private float floor;
    private float ceiling;
    private float max_left;
    private float max_right;
    private float max_forward;
    private float max_backward;


    //Used to protect data after check from the child thread.
    private float left_x;
    private float left_y;
    private float left_z;
    private float right_x;
    private float right_y;
    private float right_z;

    // Use this for initialization
    void Start () {
        //Set boundaries for Feet
        floor = 1.3f;
        ceiling = 2.91f;
        //Pending wether we decide to move along z and x axis we will. If so the max_forward and max_backward valuest will have to be subtracted from the z coordinate of the player object.
        max_left = 1.75f;
        max_right = -1.75f;
        max_forward = 3.8f;
        max_backward = 2.3f;
    }

	// Update is called once per frame
	void Update () {
        //This keeps the child thread from modifying the values used for translation after they are checked to be within the boundaries.
        left_x = playerMovement.leftX;
        left_y = playerMovement.leftY;
        left_z = playerMovement.leftZ;
        right_x = playerMovement.rightX;
        right_y = playerMovement.rightY;
        right_z = playerMovement.rightZ;


        /*
        if (Input.GetKeyDown("q") & leftFoot.transform.position.x - .5f > max_right)
        {
            leftFoot.transform.Translate(right);
            rightFoot.transform.Translate(right);
        }

        if (Input.GetKeyDown("a") & leftFoot.transform.position.x + 0.5f < max_left)
        {
            leftFoot.transform.Translate(left);
            rightFoot.transform.Translate(left);
        }
        if (Input.GetKeyDown("f") & leftFoot.transform.position.z - .5f > Player.transform.position.z - max_forward)
        {
            leftFoot.transform.Translate(forward);
            rightFoot.transform.Translate(forward);

        }

        if (Input.GetKeyDown("b") & rightFoot.transform.position.z + 0.5f < Player.transform.position.z - max_backward)
        {
            leftFoot.transform.Translate(backward);
            rightFoot.transform.Translate(backward);
        }
        */


        //leftFoot.transform.position.Set(-.96f, playerMovement.leftY - 11.1f, 2.8f);
        //rightFoot.transform.position.Set(.6f, playerMovement.rightY - 12f, 2.8f);
        //Debug.Log(String.Format("Left Z: {0}, Right Z: {1}", left_z, right_z));
        left_z = left_z * -1;
        right_z -= right_z * -1;

        left_x *= -1;
        right_x *= -1;

        left_y += 1.16f;
        right_y += 1.16f;
        
        if (Math.Abs(left_x) > .3f
                & leftFoot.transform.position.x + left_x > max_right
			& leftFoot.transform.position.x + left_x < max_left){
			leftFoot.GetComponent<Rigidbody>().MovePosition(new Vector3(leftFoot.transform.position.x + left_x/4,leftFoot.transform.position.y,leftFoot.transform.position.z));
            //leftFoot.transform.Translate(left_x / 4, 0, 0);
				}
        if (Math.Abs(right_x) > .3f
                & rightFoot.transform.position.x + right_x > max_right
			& rightFoot.transform.position.x + right_x < max_left)
            rightFoot.transform.Translate(right_x / 4, 0, 0);
		if (Math.Abs (left_y) > .18f
		          & leftFoot.transform.position.y + left_y > floor
		          & leftFoot.transform.position.y + left_y < ceiling) {
			leftFoot.GetComponent<Rigidbody>().MovePosition(new Vector3(leftFoot.transform.position.x,leftFoot.transform.position.y+left_y/2,leftFoot.transform.position.z));
			//leftFoot.transform.Translate(0, left_y / 2, 0);
		} 
        if (Math.Abs(right_y) > .18f
            & rightFoot.transform.position.y + right_y > floor
            & rightFoot.transform.position.y + right_y < ceiling)
            rightFoot.transform.Translate(0, right_y / 2, 0);
		if (Math.Abs (left_z) > .25f
		          & leftFoot.transform.position.z + left_z < Player.transform.position.z - max_backward
		          & leftFoot.transform.position.z + left_z > Player.transform.position.z - max_forward) {
			leftFoot.GetComponent<Rigidbody>().MovePosition(new Vector3(leftFoot.transform.position.x,leftFoot.transform.position.y,leftFoot.transform.position.z+left_z/2));
			//leftFoot.transform.Translate (0, 0, left_z / 2);
		}
        if (Math.Abs(right_z) > .25f
            & rightFoot.transform.position.z + right_z < Player.transform.position.z - max_backward
            & rightFoot.transform.position.z + right_z > Player.transform.position.z - max_forward)
            rightFoot.transform.Translate(0, 0, right_z / 2);



        //Debug.Log(string.Format("left x = {0}, right x = {2}, player z = {1}", leftFoot.transform.position.x, Player.transform.position.z, rightFoot.transform.position.x));
        //Debug.Log(string.Format("right Y = {0}", right_y));
    }


	void OnTriggerEnter (Collider col)
	{
		Debug.Log ("WE HIT SOMETHING");
		Destroy (col.gameObject);
	}

	void OnControllerColliderHit(ControllerColliderHit col){
		Debug.Log ("EKEKEKEKEKEKKEE");
	}
}
