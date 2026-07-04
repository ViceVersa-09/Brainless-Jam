using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public class Day
    {
        public static int CurrentDay { get { return instance.day; } set { instance.day = value; } }
        public static int CurrentTick { get { return instance.currentTick; } set { instance.currentTick = value; } }
        public static float TimeUntilNight { get { return (instance.ticksPerDay - instance.currentTick) * instance.timePerTick - (instance.timeSinceDayStarted % instance.timePerTick); } }
    }

    [Header("Values")]
    [SerializeField] int startBread;


    [Header("Day")]
    [SerializeField] float timePerTick = 10;
    [SerializeField] int ticksPerDay = 24;

    [HideInInspector] public int currentBread;

    [HideInInspector] public int day;
    [HideInInspector] public int bread;
    [HideInInspector] public int stone;

    int currentDay;
    int dayTime;
    int currentTick = 0;
    float timeSinceDayStarted;

    InputAction pauseMenu;
    UIManager uIManager;

    private void Awake()
    {
        uIManager = FindFirstObjectByType<UIManager>();

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        pauseMenu = InputSystem.actions.FindAction("Pause");

        bread = startBread;
        currentDay--;
        uIManager.DayTextUI($"Day: {Day.CurrentDay}");
        uIManager.BreadUI("Time until night: " + Day.TimeUntilNight);
    }

    private void Update()
    {
        Menu();
    }
    #region Menu
    private void Menu()
    {
        if (pauseMenu == null)
        {
            Debug.Log("No InputAction Found");
            return;
        }

        if (pauseMenu.WasPressedThisFrame())
        {
            bool isOpen = uIManager.pauseScreen.activeInHierarchy;
            uIManager.PauseMenu(!isOpen);

            uIManager.pauseScreen.SetActive(!isOpen);

            Time.timeScale = !isOpen ? 0 : 1;
        }
    }
    #endregion
    #region Something
    IEnumerator Timer()
    {
        dayTime = bread * 15;

        for (int i = dayTime; i >= 0; i--)
        {
            uIManager.BreadUI("Time until night: " + i);
            yield return new WaitForSeconds(1);

            if (i == 0)
            {
                EndGame();
                break;
            }
        }
    }

    public void StartGame()
    {
        StartCoroutine(Timer());

        LittleGuy[] everyLittleGuy = FindObjectsByType<LittleGuy>(FindObjectsSortMode.None);

        foreach (var littleGuy in everyLittleGuy)
        {
            if (littleGuy.currentState == LittleGuy.State.FarmingHome)
            {
                littleGuy.tag = "Untagged";
            }
        }
    }

    void EndGame()
    {
        ResourceManager resourceManager = FindFirstObjectByType<ResourceManager>();

        resourceManager.CountBread();
        uIManager.SummeryObject();
    }
    #endregion
}