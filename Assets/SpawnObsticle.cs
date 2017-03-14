using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObsticle : MonoBehaviour {

	private Transform spawnLocation;
	public GameObject playerPos;
	public GameObject obsticle;


	private float NextPosToGenerate = 50f;
	private GameObject generatedCube;
	private float x, y, z;

	// Use this for initialization
	void Start () {
		x = 0f;
		y = 10f;
		z = playerPos.transform.position.z - 40f;
		NextPosToGenerate = z;
	}

	// Update is called once per frame
	void Update () {
		if (playerPos.transform.position.z < NextPosToGenerate) {
			z = playerPos.transform.position.z - 40f;
			NextPosToGenerate = z;
			Object.Destroy (generatedCube);
			generatedCube = Instantiate(obsticle, new Vector3(x,y,z), Quaternion.Euler(0, 0, 0)) as GameObject;

		} else {

		}


	}
}
