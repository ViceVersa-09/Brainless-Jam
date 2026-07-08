using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float moveSpeed;
    [SerializeField] public float maxHealth;

    [Header("Not Serialized")]
    public float currentHealth;
    public bool canControl = true;

    [HideInInspector] public Interactable interactingWith;
    InputAction moveAction;
    Vector2 moveVector;
    Rigidbody2D rb;

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
    }

    private void Update()
    {
        // I'm doing it this way so that it'll be easier to make things like cutscenes
        moveVector = moveAction.ReadValue<Vector2>();
        MovePlayer(moveVector);
    }

    void MovePlayer(Vector2 moveVector)
    {
        if (moveAction.IsPressed() && canControl)
        {
            rb.linearVelocity = moveVector * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
