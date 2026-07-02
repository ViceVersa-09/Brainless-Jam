using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private TextMeshProUGUI dayCountText;
    [SerializeField] private TextMeshProUGUI breadCountText;

    [SerializeField] private int dayCount;
    [SerializeField] private int breadCount;

    private int currentDay;
    private int currentBread;

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

        dayCountText.text = $"Day: {currentDay}";
        breadCountText.text = $"Bread: {currentBread}";

        currentBread = breadCount;
        currentDay = dayCount;
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
        breadCountText.text = $"Bread: {currentBread}";
    }
    #endregion
}