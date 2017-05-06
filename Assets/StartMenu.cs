using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class StartMenu : MonoBehaviour {

    /** Initializes PlayerPrefs to default values upon Start Menu load, as they will only
     * be written if a user changes the dropdown menu value otherwise; also disables VR and enables the cursor. */
    // Use this for initialization
    void Start () {
        // Set initial values since Player Prefs are only written to if the dropdown value is changed

        Cursor.visible = true;
        VRSettings.enabled = false;

        PlayerPrefs.SetFloat("selSpeed", (float) 0.9);
        PlayerPrefs.SetInt("selDuration", 75000);
        PlayerPrefs.SetInt("selObstacleType", 0);
        PlayerPrefs.SetInt("selObstacleFrequency", 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
