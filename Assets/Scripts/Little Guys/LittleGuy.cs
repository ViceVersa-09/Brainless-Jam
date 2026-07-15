using UnityEngine;

public class LittleGuy : MonoBehaviour
{
    [Header("General")]
    [SerializeField] float moveSpeed;

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
    Pickup[] pickups;
    Pickup chosenPickup;
    ResourceManager resourceManager;

    private void Awake()
    {
        currentState = State.FarmingHome;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        littleGuyManager = FindFirstObjectByType<LittleGuyManager>();
        farmingNodes = FindObjectsByType<FarmingNode>(FindObjectsSortMode.None);
        interactable = GetComponent<Interactable>();
        resourceManager = FindFirstObjectByType<ResourceManager>();
    }

    private void Start()
    {
        state = currentState;
    }

    private void Update()
    {
        CheckBehaviorChanged();
    }

    private void FixedUpdate()
    {
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

            interactable.spriteRenderer.sprite = interactable.defaultSprite;
            interactable.enabled = false;
            col.radius = 0.5f;
            rb.MovePosition(Vector2.MoveTowards(rb.position, target, moveSpeed * Time.deltaTime));
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

            rb.MovePosition(Vector2.MoveTowards(rb.position, target, moveSpeed * Time.deltaTime));
        }
        else if (chosenNode != null)
        {
            chosenNode.occupant = null;
            chosenNode = null;
        }

        if (currentState == State.ReturningHome)
        {
            col.radius = 0.5f;

            if (chosenPickup != gameObject.GetComponentInChildren<Pickup>())
            {
                target = chosenPickup.transform.position;
                rb.MovePosition(Vector2.MoveTowards(rb.position, target, moveSpeed * Time.deltaTime));
            }
            else if (chosenPickup == gameObject.GetComponentInChildren<Pickup>())
            {
                target = resourceManager.transform.position;
                rb.MovePosition(Vector2.MoveTowards(rb.position, target, moveSpeed * Time.deltaTime));
            }
            else if (chosenPickup == null)
            {
                currentState = State.FarmingHome;
            }
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

    public void UpdatePickups()
    {
        pickups = FindObjectsByType<Pickup>(FindObjectsSortMode.None);

        foreach (var pickup in pickups)
        {
            if (pickup.chosen == this)
            {
                chosenPickup = pickup;
                break;
            }
        }
    }
}