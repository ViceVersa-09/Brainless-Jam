using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float moveSpeed;

    // Keep other as the lowest header
    [Header("Other")]

    bool canInteract;

    InputAction moveAction;
    Vector2 moveVector;
    Rigidbody2D rb;
    InputAction interactAction;
    LittleGuy littleGuy;

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        interactAction = InputSystem.actions.FindAction("Interact");
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // I'm doing it this way so that it'll be easier to make things like cutscenes
        moveVector = moveAction.ReadValue<Vector2>();
        MovePlayer(moveVector);

        Interact();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            canInteract = true;

            littleGuy = other.GetComponent<LittleGuy>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            canInteract = false;

            littleGuy = null;
        }
    }

    void Interact()
    {
        if (canInteract && interactAction.triggered)
        {
            if (littleGuy != null)
            {
                littleGuy.currentState = LittleGuy.State.FollowingPlayer;
            }
        }
    }

    void MovePlayer(Vector2 moveVector)
    {
        if (moveAction.IsPressed())
        {
            rb.linearVelocity = moveVector * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
