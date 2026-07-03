using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    // OBS! antingen ge menyer och UI egna script eller ha det under gamemanager objektet
    [Header("References")]
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private TextMeshProUGUI dayCountText;
    [SerializeField] private TextMeshProUGUI breadCountText;

    [Header("Values")]
    [SerializeField] int startBread;

    [HideInInspector] public int currentDay;
    [HideInInspector] public int bread;
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
        dayCountText.text = $"Day: {currentDay}";
        breadCountText.text = "Time until night: " + (bread * 15);
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
            bool isOpen = pauseScreen.activeSelf;

            pauseScreen.SetActive(!isOpen);

            Time.timeScale = !isOpen ? 0 : 1;
        }
    }
    #endregion
    #region UI
    void NextDay()
    {
        UIUpdate();
    }

    private void UIUpdate()
    {
        dayCountText.text = $"Day: {currentDay}";
    }
    #endregion

    IEnumerator Timer()
    {
        dayTime = bread * 15;

        for (int i = dayTime; i >= 0; i--)
        {
            breadCountText.text = "Time until night: " + i;
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
    }
}