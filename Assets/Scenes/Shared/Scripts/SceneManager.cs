using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public void OpenSceneByName(string sceneName)
    {
        UnityEngine.SceneManagement
            .SceneManager
            .LoadScene(sceneName);
    }
}
