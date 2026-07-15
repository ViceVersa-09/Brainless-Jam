using System.Collections;
using UnityEngine;

public class WolfController : MonoBehaviour
{
    [Header("Hunting for Player")]
    [SerializeField] float moveSpeed;
    [SerializeField] float stopDistance;
    [SerializeField] float huntDistance;

    [Header("Attacking")]
    [SerializeField] float damage;
    [SerializeField] float attackSpeed;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashDistance;

    [Header("Players attack stats")]
    [SerializeField] public float playerAttackSpeed;
    [SerializeField] public float playerDamage;

    bool hunting;

    Rigidbody2D rb;
    PlayerController playerController;
    Coroutine attackRoutine;
    Interactable interactable;
    Animator animator;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        interactable = GetComponent<Interactable>();
        playerController = FindFirstObjectByType<PlayerController>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, playerController.transform.position) <= huntDistance && playerController.canControl)
        {
            hunting = true;
            Hunt();
        }
        else
        {
            hunting = false;
        }

        if (Vector2.Distance(transform.position, playerController.transform.position) <= stopDistance && attackRoutine == null)
        {
            if (playerController.interactingWith == interactable || playerController.canControl)
            {
                attackRoutine = StartCoroutine(Attack());
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, playerController.transform.position) < stopDistance)
            {
                rb.linearVelocity = moveSpeed * -(playerController.transform.position - transform.position);
            }
            else if (attackRoutine == null && !hunting)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }

        if (rb.linearVelocityX != 0)
        {
            animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        }
        else
        {
            animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.y));
        }

        if (rb.linearVelocityX < -0.2)
        {
            spriteRenderer.flipX = true;
        }
        else if (rb.linearVelocityX > 0.2)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        int playerLayer = LayerMask.NameToLayer("Player");

        if (other.gameObject.layer == playerLayer && attackRoutine != null)
        {
            playerController.currentHealth -= damage;
        }
    }

    void Hunt()
    {
        if (Vector2.Distance(transform.position, playerController.transform.position) > stopDistance)
        {
            rb.linearVelocity = moveSpeed * (playerController.transform.position - transform.position);
        }
        else if (Vector2.Distance(transform.position, playerController.transform.position) == stopDistance && attackRoutine == null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    IEnumerator Attack()
    {
        Vector3 originalPos = transform.position;
        animator.SetBool("Attacking", true);
        Vector3 playerPosition = playerController.transform.position;

        while (Vector2.Distance(transform.position, playerPosition) > dashDistance)
        {
            rb.linearVelocity = dashSpeed * (playerPosition - transform.position);
            yield return new WaitForEndOfFrame();
        }

        animator.SetBool("Attacking", false);

        while (originalPos - transform.position != Vector3.zero)
        {
            rb.linearVelocity = dashSpeed * (originalPos - transform.position);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(attackSpeed);      
        attackRoutine = null;
    }
}
