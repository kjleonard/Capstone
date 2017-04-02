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
        floor = -9.49f;
        ceiling = 9.71f;
        //Pending wether we decide to move along z and x axis we will. If so the max_forward and max_backward valuest will have to be subtracted from the z coordinate of the player object.
        max_left = -1.5f;
        max_right = 1.5f;
        max_forward = 3.8f;
        max_backward = 2.3f;
    }

	// Update is called once per frame
	void Update () {
        /*if (Input.GetKeyDown ("q") & !isLeftUp) {
			leftFoot.transform.Translate(up);
			isLeftUp = true;
		}

		if (Input.GetKeyDown ("a") & isLeftUp) {
			leftFoot.transform.Translate(down);
			isLeftUp = false;
		}
        
		if (Input.GetKeyDown ("e") & !isRightup) {
			rightFoot.transform.Translate(up);
			isRightup = true;
		}

		if (Input.GetKeyDown ("d") & isRightup) {
			rightFoot.transform.Translate(down);
			isRightup = false;
		}*/
        
        leftFoot.transform.position.Set(-.96f, playerMovement.leftY - 11.1f, 2.8f);
        //rightFoot.transform.position.Set(.6f, playerMovement.rightY - 12f, 2.8f);

        if ( Math.Abs(playerMovement.leftY) > 1.0f
            && leftFoot.transform.position.y + playerMovement.leftY > floor 
            && leftFoot.transform.position.y + playerMovement.leftY < ceiling)
            leftFoot.transform.Translate(0, playerMovement.leftY, 0);
        if ( Math.Abs(playerMovement.leftY) > 1.0f
            && rightFoot.transform.position.y + playerMovement.rightY > floor
            && rightFoot.transform.position.y + playerMovement.rightY < ceiling)
            rightFoot.transform.Translate(0, playerMovement.rightY, 0);

        Debug.Log(string.Format("leftfoot position = {0}", leftFoot.transform.position));
        Debug.Log(string.Format("leftfoot z position = {0}", leftFoot.transform.position.z));
    }
}
