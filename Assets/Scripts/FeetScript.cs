using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetScript : MonoBehaviour {
	public GameObject leftFoot;
	public GameObject rightFoot;
    public MovementController playerMovement;

	Vector3 up = new Vector3(0,.5f,0);
	Vector3 down = new Vector3(0,-.5f,0);
	bool isLeftUp = false;
	bool isRightup = false;


	// Use this for initialization
	void Start () {

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
        rightFoot.transform.Translate(0, playerMovement.rightY, 0);
        Debug.Log(string.Format("leftfoot position = {0}", rightFoot.transform.position));
    }
}
