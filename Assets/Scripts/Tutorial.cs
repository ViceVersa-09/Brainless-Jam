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

    [Header("One-Time use")]   
    [SerializeField] Vector3 target1;
    [SerializeField] float playerMoveSpeed;

    [Header("Texts")]
    [SerializeField] string[] texts1;
    [SerializeField] string[] texts2;
    [SerializeField] string[] texts3;

    [HideInInspector] public bool cutscene;
    bool buttonPressed;
    [HideInInspector] public bool canOpenGates;

    PlayerController playerController;
    InputAction textAction;
    Gates gates;

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
        CheckForAction();
    }

    void UpdateText(string text)
    {
        parentObjectUI.SetActive(true);
        tutorialText.text = text;
    }

    void CheckForAction()
    {
        if (textAction.triggered)
        {
            buttonPressed = true;
        }
    }

    IEnumerator FirstCutscene()
    {
        cutscene = true;
        while (Vector2.Distance(target1, playerController.transform.position) > walkDistanceMarginal)
        {
            playerController.MovePlayer(target1 - playerController.transform.position, playerMoveSpeed);
            yield return new WaitForEndOfFrame();
        }
        
        foreach (var text in texts1)
        {
            Debug.Log(text);
            UpdateText(text);
            yield return new WaitUntil(() => buttonPressed);
            buttonPressed = false;
        }
        
        parentObjectUI.SetActive(false);
        cutscene = false;
        StartCoroutine(SecondCutscene());
    }

    IEnumerator SecondCutscene()
    {
        LittleGuy[] littleGuys = FindObjectsByType<LittleGuy>(FindObjectsSortMode.None);
        int guysRecruited = 0;

        while (guysRecruited < 3)
        {
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
            yield return new WaitUntil(() => buttonPressed);
            buttonPressed = false;
        }

        parentObjectUI.SetActive(false);
        cutscene = false;
        canOpenGates = true;
        StartCoroutine(ThirdCutscene());
    }

    IEnumerator ThirdCutscene()
    {
        yield return new WaitUntil(() => !gates.gateCollider.enabled);

        cutscene = true;

        foreach (var text in texts3)
        {
            Debug.Log(text);
            UpdateText(text);
            yield return new WaitUntil(() => buttonPressed);
            buttonPressed = false;
        }

        parentObjectUI.SetActive(false);
        cutscene = false;
        //StartCoroutine(FourthCutscene());
    }
}
