using TMPro;
using UnityEngine;

public class Summary : MonoBehaviour
{
    [Header("Bread")]
    [SerializeField] TextMeshProUGUI breadText;
    [SerializeField] TextMeshProUGUI timeText;

    [Header("Wood")]
    [SerializeField] TextMeshProUGUI missionWoodText;
    [SerializeField] TextMeshProUGUI woodText;
    [SerializeField] TextMeshProUGUI woodGainedText;

    [Header("Wood")]
    [SerializeField] TextMeshProUGUI missionStoneText;
    [SerializeField] TextMeshProUGUI stoneText;
    [SerializeField] TextMeshProUGUI stoneGainedText;

    ResourceManager resourceManager;
    GameManager gameManager;

    private void Start()
    {
        resourceManager = FindFirstObjectByType<ResourceManager>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void Bread()
    {
        breadText.text = gameManager.bread + " Bread Baked";
        timeText.text = "+" + (gameManager.bread * 15) + " seconds";
    }

    void Wood()
    {

    }
}
