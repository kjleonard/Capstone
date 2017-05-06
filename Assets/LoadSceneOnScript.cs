using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.VR;

public class LoadSceneOnScript : MonoBehaviour {

    /** Loads the scene with the given index; enables VR and disables the cursor if the scene
     * in question is the Tunnel simulation. */

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

    /** Exits the application. */

    public void EndSimulation()
    {
        Application.Quit();
    }
}
