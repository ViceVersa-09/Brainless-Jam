using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    public enum What
    {
        Wood,
        Stone,
        LittleGuy,
        Wolf
    }

    [Header("Information")]
    [SerializeField] What what;
    [SerializeField] float health;

    [Header("Not Little Guy")]
    [SerializeField] float damagePerGuy;
    [SerializeField] GameObject itemDrop;
    [SerializeField] float shakeMagnitude;
    [SerializeField] float shakeDuration;

    [Header("Sprites")]
    [SerializeField] public Sprite defaultSprite;
    [SerializeField] public Sprite outlinedSprite;

    public bool canInteract;
    bool canAttack = true;

    SpriteRenderer spriteRenderer;
    InputAction mineAction;
    InputAction recruitAction;
    InputAction unRecruitAction;
    PlayerController playerController;

    #region Unity Methods

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerController = FindFirstObjectByType<PlayerController>();
        mineAction = InputSystem.actions.FindAction("Mine");
        recruitAction = InputSystem.actions.FindAction("Interact");
        unRecruitAction = InputSystem.actions.FindAction("UnRecruit");
    }

    private void Update()
    {
        CheckMineInput();
        CheckRecruitInput();

        if (playerController.interactingWith == this && canInteract)
        {
            spriteRenderer.sprite = outlinedSprite;
        }
        else
        {
            canInteract = false;
            spriteRenderer.sprite = defaultSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int playerLayer = LayerMask.NameToLayer("Player");

        if (other.gameObject.layer == playerLayer)
        {
            playerController.interactingWith = this;
            canInteract = true;
            canAttack = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        canInteract = false;
    }

    #endregion

    void CheckMineInput()
    {
        if (mineAction.triggered && canInteract && playerController.interactingWith == this)
        {        
            if (what == What.Wood || what == What.Stone)
            {
                StartCoroutine(Mine());
            }
            else if (what == What.Wolf && canAttack)
            {
                StopAllCoroutines();
                StartCoroutine(PlayerAttack());
            }
        }
    }

    IEnumerator Mine()
    {
        LittleGuy[] littleGuys = FindObjectsByType<LittleGuy>(FindObjectsSortMode.None);
        playerController.canControl = false;

        foreach (var littleGuy in littleGuys)
        {
            if (littleGuy.currentState == LittleGuy.State.FollowingPlayer)
            {
                health -= damagePerGuy;
            }
        }

        float timeBetweenShake = health / 3;

        for (int i = 0; i < 3; i++)
        {
            Vector2 originalPos = transform.localPosition;
            float elapsed = 0f;

            while (elapsed < shakeDuration)
            {
                float offsetX = UnityEngine.Random.Range(-1, 1) * shakeMagnitude;
                float offsetY = UnityEngine.Random.Range(-1, 1) * shakeMagnitude;
                transform.localPosition = originalPos + new Vector2(offsetX, offsetY);
                yield return new WaitForEndOfFrame();
                elapsed += Time.deltaTime;
            }
            
            transform.localPosition = originalPos;
            yield return new WaitForSeconds(timeBetweenShake - shakeDuration);
        }

        Break();
    }

    IEnumerator PlayerAttack()
    {
        WolfController wolfController = GetComponent<WolfController>();
        LittleGuy[] littleGuys = FindObjectsByType<LittleGuy>(FindObjectsSortMode.None);
        canAttack = false;
        float allGuysDamage = 0f;

        foreach (var littleGuy in littleGuys)
        {
            if (littleGuy.currentState == LittleGuy.State.FollowingPlayer)
            {
                allGuysDamage += damagePerGuy;
            }
        }

        health -= allGuysDamage + wolfController.playerDamage;
        Vector2 originalPos = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float offsetX = UnityEngine.Random.Range(-1, 1) * shakeMagnitude;
            float offsetY = UnityEngine.Random.Range(-1, 1) * shakeMagnitude;
            transform.localPosition = originalPos + new Vector2(offsetX, offsetY);
            yield return new WaitForEndOfFrame();
            elapsed += Time.deltaTime;
        }

        transform.localPosition = originalPos;

        if (health <= 0)
        {
            Break();
        }

        yield return new WaitForSeconds(wolfController.playerAttackSpeed);
        canAttack = true;
    }

    void Break()
    {
        canAttack = true;
        playerController.canControl = true;
        if (itemDrop != null)
        {
            Instantiate(itemDrop);
        }        
        Destroy(gameObject);
    }

    void CheckRecruitInput()
    {
        if (recruitAction.triggered && canInteract && what == What.LittleGuy)
        {
            LittleGuy littleGuy = GetComponent<LittleGuy>();

            enabled = false;
            littleGuy.currentState = LittleGuy.State.FollowingPlayer;
            playerController.maxHealth += health;
        }
    }

    public void UnRecruit()
    {
        if (unRecruitAction.triggered && what == What.LittleGuy)
        {
            LittleGuy littleGuy = GetComponent<LittleGuy>();

            enabled = true;
            littleGuy.currentState = LittleGuy.State.FarmingHome;
            playerController.maxHealth -= health;
        }
    }
}
