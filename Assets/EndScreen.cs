using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;

public class EndScreen : MonoBehaviour {

    public Text lblAvoidedCount;
    public Text lblDistance;
    public Text lblFeedback;
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
		lblDistance.text = ((duration/60000) * speed * 1000 / 60).ToString() + " meters";
        lblSpeed.text = speed + " kilometers/hour";
        lblLeftEMG.text = leftEMG*1000 + " mV";
        lblRightEMG.text = rightEMG*1000 + " mV";

        double feedback = (obstacleTotal - obstaclesHit) / obstacleTotal;
        string feedbackText = "";
        if (feedback < .25) // < 25% obstacles avoided
        {
            feedbackText = "Nice Work!";
        }
        else if (feedback < .5) // < 50% obstacles avoided
        {
            feedbackText = "Great Job!";
        }
        else if (feedback < .75)    // < 75% obstacles avoided
        {
            feedbackText = "Awesome!";
        }
        else    // < 100% obstacles avoided
        {
            feedbackText = "Excellent!";
        }
        lblFeedback.text = feedbackText;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
