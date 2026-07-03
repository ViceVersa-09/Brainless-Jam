using UnityEngine;

public class Summary : MonoBehaviour
{
    ResourceManager resourceManager;

    private void Start()
    {
        resourceManager = FindFirstObjectByType<ResourceManager>();
    }
}
