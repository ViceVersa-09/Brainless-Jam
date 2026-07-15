using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    Animator animator;
    SpriteRenderer spriteRenderer;
    InputAction mineAction;
    GameManager gameManager;
    Gates gates;

    // this will tell the little guys at which direction the player is heading at
    public Vector2 LastMoveDirection { get; private set; } = Vector2.down;

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        unRecruitAction = InputSystem.actions.FindAction("UnRecruit");
        rb = GetComponent<Rigidbody2D>();
        tutorial = FindFirstObjectByType<Tutorial>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        mineAction = InputSystem.actions.FindAction("Mine");
        gameManager = FindFirstObjectByType<GameManager>();
        gates = FindFirstObjectByType<Gates>();

        currentHealth = maxHealth;
    }

    private void Update()
    {
        // I'm doing it this way so that it'll be easier to make things like cutscenes
        moveVector = moveAction.ReadValue<Vector2>();

        if (rb.linearVelocityX != 0)
        {
            animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        }
        else
        {
            animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.y));
        }

        if (tutorial == null || tutorial != null && !tutorial.cutscene)
        {
            MovePlayer(moveVector, moveSpeed);
        }
        else if (tutorial != null && tutorial.cutscene)
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (unRecruitAction.triggered && !gameManager.isDay && canControl)
        {
            UnRecruit();
        }

        if (mineAction.triggered && canControl)
        {
            animator.SetTrigger("Punch");
        }

        if (gates.canInteract && tutorial == null)
        {
            currentHealth = maxHealth;
        }

        if (currentHealth <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void MovePlayer(Vector2 moveVector, float moveSpeed)
    {
        if (canControl)
        {
            if (moveVector != Vector2.zero)
            {
                LastMoveDirection = moveVector.normalized;
                StartCoroutine(AudioManager.instance.WalkLoop());
            }

            rb.linearVelocity = moveVector * moveSpeed;

            if (rb.linearVelocityX < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (rb.linearVelocityX > 0)
            {
                spriteRenderer.flipX = false;
            }
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
                littleGuy.GetComponent<Interactable>().EnableRecruiting();
                AudioManager.instance.PlaySFX(AudioManager.instance.unRecruitClip);
                break;
            }
        }
    }
}
