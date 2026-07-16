using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("UI References")]
    public TextMeshProUGUI dayCountText;
    [SerializeField] float disappearTimeDayText;
    public GameObject pauseScreen;
    public GameObject summaryPrefab;
    public TextMeshProUGUI breadCountText;
    [SerializeField] Slider healthbar;
    PlayerController playerController;
    public Animator animator;

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

        playerController = FindFirstObjectByType<PlayerController>();
        healthbar.maxValue = playerController.maxHealth;
        animator = dayCountText.GetComponent<Animator>();
    }

    private void Update()
    {
        healthbar.maxValue = playerController.maxHealth;
        healthbar.value = playerController.currentHealth;

        if (playerController == null)
        {
            playerController = FindFirstObjectByType<PlayerController>();
        }
    }

    #region UI
    public void DayTextUI(string day)
    {
        if (dayCountText != null)
            dayCountText.text = day;
    }

    public void BreadUI(string bread)
    {
        if (breadCountText != null)
            breadCountText.text = bread;
    }

    #endregion
    #region Menu
    public void PauseMenu(bool value)
    {
        if (pauseScreen != null)
            pauseScreen.SetActive(value);

        AudioManager.instance.PlaySFX(AudioManager.instance.buttonClip);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
        AudioManager.instance.PlaySFX(AudioManager.instance.buttonClip);
        CameraController cameraController = Camera.main.GetComponent<CameraController>();
        StartCoroutine(cameraController.Transition(0));
    }
    #endregion
    #region GameObjects
    public void SummeryObject()
    {
        if (summaryPrefab != null)
            Instantiate(summaryPrefab);
    }

    #endregion
}