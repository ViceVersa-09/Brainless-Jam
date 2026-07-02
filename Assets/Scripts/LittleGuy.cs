using UnityEngine;

public class LittleGuy : MonoBehaviour
{
    [Header("General")]
    [SerializeField] float moveSpeed;

    [Header("Following Player")]
    [SerializeField] float distance;

    [HideInInspector] public enum State
    {
        FollowingPlayer,
        ReturningHome,
        FarmingHome,
        FarmingWildAlone,

    }

    [HideInInspector] public State currentState;
    PlayerController playerController;
    Rigidbody2D rb;
    CircleCollider2D col;

    private void Start()
    {
        currentState = State.FarmingHome;
        playerController = FindFirstObjectByType<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        Behaviour();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        
    }

    void Behaviour()
    {
        if (currentState == State.FollowingPlayer)
        {
            col.radius = 1;

            if (Vector2.Distance(transform.position, playerController.transform.position) > distance)
            {
                rb.linearVelocity = moveSpeed * (playerController.transform.position - transform.position);
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
}
