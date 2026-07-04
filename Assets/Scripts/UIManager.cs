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

    [SerializeField] private int currentDay;
    [SerializeField] private int currentBread;
    [SerializeField] private int currentStone;

    private int dayCount;
    private int breadCount;
    private int stoneCount;

    private int timeCount;
    private int delayTime;

    private bool isHome;

    public void NextDay()
    {
        if (isHome && timeCount > 0)
        {
            // something cool will happen in the future
        }
        else if (isHome && timeCount <= 0)
        {
            StartCoroutine(PopupMenuAnimation());
        }
    }

    private IEnumerator PopupMenuAnimation()
    {
        statisticPopupMenu.SetActive(true);

        breadCountText.text = $"Bread: {breadCount}";

        yield return new WaitForSeconds(delayTime);

        stoneCountText.text = $"Stone: {stoneCount}";

        yield return new WaitForSeconds(delayTime);

        dayCountText.text = $"Day: {dayCount}";

        yield return new WaitForSeconds(delayTime);

        nextDayButton.SetActive(true);
    }

    public void HideMenu()
    {
        statisticPopupMenu.SetActive(false);
        nextDayButton.SetActive(false);
    }
}
