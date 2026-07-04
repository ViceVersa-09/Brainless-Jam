using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour
{
    #region Audio
    [Header("Other")]
    [SerializeField] private int startScene = 0;

    [Header("Sliders")]
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sFXSlider;

    private void Start()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex == 44)
        {
            masterSlider.value = PlayerPrefs.GetFloat("Master");
            musicSlider.value = PlayerPrefs.GetFloat("Music");
            sFXSlider.value = PlayerPrefs.GetFloat("SFX");
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
        SceneManager.LoadSceneAsync(1);
    }
    public void Options(bool value)
    {
        optionsMenu.SetActive(value);
        startMenu.SetActive(!value);
    }
    #endregion
}