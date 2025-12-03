using UnityEngine;

public class SceneManagerEditor : MonoBehaviour
{
    private void Awake()
    {
        ApiClient.SetDomain("http://localhost:8001");
    }
}
