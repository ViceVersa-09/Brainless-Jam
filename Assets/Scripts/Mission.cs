using TMPro;
using UnityEngine;

public class Mission : MonoBehaviour
{
    [Header("Missions")]
    [SerializeField] Vector2 woodRange;
    [SerializeField] Vector2 stoneRange;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI woodText;
    [SerializeField] TextMeshProUGUI stoneText;

    int woodMission;
    int stoneMission;

    ResourceManager resourceManager;

    private void Start()
    {
        resourceManager = FindFirstObjectByType<ResourceManager>();
    }

    public void NewMission()
    {
        woodMission = Random.Range((int)woodRange.x, (int)woodRange.y);
        stoneMission = Random.Range((int)stoneRange.x, (int)stoneRange.y);
    }

    void MissionText()
    {
        woodText.text = resourceManager.wood + "/" + woodMission;
        stoneText.text = resourceManager.stone + "/" + stoneMission;
    }

    void MissionButton()
    {

    }
}
