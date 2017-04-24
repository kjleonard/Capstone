using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.VR;

public class LoadSceneOnScript : MonoBehaviour {

    public void LoadByIndex(int sceneIndex)
    {
        // Enables VR if we are in simulation (Scene 1), disables when at start/end screens (Scene 0 or Scene 2)
        if (sceneIndex == 1)
        {
            Cursor.visible = false;
            VRSettings.enabled = true;
        }
        SceneManager.LoadScene(sceneIndex);
    }

    //void OnEnable()
    //{
    //    //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
    //    SceneManager.sceneLoaded += OnLevelFinishedLoading;
    //}

    //void OnDisable()
    //{
    //    //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
    //    SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    //}

    //void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    //{
    //    if (scene.buildIndex == 1)
    //    {
    //        Dropdowns.getValue(GameObject.Find("selSpeed").GetComponent<Dropdown>());
    //    }
    //}

    public void EndSimulation()
    {
        Application.Quit();
    }
}
