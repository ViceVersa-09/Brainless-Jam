using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject nextDayButton;
    [SerializeField] private GameObject statisticPopupMenu;
    [SerializeField] private TextMeshProUGUI dayCountText;
    [SerializeField] private TextMeshProUGUI breadCountText;
    [SerializeField] private TextMeshProUGUI stoneCountText;
    [SerializeField] private TextMeshProUGUI woodCountText;

    private int timeCount;
    private int delayTime;

    private bool isHome;

    GameManager gameManager;
    ResourceManager resourceManager;

    private void Start()
    {
        GameManager.instance = gameManager;
        resourceManager = FindFirstObjectByType<ResourceManager>();
    }
    #region Statistics Menu
    public void NextDay()
    {
        // if the player is at home but the day is not yet over
        if (isHome && timeCount > 0)
        {
            // give the player the option to end the day or wait
        }
        //if the player is home and the day is over
        else if (isHome && timeCount <= 0)
        {
            StartCoroutine(PopupMenuAnimation());
        }
    }

    private IEnumerator PopupMenuAnimation()
    {
        statisticPopupMenu.SetActive(true);

        breadCountText.text = $"Bread: {gameManager.bread}";
        yield return new WaitForSeconds(delayTime);

        stoneCountText.text = $"Stone: {resourceManager.stone}";
        yield return new WaitForSeconds(delayTime);

        stoneCountText.text = $"Wood: {resourceManager.wood}";
        yield return new WaitForSeconds(delayTime);

        dayCountText.text = $"Day: {gameManager.day}";
        yield return new WaitForSeconds(delayTime);

        nextDayButton.SetActive(true);
    }

    public void HideMenu()
    {
        statisticPopupMenu.SetActive(false);
        nextDayButton.SetActive(false);
    }
    #endregion

}