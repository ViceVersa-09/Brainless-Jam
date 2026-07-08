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
    Tutorial tutorial;

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody2D>();
        tutorial = FindFirstObjectByType<Tutorial>();

        currentHealth = maxHealth;
    }

    private void Update()
    {
        // I'm doing it this way so that it'll be easier to make things like cutscenes
        moveVector = moveAction.ReadValue<Vector2>();

        if (tutorial != null && !tutorial.cutscene)
        {
            MovePlayer(moveVector);
        }
        else if (tutorial == null)
        {
            MovePlayer(moveVector);
        }
    }

    public void MovePlayer(Vector2 moveVector)
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
