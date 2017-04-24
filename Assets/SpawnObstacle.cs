using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacle : MonoBehaviour {

	private Transform spawnLocation;
	public GameObject playerPos;
	public GameObject obstacle;
    public UnityEngine.UI.Text Feedback_Text;
    public System.Timers.Timer feedbackTimer;
	public System.Timers.Timer obstacleSpawnTimer;


	private float NextPosToGenerate = 50f;
	private GameObject generatedCube;
	private float x, y, z;
    public  bool obstacleHit;
    private bool feedbackTimerFired;
    private bool firstRun;  // Used to not display feedback text on first obstacle spawn
	private bool obstacleTimerFired;
	private int obstacleFrequency;
	// Use this for initialization
	void Start () {
		x = 0f;
		y = 10f;
		z = playerPos.transform.position.z - 40f;
		NextPosToGenerate = z;
        obstacleHit = false;
        PlayerPrefs.SetInt("obstaclesHit", 0);
        Feedback_Text.enabled = false;
        feedbackTimer = new System.Timers.Timer(5000);
        feedbackTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent_Elapsed);
        feedbackTimer.AutoReset = false;
        feedbackTimer.Enabled = false;
        feedbackTimerFired = false;
        firstRun = true;

		obstacleFrequency = PlayerPrefs.GetInt("selObstacleFrequency");
		int durationPref = PlayerPrefs.GetInt ("selDuration");
		int obstacleCount = 0;
		int obstaclesInterval = 0;
		// To-Do: Figure out best way to accomodate for # per minute in combination with speed
		if (obstacleFrequency == 1)
		{   // Low - 4 per minute = 1/15s
			obstacleCount =4;
			obstaclesInterval = 15000;
		}
		else if (obstacleFrequency == 2)
		{   // Medium - 6 per minute = 1/10s
			obstacleCount =4;
			obstaclesInterval = 10000;
		}
		else if (obstacleFrequency == 3)
		{   // High - 10 per minute = 1/6s
			obstacleCount =4;
			obstaclesInterval = 6000;
		}
		PlayerPrefs.SetInt("obstacleTotal", obstacleCount * (durationPref/60000));

		obstacleSpawnTimer = new System.Timers.Timer((double) obstaclesInterval);
		obstacleSpawnTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnObstacleEvent_Elapsed);
		obstacleSpawnTimer.AutoReset = true;
		obstacleSpawnTimer.Enabled = true;
		obstacleTimerFired = false;

		int obstacleType = PlayerPrefs.GetInt("selObstacleType");
		// To-Do: Should we change this from a dropdown to checkboxes to accomodate randomized selection of obstacles?
		switch (obstacleType)
		{
		case 0: // None
			break;
		case 1: // Boxes
			break;
		case 2: // Strings
			break;
		case 3: // Other
			break;
		}

    }

    // Display Feedback
    private void OnTimedEvent_Elapsed(System.Object source, System.Timers.ElapsedEventArgs e)
    {
        // Workaround for multi-threading and Unity calls
        feedbackTimerFired = true;

    }
		
	private void OnObstacleEvent_Elapsed(System.Object source, System.Timers.ElapsedEventArgs e)
	{
		obstacleTimerFired = true;
	}

    void Update () {
		if (feedbackTimerFired)
        {
            Feedback_Text.enabled = false;
            feedbackTimer.Enabled = false;
            feedbackTimerFired = false;
        }
		if (obstacleTimerFired && obstacleFrequency > 0)
        {
            z = playerPos.transform.position.z - 40f;
            NextPosToGenerate = z;
            if (obstacleHit)    // Increase obstaclesHit count and reset for next obstacle
            {
                PlayerPrefs.SetInt("obstaclesHit", (PlayerPrefs.GetInt("obstaclesHit") + 1));
                obstacleHit = false;
            }
            else if (!firstRun )    // Display feedback
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
                feedbackTimer.Enabled = true;
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
			obstacleTimerFired = false;

		}

	}



}
