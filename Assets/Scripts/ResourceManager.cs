using System.Reflection;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] GameObject gates;
    [SerializeField] TextMeshProUGUI skipText;

    [HideInInspector] public int currentWood;
    [HideInInspector] public int currentStone;
    [HideInInspector] public int currentBread;

    InputAction inputAction;
    PlayerController playerController;
    Tutorial tutorial;
    Mission mission;

    private void Start()
    {
        inputAction = InputSystem.actions.FindAction("Text");
        playerController = FindFirstObjectByType<PlayerController>();
        tutorial = FindFirstObjectByType<Tutorial>();
        mission = FindFirstObjectByType<Mission>();

        UIManager.instance.animator.SetTrigger("Fade");
    }

    private void Update()
    {
        if (inputAction.triggered && skipText.gameObject.activeInHierarchy)
        {
            GameManager.instance.currentTick = GameManager.instance.ticksPerDay - 1;
        }

        if (skipText.gameObject.activeInHierarchy)
        {
            skipText.transform.position = playerController.transform.position + new Vector3(0, 2);
        }
    }

    public void CountBread()
    {
        LittleGuy[] everyLittleGuy = FindObjectsByType<LittleGuy>(FindObjectsSortMode.None);

        currentBread = 0;

        foreach (var littleGuy in everyLittleGuy)
        {
            if (littleGuy.currentState == LittleGuy.State.FarmingHome)
            {
                currentBread++;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int playerLayer = LayerMask.NameToLayer("Player");

        if (other.gameObject.layer == playerLayer && tutorial == null)
        {
            skipText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        skipText.gameObject.SetActive(false);
    }

    public void EndOfDayMaterials()
    {
        if (currentWood >= mission.woodMission && currentStone >= mission.stoneMission && tutorial == null)
        {
            Debug.Log("One more guy");
            GameManager.instance.littleGuysSpawnAmount++;
        }

        GameManager.instance.leftoverBread = GameManager.instance.bread;

        GameManager.instance.wood += currentWood;
        currentWood = 0;

        GameManager.instance.stone += currentStone;
        currentStone = 0;

        GameManager.instance.bread += currentBread;
        currentBread = 0;
    }
}
