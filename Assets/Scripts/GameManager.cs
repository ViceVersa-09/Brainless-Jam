using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static class Day
    {
        public static int CurrentDay { get { return instance.currentDay; } set { instance.currentDay = value; } }
        public static int CurrentTick { get { return instance.currentTick; } set { instance.currentTick = value; } }
        public static float TimeUntilNight { get { return (instance.ticksPerDay - instance.currentTick) * instance.timePerTick - (instance.timeSinceDayStarted % instance.timePerTick); } }
        public static bool IsDay { get { return instance.isDay; } set { instance.isDay = value; } }
    }

    [Header("Values")]
    [SerializeField] public int startBread;
    [SerializeField] public int littleGuys;
    [SerializeField] GameObject littleGuyPrefab;


    [Header("Day")]
    [SerializeField] float timePerTick = 10;
    [SerializeField] public int ticksPerDay = 24;

    [HideInInspector] public int wood;
    [HideInInspector] public int stone;
    [HideInInspector] public int bread;
    [HideInInspector] public int leftoverBread;
    int currentDay;
    public int missionMaxAdder;
    [HideInInspector] public int littleGuysSpawnAmount;
    [HideInInspector] public int currentTick = 0;
    [HideInInspector] public float timeSinceDayStarted;
    [HideInInspector] public bool isDay = false;

    InputAction pauseMenu;
    ResourceManager resourceManager;

    private void Awake()
    {
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

        littleGuysSpawnAmount = littleGuys;

        if (UIManager.instance != null)
        {
            UIManager.instance.DayTextUI($"Day: {currentDay}");
            UIManager.instance.BreadUI("Time until night: " + Day.TimeUntilNight);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        resourceManager = FindFirstObjectByType<ResourceManager>();

        if (currentDay < 1)
        {
            bread = startBread;
        }

        if (UIManager.instance != null)
        {
            UIManager.instance.DayTextUI($"Day: {currentDay}");
            UIManager.instance.BreadUI("Time until night: " + Day.TimeUntilNight);
        }

        bread += leftoverBread;
        leftoverBread = 0;
    }

    private void Update()
    {
        Menu();
        BreadUI();

        if (resourceManager == null)
        {
            resourceManager = FindFirstObjectByType<ResourceManager>();
        }
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
            bool isOpen = UIManager.instance.pauseScreen.activeInHierarchy;
            UIManager.instance.PauseMenu(!isOpen);

            UIManager.instance.pauseScreen.SetActive(!isOpen);

            Time.timeScale = !isOpen ? 0 : 1;
        }
    }
    #endregion
    #region DayNightCyckle
    void BreadUI()
    {
        if (isDay)
        {
            timeSinceDayStarted += Time.deltaTime;
        }

        if (UIManager.instance != null)
        {
            UIManager.instance.DayTextUI($"Day: {currentDay}");
            UIManager.instance.BreadUI("Time until night: " + FloatToIntRoundedUp(Day.TimeUntilNight));
        }
    }

    IEnumerator DayTimer()
    {
        currentTick = 0;
        timeSinceDayStarted = 0;
        for (int i = 0; i < ticksPerDay; i = currentTick)
        {
            yield return new WaitForSeconds(timePerTick);
            currentTick++;
        }
        isDay = false;
        timeSinceDayStarted = 0;
        EndGame();
    }

    public void StartGame()
    {
        isDay = true;
        resourceManager.CountBread();
        StartCoroutine(DayTimer());

        LittleGuy[] everyLittleGuy = FindObjectsByType<LittleGuy>(FindObjectsSortMode.None);

        foreach (var littleGuy in everyLittleGuy)
        {
            if (littleGuy.currentState == LittleGuy.State.FarmingHome)
            {
                littleGuy.tag = "Untagged";
            }
        }
    }

    public void SpawnLittleGuys()
    {
        for (int i = 0; i < littleGuysSpawnAmount; i++)
        {
            Instantiate(littleGuyPrefab, new Vector2(0, 5), Quaternion.identity);
        }
    }

    public void EndGame()
    {
        StopAllCoroutines();
        currentDay++;
        missionMaxAdder++;
        UIManager.instance.SummeryObject();
    }

    public int FloatToIntRoundedUp(float input)
    {
        if (Mathf.RoundToInt(input) - input == 0)
        {
            return (int)input;
        }
        return (int)input + 1;
    }
    #endregion
}