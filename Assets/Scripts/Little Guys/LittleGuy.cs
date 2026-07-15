using System.Collections;
using UnityEngine;

public class LittleGuy : MonoBehaviour
{
    [Header("General")]
    [SerializeField] float moveSpeed;
    [SerializeField] float bopDelay;
    [SerializeField] float bopMagnitude;

    [Header("Following Player")]
    [SerializeField] float distance;

    [HideInInspector]
    public enum State
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
    FarmingNode[] farmingNodes;
    FarmingNode chosenNode;
    Interactable interactable;

    private void Awake()
    {
        currentState = State.FarmingHome;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        littleGuyManager = FindFirstObjectByType<LittleGuyManager>();
        farmingNodes = FindObjectsByType<FarmingNode>(FindObjectsSortMode.None);
        interactable = GetComponent<Interactable>();
    }

    private void Start()
    {
        state = currentState;

        StartCoroutine(BopAnimation());
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
            if (littleGuyManager.LittleGuysTarget.Length > targetIndex)
            {
                target = littleGuyManager.LittleGuysTarget[targetIndex];
            }

            interactable.enabled = false;
            col.radius = 1;
            transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
        }
        else
        {
            interactable.enabled = true;
        }

        if (currentState == State.FarmingHome)
        {
            col.radius = 2;

            if (chosenNode == null)
            {
                foreach (var node in farmingNodes)
                {
                    if (node.occupant == null)
                    {
                        node.occupant = this;
                        target = node.transform.position;
                        chosenNode = node;
                        break;
                    }
                }
            }

            transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
        }
        else if (chosenNode != null)
        {
            chosenNode.occupant = null;
            chosenNode = null;
        }
    }

    void CheckBehaviorChanged()
    {
        if (state != currentState)
        {
            if (state == State.FollowingPlayer)
            {
                rb.linearVelocity = Vector2.zero;
                littleGuyManager.GiveLittleGuysIndex();
            }
            state = currentState;
        }
    }

    IEnumerator BopAnimation()
    {
        yield return new WaitForSeconds(Random.Range(0, bopDelay));

        while (true)
        {
            transform.localPosition += new Vector3(0, bopMagnitude);
            yield return new WaitForSeconds(bopDelay);
        }
    }
}