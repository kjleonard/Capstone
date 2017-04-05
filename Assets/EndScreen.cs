using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour {

    Text lblAvoidedCount;
    Text lblDistance;

	// Use this for initialization
	void Start () {

        int obstaclesHit = PlayerPrefs.GetInt("selObstaclesHit");
        int obstacleTotal = PlayerPrefs.GetInt("selObstacleFrequency");
        int duration = PlayerPrefs.GetInt("selDuration");
        float speed = PlayerPrefs.GetFloat("selSpeed");

        lblAvoidedCount.text = (obstacleTotal - obstaclesHit).ToString() + " / " + obstacleTotal.ToString();
        lblDistance.text = (duration * speed).ToString() + " meters";

        //GameObject lblAvoidedCount = GameObject.Find("lblAvoidedCount");
        //lblAvoidedCount.GetComponent
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
