using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacle : MonoBehaviour {

	private Transform spawnLocation;
	public GameObject playerPos;
	public GameObject obstacle;
    public UnityEngine.UI.Text Feedback_Text;
    public System.Timers.Timer timer;

	private float NextPosToGenerate = 50f;
	private GameObject generatedCube;
	private float x, y, z;
    public  bool obstacleHit;
    private bool feedbackTimerFired;
    private bool firstRun;  // Used to not display feedback text on first obstacle spawn


	// Use this for initialization
	void Start () {
		x = 0f;
		y = 10f;
		z = playerPos.transform.position.z - 40f;
		NextPosToGenerate = z;
        obstacleHit = false;
        PlayerPrefs.SetInt("obstaclesHit", 0);
        Feedback_Text.enabled = false;
        timer = new System.Timers.Timer(5000);
        timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent_Elapsed);
        timer.AutoReset = false;
        timer.Enabled = false;
        feedbackTimerFired = false;
        firstRun = true;
    }

    // Once the selected duration has been reached, move to the end screen
    private void OnTimedEvent_Elapsed(System.Object source, System.Timers.ElapsedEventArgs e)
    {
        // Workaround for multi-threading and Unity calls
        feedbackTimerFired = true;
    }



    void Update () {
        if (feedbackTimerFired)
        {
            Feedback_Text.enabled = false;
            timer.Enabled = false;
            feedbackTimerFired = false;
        }
        if (playerPos.transform.position.z < NextPosToGenerate)
        {
            z = playerPos.transform.position.z - 40f;
            NextPosToGenerate = z;
            if (obstacleHit)    // Increase obstaclesHit count and reset for next obstacle
            {
                PlayerPrefs.SetInt("obstaclesHit", (PlayerPrefs.GetInt("obstaclesHit") + 1));
                obstacleHit = false;
            }
            else if (!firstRun)    // Display feedback
            {
                System.Random rand = new System.Random();
                int feedback = rand.Next(4);
                string feedbackText = "";
                switch (feedback)
                {
                    case 0:
                        feedbackText = "Awesome!";
                        break;
                    case 1:
                        feedbackText = "Great Job!";
                        break;
                    case 2:
                        feedbackText = "Nice Work!";
                        break;
                    case 3:
                        feedbackText = "Keep it up!";
                        break;
                    default:
                        feedbackText = "This should never be seen";
                        break;
                }
                Feedback_Text.text = feedbackText;
                Feedback_Text.enabled = true;
                timer.Enabled = true;
            }
            else
            {
                firstRun = false;
            }
            Destroy(generatedCube);

			generatedCube = Instantiate(obstacle, new Vector3(x,y,z), Quaternion.Euler(0, 0, 0)) as GameObject;
			Rigidbody r = generatedCube.GetComponent<Rigidbody>();
			r.angularDrag = 0;
			r.isKinematic = false;
			r.useGravity = true;

		}
        else
        {

		}

	}



}
