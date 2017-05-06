using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetectionScript : MonoBehaviour {

    /** @file
     * @brief Used for testing collisions with obstacles and other game objects; prints the name
     * of hit obstacles to Debug.Log. */

    void OnTriggerEnter(Collider col){
		if (col.gameObject.name == "Obstacle" || col.gameObject.name == "Obstacle(Clone)") {
			Debug.Log("We hit obstacle");
			GameObject theSpawner = GameObject.Find ("spawner");
			SpawnObstacle spawner = theSpawner.GetComponent<SpawnObstacle> ();
			spawner.obstacleHit = true;
		}

		Debug.Log ("We Hit : " + col.gameObject.name);

	}
}
