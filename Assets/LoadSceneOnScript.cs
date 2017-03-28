using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.VR;
using System.Collections.Generic;

public class LoadSceneOnScript : MonoBehaviour {

    public void LoadByIndex(int sceneIndex)
    {
        // Enables VR if we are in simulation (Scene 1), disables when at start/end screens (Scene 0 or Scene 2)
        if (sceneIndex == 0 || sceneIndex == 2)
        {
            VRSettings.enabled = false;
        }
        else
        {
            VRSettings.enabled = true;
        }
        SceneManager.LoadScene(sceneIndex);
    }

    /// <summary>
    /// Saves preferences when exiting the start screen so that they can be utilized in the simulation
    /// </summary>
    public void SavePlayerPrefs()
    {
        PlayerPrefs.SetInt("Speed", selSpeed.GetComponent<Dropdown>.value);
        int durationIndex = selDuration.GetComponent<Dropdown>.value;
        List<Dropdown.OptionData> durationOptions = selDuration.GetComponent<Dropdown>().options;
        PlayerPrefs.SetString("Duration", durationOptions[durationIndex].text);
        PlayerPrefs.SetInt("ObstacleType", selObstacleType.GetComponent<Dropdown>.value);
        PlayerPrefs.SetInt("ObstacleFrequency", selObstacleFrequency.GetComponent<Dropdown>.value);
    }

    public void EndSimulation()
    {
        Application.Quit();
    }
}
