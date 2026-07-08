using System;
using System.Collections;
using TMPro;
using UnityEngine;
using static GameManager;

public class Summary : MonoBehaviour
{
    [Header("Moving")]
    [SerializeField] RectTransform[] rects;
    [SerializeField] Vector2 startHeight;
    [SerializeField] float moveSpeed;
    [SerializeField] float slowDownTime;
    [SerializeField] Vector2 awayPosition; 

    [Header("Bread")]
    [SerializeField] TextMeshProUGUI breadText;
    [SerializeField] TextMeshProUGUI leftoverText;

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
        gameManager = instance;
        Array.Resize(ref ogPositions, rects.Length);

        StartMovement();
        Bread();
        Wood();
        Stone();
        resourceManager.EndOfDayMaterials();
    }

    void StartMovement()
    {
        for (int i = 0; i < rects.Length; i++)
        {
            ogPositions[i] = rects[i].anchoredPosition;
            rects[i].anchoredPosition = startHeight;

            StartCoroutine(RectMovement(rects[i], ogPositions[i]));
        }
    }

    IEnumerator RectMovement(RectTransform rect, Vector2 target)
    {
        for (int i = 0; i < 1; i++)
        {
            rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition,
                Vector2.Lerp(rect.anchoredPosition, target, slowDownTime * Time.deltaTime), moveSpeed);

            if (rect.anchoredPosition != target)
            {
                i--;
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public void MoveAway()
    {
        StopAllCoroutines();
        foreach (var rect in rects)
        { 
            StartCoroutine(RectMovement(rect, new Vector2(awayPosition.x, rect.anchoredPosition.y)));
        }
    }

    void Bread()
    {
        breadText.text = resourceManager.bread + " Bread Baked";
        leftoverText.text = "+" + resourceManager.leftoverBread + " leftover";
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
