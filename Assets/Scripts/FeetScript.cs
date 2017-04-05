using System.Collections;
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
        /*
        if (Input.GetKeyDown("q") & leftFoot.transform.position.y + .5f < ceiling)
        {
            leftFoot.transform.Translate(up);
            rightFoot.transform.Translate(up);
        }

        if (Input.GetKeyDown("a") & leftFoot.transform.position.y - 0.5f > floor)
        {
            leftFoot.transform.Translate(down);
            rightFoot.transform.Translate(down);
        }
        if (Input.GetKeyDown ("f") &  leftFoot.transform.position.z - .5f > Player.transform.position.z - max_forward ) {
			leftFoot.transform.Translate(forward);
            rightFoot.transform.Translate(forward);

        }

		if (Input.GetKeyDown ("b") & rightFoot.transform.position.z + 0.5f < Player.transform.position.z - max_backward) {
            leftFoot.transform.Translate(backward);
            rightFoot.transform.Translate(backward);
		}             
        */

        //leftFoot.transform.position.Set(-.96f, playerMovement.leftY - 11.1f, 2.8f);
        //rightFoot.transform.position.Set(.6f, playerMovement.rightY - 12f, 2.8f);


        if (Math.Abs(playerMovement.leftY) > .2f
            & leftFoot.transform.position.y + playerMovement.leftY > floor
            & leftFoot.transform.position.y + playerMovement.leftY < ceiling)
            leftFoot.transform.Translate(0, playerMovement.leftY, 0);
        if (Math.Abs(playerMovement.rightY) > .2f
            & rightFoot.transform.position.y + playerMovement.rightY > floor
            & rightFoot.transform.position.y + playerMovement.rightY < ceiling)
            rightFoot.transform.Translate(0, playerMovement.rightY, 0);


        Debug.Log(string.Format("left z = {0}, right z = {2}, player z = {1}", leftFoot.transform.position.z, Player.transform.position.z, rightFoot.transform.position.z));
        //Debug.Log(string.Format("right Y = {0}", playerMovement.rightY));
    }
}
