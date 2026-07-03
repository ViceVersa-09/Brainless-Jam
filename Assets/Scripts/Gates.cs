using UnityEngine;
using UnityEngine.InputSystem;

public class Gates : MonoBehaviour
{
    bool canInteract;

    GameManager gameManager;
    InputAction interactAction;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        interactAction = InputSystem.actions.FindAction("Interact");
    }

    private void Update()
    {
        if (canInteract && interactAction.triggered)
        {
            OpenGates();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int playerLayer = LayerMask.NameToLayer("Player");

        if (other.gameObject.layer == playerLayer)
        {
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        int playerLayer = LayerMask.NameToLayer("Player");

        if (other.gameObject.layer == playerLayer)
        {
            canInteract = false;
        }
    }

    void OpenGates()
    {
        gameManager.StartGame();
        gameObject.SetActive(false);
    }
}
