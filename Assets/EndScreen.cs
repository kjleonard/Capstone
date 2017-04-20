using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;

public class EndScreen : MonoBehaviour {

    public Text lblAvoidedCount;
    public Text lblDistance;
    public Text lblSpeed;
    public Text lblLeftEMG;
    public Text lblRightEMG;

    // Use this for initialization
    void Start () {

        Cursor.visible = true;
        VRSettings.enabled = false;

        int obstaclesHit = PlayerPrefs.GetInt("obstaclesHit");
        int obstacleTotal = PlayerPrefs.GetInt("obstacleTotal");
        int duration = PlayerPrefs.GetInt("selDuration");
        float speed = PlayerPrefs.GetFloat("selSpeed");
        float leftEMG = PlayerPrefs.GetFloat("countLeftEMG");
        float rightEMG = PlayerPrefs.GetFloat("countRightEMG");

        lblAvoidedCount.text = (obstacleTotal - obstaclesHit).ToString() + " / " + obstacleTotal.ToString();
        lblDistance.text = (duration * speed * 1000 / 60).ToString() + " meters";
        lblSpeed.text = speed + " kilometers/hour";
        lblLeftEMG.text = leftEMG + " mV";
        lblRightEMG.text = rightEMG + " mV";
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
