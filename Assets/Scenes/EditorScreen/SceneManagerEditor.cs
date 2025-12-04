using UnityEngine;

public class SceneManagerEditor : MonoBehaviour
{
    private void Awake()
    {
        ApiClient.SetDomain($"https://{SettingConnection._baseDomain}");
    }
}
