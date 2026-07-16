using System.Collections;
using System.Reflection;
using TMPro;
using UnityEngine;

public class Mission : MonoBehaviour
{
    [Header("Missions")]
    [SerializeField] Vector2 woodRange;
    [SerializeField] Vector2 stoneRange;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI woodText;
    [SerializeField] TextMeshProUGUI stoneText;
    [SerializeField] GameObject openButton;
    [SerializeField] GameObject closeButton;

    [Header("Moving")]
    [SerializeField] Vector2 openPosition;
    [SerializeField] Vector2 closedPosition;
    [SerializeField] float moveSpeed;
    [SerializeField] float slowDownTime;

    [HideInInspector] public int woodMission;
    [HideInInspector] public int stoneMission;
    bool open;

    ResourceManager resourceManager;
    RectTransform rect;

    private void Start()
    {
        resourceManager = FindFirstObjectByType<ResourceManager>();
        rect = GetComponent<RectTransform>();

        open = closeButton.activeInHierarchy;
        NewMission();
        UpdateMissionText();
    }

    public void NewMission()
    {
        woodMission = Random.Range((int)woodRange.x, (int)woodRange.y + GameManager.instance.missionMaxAdder);
        stoneMission = Random.Range((int)stoneRange.x, (int)stoneRange.y + GameManager.instance.missionMaxAdder);
    }

    public void UpdateMissionText()
    {
        woodText.text = resourceManager.currentWood + "/" + woodMission;
        stoneText.text = resourceManager.currentStone + "/" + stoneMission;
    }

    public void MissionButton()
    {
        StopAllCoroutines();
        StartCoroutine(MissionRoutine());
    }

    IEnumerator MissionRoutine()
    {       
        if (!open)
        {
            open = true;
            openButton.SetActive(false);
            closeButton.SetActive(true);
            for (int i = 0; i < 1; i++)
            {
                rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition,
                    Vector2.Lerp(rect.anchoredPosition, openPosition, slowDownTime * Time.deltaTime), moveSpeed);

                if (rect.anchoredPosition != openPosition)
                {
                    i--;
                    yield return new WaitForEndOfFrame();
                }
            }
        }
        else if (open)
        {
            open = false;
            openButton.SetActive(true);
            closeButton.SetActive(false);
            for (int i = 0; i < 1; i++)
            {
                rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition,
                    Vector2.Lerp(rect.anchoredPosition, closedPosition, slowDownTime * Time.deltaTime), moveSpeed);

                if (rect.anchoredPosition != closedPosition)
                {
                    i--;
                    yield return new WaitForEndOfFrame();
                }
            }
        }
    }
}
