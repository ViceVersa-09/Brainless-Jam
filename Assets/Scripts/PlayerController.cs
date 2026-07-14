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
    InputAction unRecruitAction;

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        unRecruitAction = InputSystem.actions.FindAction("UnRecruit");
        rb = GetComponent<Rigidbody2D>();
        tutorial = FindFirstObjectByType<Tutorial>();

        currentHealth = maxHealth;
    }

    private void Update()
    {
        // I'm doing it this way so that it'll be easier to make things like cutscenes
        moveVector = moveAction.ReadValue<Vector2>();

        if (tutorial == null || tutorial != null && !tutorial.cutscene)
        {
            MovePlayer(moveVector, moveSpeed);
        }      

        if (unRecruitAction.triggered)
        {
            UnRecruit();
        }
    }

    public void MovePlayer(Vector2 moveVector, float moveSpeed)
    {
        if (canControl)
        {
            rb.linearVelocity = moveVector * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void UnRecruit()
    {
        LittleGuy[] littleGuys = FindObjectsByType<LittleGuy>(FindObjectsSortMode.None);

        foreach (var littleGuy in littleGuys)
        {
            if (littleGuy.currentState == LittleGuy.State.FollowingPlayer)
            {
                littleGuy.currentState = LittleGuy.State.FarmingHome;
                break;
            }
        }
    }
}
