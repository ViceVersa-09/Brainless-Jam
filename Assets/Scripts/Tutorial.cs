using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class Tutorial : MonoBehaviour
{
    [Header("General")]
    [SerializeField] GameObject parentObjectUI;
    [SerializeField] TextMeshProUGUI tutorialText;
    [SerializeField] float walkDistanceMarginal;
    [SerializeField] TextMeshProUGUI tipText;

    [Header("One-Time use")]   
    [SerializeField] Vector3 target1;
    [SerializeField] float playerMoveSpeed;
    [SerializeField] GameObject wolfPrefab;
    [SerializeField] Vector2 wolfOffset;

    [Header("Texts")]
    [SerializeField] string[] tipTexts;
    [SerializeField] string[] texts1;
    [SerializeField] string[] texts2;
    [SerializeField] string[] texts3;
    [SerializeField] string[] texts4;
    [SerializeField] string[] texts5;
    [SerializeField] string[] texts6;
    [SerializeField] string[] texts7;

    [HideInInspector] public bool cutscene;
    [HideInInspector] public bool canOpenGates;

    PlayerController playerController;
    InputAction textAction;
    Gates gates;
    GameObject wolf;

    private void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        gates = FindFirstObjectByType<Gates>();
        textAction = InputSystem.actions.FindAction("Text");

        canOpenGates = false;
        StartCoroutine(FirstCutscene());
    }

    private void Update()
    {
        tipText.gameObject.SetActive(!cutscene);

        if (playerController == null)
        {
            playerController = FindFirstObjectByType<PlayerController>();
        }
    }

    void UpdateText(string text)
    {
        parentObjectUI.SetActive(true);
        tutorialText.text = text;
    }

    IEnumerator FirstCutscene()
    {
        yield return new WaitUntil(() => playerController != null);

        cutscene = true;

        while (Vector2.Distance(target1, playerController.transform.position) > walkDistanceMarginal)
        {
            Debug.Log("Moving");
            playerController.MovePlayer((Vector2)target1 - (Vector2)playerController.transform.position, playerMoveSpeed);
            yield return new WaitForEndOfFrame();
        }
        
        foreach (var text in texts1)
        {
            Debug.Log(text);
            UpdateText(text);
            yield return new WaitUntil(() => !textAction.triggered);
            yield return new WaitUntil(() => textAction.triggered);
            AudioManager.instance.PlaySFX(AudioManager.instance.buttonClip);
        }
        
        parentObjectUI.SetActive(false);
        cutscene = false;
        tipText.text = tipTexts[0];
        StartCoroutine(SecondCutscene());
    }

    IEnumerator SecondCutscene()
    {
        LittleGuy[] littleGuys = FindObjectsByType<LittleGuy>(FindObjectsSortMode.None);
        int guysRecruited = 0;

        while (guysRecruited < 5)
        {
            guysRecruited = 0;
            foreach (var littleGuy in littleGuys)
            {
                if (littleGuy.currentState == LittleGuy.State.FollowingPlayer)
                {
                    guysRecruited++;
                }
            }
            yield return new WaitForEndOfFrame();
        }

        cutscene = true;

        foreach (var text in texts2)
        {
            Debug.Log(text);
            UpdateText(text);
            yield return new WaitUntil(() => !textAction.triggered);
            yield return new WaitUntil(() => textAction.triggered);
            AudioManager.instance.PlaySFX(AudioManager.instance.buttonClip);
        }

        parentObjectUI.SetActive(false);
        cutscene = false;
        tipText.text = tipTexts[1];
        StartCoroutine(ThirdCutscene());
    }

    IEnumerator ThirdCutscene()
    {
        LittleGuy[] littleGuys = FindObjectsByType<LittleGuy>(FindObjectsSortMode.None);
        int guysRecruited = 0;

        while (guysRecruited != 3)
        {
            guysRecruited = 0;
            foreach (var littleGuy in littleGuys)
            {
                if (littleGuy.currentState == LittleGuy.State.FollowingPlayer)
                {
                    guysRecruited++;
                }
            }
            yield return new WaitForEndOfFrame();
        }

        cutscene = true;

        foreach (var text in texts3)
        {
            Debug.Log(text);
            UpdateText(text);
            yield return new WaitUntil(() => !textAction.triggered);
            yield return new WaitUntil(() => textAction.triggered);
            AudioManager.instance.PlaySFX(AudioManager.instance.buttonClip);
        }

        parentObjectUI.SetActive(false);
        cutscene = false;
        canOpenGates = true;
        tipText.text = tipTexts[2];
        StartCoroutine(FourthCutscene());
    }

    IEnumerator FourthCutscene()
    {
        yield return new WaitUntil(() => !gates.gateCollider.enabled);

        cutscene = true;

        foreach (var text in texts4)
        {
            Debug.Log(text);
            UpdateText(text);
            yield return new WaitUntil(() => !textAction.triggered);
            yield return new WaitUntil(() => textAction.triggered);
            AudioManager.instance.PlaySFX(AudioManager.instance.buttonClip);
        }

        parentObjectUI.SetActive(false);
        cutscene = false;
        tipText.text = tipTexts[3];
        StartCoroutine(FifthCutscene());
    }

    IEnumerator FifthCutscene()
    {
        ResourceManager resourceManager = FindFirstObjectByType<ResourceManager>();
        Mission mission = FindFirstObjectByType<Mission>();

        yield return new WaitUntil(() => resourceManager.currentWood >= mission.woodMission && resourceManager.currentStone >= mission.stoneMission);

        cutscene = true;

        foreach (var text in texts5)
        {
            Debug.Log(text);
            UpdateText(text);
            yield return new WaitUntil(() => !textAction.triggered);
            yield return new WaitUntil(() => textAction.triggered);
            AudioManager.instance.PlaySFX(AudioManager.instance.buttonClip);
        }

        parentObjectUI.SetActive(false);
        cutscene = false;
        tipText.text = tipTexts[4];
        StartCoroutine(SixthCutscene());
    }

    IEnumerator SixthCutscene()
    {
        yield return new WaitUntil(() => gates.canInteract);

        cutscene = true;

        foreach (var text in texts6)
        {
            Debug.Log(text);
            UpdateText(text);
            yield return new WaitUntil(() => !textAction.triggered);
            yield return new WaitUntil(() => textAction.triggered);
            AudioManager.instance.PlaySFX(AudioManager.instance.buttonClip);
        }

        wolf = Instantiate(wolfPrefab, (Vector2)playerController.transform.position + wolfOffset, Quaternion.identity);

        parentObjectUI.SetActive(false);
        cutscene = false;
        tipText.text = tipTexts[5];
        StartCoroutine(SeventhCutscene());
    }

    IEnumerator SeventhCutscene()
    {
        yield return new WaitUntil(() => wolf == null);

        cutscene = true;

        foreach (var text in texts7)
        {
            Debug.Log(text);
            UpdateText(text);
            yield return new WaitUntil(() => !textAction.triggered);
            yield return new WaitUntil(() => textAction.triggered);
            AudioManager.instance.PlaySFX(AudioManager.instance.buttonClip);
        }

        parentObjectUI.SetActive(false);
        cutscene = false;
        tipText.text = tipTexts[6];

        ResourceManager resourceManager = FindFirstObjectByType<ResourceManager>();
        resourceManager.leftoverBread = 0;
        GameManager.instance.EndGame();
        resourceManager.currentBread = GameManager.instance.startBread;
    }
}
