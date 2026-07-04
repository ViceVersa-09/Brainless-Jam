using System.Collections;
using TMPro;
using UnityEngine;

public class Summary : MonoBehaviour
{
    [Header("Moving")]
    [SerializeField] float startHeight;
    [SerializeField] float moveSpeed;
    [SerializeField] float slowDownTime;

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
    RectTransform[] rects;
    Vector2[] ogPositions;

    private void Start()
    {
        resourceManager = FindFirstObjectByType<ResourceManager>();
        gameManager = GameManager.instance;
        rects = GetComponentsInChildren<RectTransform>();
    }

    IEnumerator MovePopup()
    {
        for (int i = 0; i < rects.Length; i++)
        {
            ogPositions[i] = rects[i].position;
            Vector2 positionChanger = rects[i].position;
            positionChanger.y += startHeight;
            rects[i].position = positionChanger;
        }

        for (int i = 0; i < 1; i++)
        {
            foreach (var rect in rects)
            {
                rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition,
                Vector2.Lerp(rect.anchoredPosition, Vector2.zero, slowDownTime * Time.deltaTime), moveSpeed);

                if (rect.anchoredPosition.magnitude != 0)
                {
                    i--;
                    yield return new WaitForEndOfFrame();
                }
            }
        }
    }

    void Bread()
    {
        breadText.text = gameManager.bread + " Bread Baked";
        timeText.text = "+" + (gameManager.bread * 15) + " seconds";
    }

    void Wood()
    {
        woodText.text = resourceManager.wood.ToString();
        woodGainedText.text = "+" + resourceManager.currentWood;
    }

    void Stone()
    {
        woodText.text = resourceManager.stone.ToString();
        woodGainedText.text = "+" + resourceManager.currentStone;
    }
}
