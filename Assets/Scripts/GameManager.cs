using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Values")]
    [SerializeField] int startBread;

    [HideInInspector] public int currentDay;
    [HideInInspector] public int currentBread;

    [HideInInspector] public int day;
    [HideInInspector] public int bread;
    [HideInInspector] public int stone;
    
    int dayTime;

    InputAction pauseMenu;

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

        bread = startBread;
        currentDay--;
        UIManager.instance.DayTextUI($"Day: {currentDay}");
        UIManager.instance.BreadUI("Time until night: " + (bread * 15));
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
            bool isOpen = UIManager.instance.pauseScreen.activeInHierarchy;
            UIManager.instance.PauseMenu(!isOpen);

            UIManager.instance.pauseScreen.SetActive(!isOpen);

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
            UIManager.instance.BreadUI("Time until night: " + i);
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
        UIManager.instance.SummeryObject();
    }
    #endregion
}