using UnityEngine;
using UnityEngine.InputSystem;

public class Gates : MonoBehaviour
{
    public bool canInteract;

    GameManager gameManager;
    InputAction interactAction;
    Animator animator;
    public BoxCollider2D gateCollider;
    Tutorial tutorial;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        interactAction = InputSystem.actions.FindAction("Interact");
        animator = GetComponent<Animator>();
        gateCollider = GetComponent<BoxCollider2D>();
        tutorial = FindFirstObjectByType<Tutorial>();
    }

    private void Update()
    {
        if (tutorial != null && tutorial.canOpenGates || tutorial == null)
        {
            if (canInteract && interactAction.triggered)
            {
                OpenGates();
            }
        }       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int playerLayer = LayerMask.NameToLayer("Player");

        if (other.gameObject.layer == playerLayer)
        {
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        int playerLayer = LayerMask.NameToLayer("Player");

        if (other.gameObject.layer == playerLayer)
        {
            canInteract = false;
        }
    }

    void OpenGates()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.gatesClip);
        gameManager.StartGame();
        //gameObject.SetActive(false);
        animator.SetInteger("State", 1);
        gateCollider.enabled = false;
    }
}
