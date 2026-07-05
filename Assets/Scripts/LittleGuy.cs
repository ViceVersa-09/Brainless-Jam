using System.Linq;
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

    [HideInInspector] public Vector3 target;

    [HideInInspector] public State currentState;
    PlayerController playerController;
    Rigidbody2D rb;
    CircleCollider2D col;
    LittleGuyManager littleGuyManager;

    private void Awake()
    {
        currentState = State.FarmingHome;
        playerController = FindFirstObjectByType<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        littleGuyManager = FindFirstObjectByType<LittleGuyManager>();
    }

    private void Start()
    {
        littleGuyManager.UpdateLittleGuysTarget();
        target = LittleGuyManager.LittleGuysTarget[GetIndex()];
    }

    private void Update()
    {
        Behaviour();
    }

    void Behaviour()
    {
        if (currentState == State.FollowingPlayer)
        {
            col.radius = 1;

            if (Vector2.Distance(transform.position, target) > distance)
            {
                rb.linearVelocity = moveSpeed * (target - transform.position);
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    int GetIndex()
    {
        for (int i = 0; i < LittleGuyManager.LittleGuysTarget.Length; i++)
        {
            foreach (LittleGuy li in FindObjectsByType<LittleGuy>(FindObjectsSortMode.None))
            {
                if (li.target == LittleGuyManager.LittleGuysTarget[i])
                {
                    goto OuterLoop;
                }
            }
            return i;
        OuterLoop: continue;
        }
        return 0;
    }
}
