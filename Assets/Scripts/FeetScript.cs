using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetScript : MonoBehaviour {
	public GameObject leftFoot;
	public GameObject rightFoot;


	Vector3 up = new Vector3(0,.5f,0);
	Vector3 down = new Vector3(0,-.5f,0);
	bool isLeftUp = false;
	bool isRightup = false;


	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("q") & !isLeftUp) {
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
		}
	}
}
