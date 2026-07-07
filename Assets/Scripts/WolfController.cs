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
        if (Vector2.Distance(transform.position, playerController.transform.position) <= huntDistance)
        {
            Hunt();
        }       

        if (Vector2.Distance(transform.position, playerController.transform.position) <= stopDistance && attackRoutine == null && playerController.interactingWith == interactable)
        {
            attackRoutine = StartCoroutine(Attack());
        }
    }

    void Hunt()
    {
        if (Vector2.Distance(transform.position, playerController.transform.position) > stopDistance)
        {
            rb.linearVelocity = moveSpeed * (playerController.transform.position - transform.position);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    IEnumerator Attack()
    {
        // It should like dash towards you
        playerController.health -= damage;

        yield return new WaitForSeconds(attackSpeed);

        attackRoutine = null;
    }
}
