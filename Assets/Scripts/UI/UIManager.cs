using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("UI References")]
    public TextMeshProUGUI dayCountText;

    //[Header("Menu")]
    public GameObject pauseScreen;

    //[Header("GameObjects")]
    public GameObject summaryPrefab;
    public GameObject gates;

    //[Header("Material")]
    public TextMeshProUGUI breadCountText;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
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
    }
    #endregion
    #region GameObjects
    public void SummeryObject()
    {
        if (summaryPrefab != null)
            Instantiate(summaryPrefab);
    }

    public void GateObject(bool value)
    {
        if (gates != null)
            gates.SetActive(value);
    }

    #endregion
}