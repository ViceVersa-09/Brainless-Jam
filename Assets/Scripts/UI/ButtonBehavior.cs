using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour
{
    #region Audio
    [Header("Sliders")]
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sFXSlider;

    private void Start()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex == 0)
        {
            masterSlider.value = PlayerPrefs.GetFloat("Master", 1);
            musicSlider.value = PlayerPrefs.GetFloat("Music", 1);
            sFXSlider.value = PlayerPrefs.GetFloat("SFX", 1);
        }
    }

    public void VolumeSliders()
    {
        PlayerPrefs.SetFloat("Master", masterSlider.value);
        PlayerPrefs.SetFloat("Music", musicSlider.value);
        PlayerPrefs.SetFloat("SFX", sFXSlider.value);
    }
    #endregion
    #region Button
    [Header("Menu")]
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject startMenu;
    
    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(0);
    }
    public void StartGame()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.buttonClip);
        CameraController cameraController = Camera.main.GetComponent<CameraController>();
        StartCoroutine(cameraController.Transition(1));
    }
    public void Options(bool value)
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.buttonClip);
        optionsMenu.SetActive(value);
        startMenu.SetActive(!value);
    }
    #endregion
}