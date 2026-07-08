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

    [HideInInspector] public int targetIndex;
    [HideInInspector] public Vector3 target;

    [HideInInspector] public State currentState;
    State state;
    Rigidbody2D rb;
    CircleCollider2D col;
    LittleGuyManager littleGuyManager;

    private void Awake()
    {
        currentState = State.FarmingHome;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        littleGuyManager = FindFirstObjectByType<LittleGuyManager>();
    }

    private void Start()
    {
        currentState = State.FollowingPlayer;
        state = currentState;
    }

    private void Update()
    {
        CheckBehaviorChanged();
        Behaviour();
    }

    void Behaviour()
    {
        if (currentState == State.FollowingPlayer)
        {
            target = littleGuyManager.LittleGuysTarget[targetIndex];
            col.radius = 1;

            transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
        }
    }

    void CheckBehaviorChanged()
    {
        if (state != currentState)
        {
            if (state == State.FollowingPlayer)
            {
                littleGuyManager.GiveLittleGuysIndex();
            }
            state = currentState;
        }
    }
}
