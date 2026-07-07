using System.Collections;
using Unity.VisualScripting;
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

    bool hunting;

    Rigidbody2D rb;
    PlayerController playerController;
    Coroutine attackRoutine;
    Interactable interactable;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        interactable = GetComponent<Interactable>();
        playerController = FindFirstObjectByType<PlayerController>();
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

        while (Vector2.Distance(transform.position, playerController.transform.position) > dashDistance)
        {
            rb.linearVelocity = dashSpeed * (playerController.transform.position - transform.position);
            yield return new WaitForEndOfFrame();
        }      

        playerController.currentHealth -= damage;

        while (originalPos - transform.position != Vector3.zero)
        {
            rb.linearVelocity = dashSpeed * (originalPos - transform.position);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(attackSpeed);

        attackRoutine = null;
    }
}
