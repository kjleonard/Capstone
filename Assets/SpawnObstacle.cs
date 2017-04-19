using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacle : MonoBehaviour {

	private Transform spawnLocation;
	public GameObject playerPos;
	public GameObject obstacle;


	private float NextPosToGenerate = 50f;
	private GameObject generatedCube;
	private float x, y, z;

	// Use this for initialization
	void Start () {
		x = 0f;
		y = 10f;
		z = playerPos.transform.position.z - 40f;
		NextPosToGenerate = z;
        PlayerPrefs.SetInt("obstaclesHit", 0);
	}

	void Update () {
		if (playerPos.transform.position.z < NextPosToGenerate) {
			z = playerPos.transform.position.z - 40f;
			NextPosToGenerate = z;
			Object.Destroy (generatedCube);
			generatedCube = Instantiate(obstacle, new Vector3(x,y,z), Quaternion.Euler(90, 0, 90)) as GameObject;

			Rigidbody gameObjectsRigidBody = generatedCube.AddComponent<Rigidbody>();
			gameObjectsRigidBody.mass = 5; 
			gameObjectsRigidBody.useGravity = true;

			BoxCollider bc = generatedCube.AddComponent<BoxCollider> ();
		}  else {

		}


	}
}
