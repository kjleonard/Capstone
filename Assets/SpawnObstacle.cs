using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacle : MonoBehaviour {

	private Transform spawnLocation;
	public GameObject playerPos;
	public GameObject obstacle;
    public UnityEngine.AudioSource chime;
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

    /** @file
     * @brief Initializes variables and starts obstacle drop timer; called when the Tunnel scene is loaded.
     * 
     * This function is called once when the Tunnel scene is loaded. Positional information, obstacle frequency
     * and timing, and feedback and obstacle drop timers are initialized. */
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
        chime.enabled = false;
        chime.playOnAwake = false;

		obstacleFrequency = PlayerPrefs.GetInt("selObstacleFrequency");
		int durationPref = PlayerPrefs.GetInt ("selDuration");
		int obstacleCount = 0;
		int obstaclesInterval = 0;
		// To-Do: Figure out best way to accomodate for # per minute in combination with speed
		if (obstacleFrequency == 1)
		{   // Low - 4 per minute = 1/15s
			obstacleCount = 4;
			obstaclesInterval = 15000;
		}
		else if (obstacleFrequency == 2)
		{   // Medium - 6 per minute = 1/10s
			obstacleCount = 6;
			obstaclesInterval = 10000;
		}
		else if (obstacleFrequency == 3)
		{   // High - 10 per minute = 1/6s
			obstacleCount = 10;
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

    /** @file
     * @brief Activated when the Feedback timer fires; the flag feedbackTimerFired causes the next frame of
     * the program to disable the feedback text. */

    // Display Feedback
    private void OnTimedEvent_Elapsed(System.Object source, System.Timers.ElapsedEventArgs e)
    {
        // Workaround for multi-threading and Unity calls
        feedbackTimerFired = true;

    }

    /** @file
     * @brief Activated when the Obstacle timer fires; the flag obstacleTimerFired causes the next frame of
     * the program to destroy the existing obstacle and spawn a new obstacle. */

    private void OnObstacleEvent_Elapsed(System.Object source, System.Timers.ElapsedEventArgs e)
	{
		obstacleTimerFired = true;
	}

    /** @file
     * @brief Determines whether the player has passed the existing obstacle, generates feedback dependent upon
     * whether the player hit the obstacle or not, destroys the existing obstacle, and spawns a new obstacle.
     * 
     * This function is called once per frame and performs a number of functions. If feedback is present
     * and the feedback timer has expired, the feedback is hidden. If it is time to spawn a new obstacle,
     * a check is made to determine whether the existing obstacle has been hit or not. If the obstacle has
     * not been hit, visual and audio feedback is displayed to the user. If the obstacle has been hit,
     * the obstacles hit counter is incremented. In either case, the existing obstacle is destroyed, 
     * a new obstacle is instantiated, and the obstacle timer resets. */

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
                chime.Play();
            }
            else // We ignore the first obstacle as it is generated behind the player
            {
                firstRun = false;
                chime.enabled = true;
            }

            // Destroy existing obstacle
            Destroy(generatedCube);

            // Generate a new obstacle
			generatedCube = Instantiate(obstacle, new Vector3(x,y,z), Quaternion.Euler(0, 0, 0)) as GameObject;
			Rigidbody r = generatedCube.GetComponent<Rigidbody>();
			r.angularDrag = 0;
			r.isKinematic = false;
			r.useGravity = true;

            // Reset obstacle timer
			obstacleTimerFired = false;

		}

	}



}
