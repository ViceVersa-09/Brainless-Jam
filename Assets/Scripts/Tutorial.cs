using UnityEngine;
using System.Collections;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [Header("General")]
    [SerializeField] GameObject parentObjectUI;
    [SerializeField] TextMeshProUGUI tutorialText;

    [Header("First")]
    [SerializeField] Vector3 target1;
    [SerializeField] string[] texts1;

    public bool cutscene;

    PlayerController playerController;

    private void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
    }

    IEnumerator FirstCutscene()
    {
        while (Vector2.Distance(transform.position, playerController.transform.position) > 0)
        {
            playerController.MovePlayer(target1 - playerController.transform.position);
            yield return new WaitForEndOfFrame();
        }

        UpdateText(texts1[0]);
    }

    void UpdateText(string text)
    {
        parentObjectUI.SetActive(true);
        tutorialText.text = text;
    }
}
