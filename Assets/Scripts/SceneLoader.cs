using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Put the EXACT scene name here in Inspector (must match Build Settings / file name)
    public string sceneToLoad;

    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
            SceneManager.LoadScene(sceneToLoad);
        else
            Debug.LogError("SceneLoader: sceneToLoad is empty!");
    }
}
