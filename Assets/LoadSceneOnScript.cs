using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadSceneOnScript : MonoBehaviour {

    public void LoadByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void EndSimulation()
    {
        Application.Quit();
    }
}
