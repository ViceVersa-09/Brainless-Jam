using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Summary : MonoBehaviour
{
    [Header("Moving")]
    [SerializeField] RectTransform[] rects;
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
    Vector2[] ogPositions;

    private void Start()
    {
        resourceManager = FindFirstObjectByType<ResourceManager>();
        gameManager = GameManager.instance;
        Array.Resize(ref ogPositions, rects.Length);

        MovePopup();
    }

    void MovePopup()
    {
        for (int i = 0; i < rects.Length; i++)
        {
            ogPositions[i] = rects[i].anchoredPosition;
            Vector2 positionChanger = rects[i].anchoredPosition;
            positionChanger.y += startHeight;
            rects[i].anchoredPosition = positionChanger;

            StartCoroutine(RectMovement(rects[i], ogPositions[i]));
        }
    }

    IEnumerator RectMovement(RectTransform rect, Vector2 ogPosition)
    {
        for (int i = 0; i < 1; i++)
        {
            rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition,
                Vector2.Lerp(rect.anchoredPosition, ogPosition, slowDownTime * Time.deltaTime), moveSpeed);

            if (rect.anchoredPosition != ogPosition)
            {
                i--;
                yield return new WaitForEndOfFrame();
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
