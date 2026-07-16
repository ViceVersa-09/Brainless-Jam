using System.Collections;
using Unity.VisualScripting;
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
    [SerializeField] bool noOutline;

    [Header("Sprites")]
    [SerializeField] public Sprite defaultSprite;
    [SerializeField] public Sprite outlinedSprite;

    public bool canInteract;
    public bool canAttack = true;
    
    bool canRecruit = true;

    [HideInInspector] public SpriteRenderer spriteRenderer;
    InputAction mineAction;
    InputAction recruitAction;
    PlayerController playerController;
    LittleGuyManager littleGuyManager;
    GameManager gameManager;

    #region Unity Methods
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerController = FindFirstObjectByType<PlayerController>();
        littleGuyManager = FindFirstObjectByType<LittleGuyManager>();
        mineAction = InputSystem.actions.FindAction("Mine");
        recruitAction = InputSystem.actions.FindAction("Interact");
        gameManager = FindFirstObjectByType<GameManager>();

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
    }

    private void Update()
    {
        CheckMineInput();
        CheckRecruitInput();

        if (playerController.interactingWith == this && canInteract)
        {
            if (!noOutline)
            {
                spriteRenderer.sprite = outlinedSprite;
            }
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
        int playerLayer = LayerMask.NameToLayer("Player");

        if (other.gameObject.layer == playerLayer)
        {
            canInteract = false;
        }
    }

    #endregion

    void CheckMineInput()
    {
        if (mineAction.triggered && canInteract && playerController.interactingWith == this && playerController.canControl)
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
            Animator playerAnimator = playerController.GetComponent<Animator>();
            playerAnimator.SetTrigger("Punch");
            AudioManager.instance.PlaySFX(AudioManager.instance.materialMineClip);
            Vector2 originalPos = transform.localPosition;
            float elapsed = 0f;

            while (elapsed < shakeDuration)
            {
                float offsetX = Random.Range(-1, 1) * shakeMagnitude;
                float offsetY = Random.Range(-1, 1) * shakeMagnitude;
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
        AudioManager.instance.PlaySFX(AudioManager.instance.wolfDamageClip);

        while (elapsed < shakeDuration)
        {
            float offsetX = Random.Range(-1, 1) * shakeMagnitude;
            float offsetY = Random.Range(-1, 1) * shakeMagnitude;
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
        Animator playerAnimator = playerController.GetComponent<Animator>();
        playerAnimator.SetTrigger("Punch");
        AudioManager.instance.PlaySFX(AudioManager.instance.materialBreakClip);
        canAttack = true;
        playerController.canControl = true;
        if (itemDrop != null)
        {
            Instantiate(itemDrop, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    void CheckRecruitInput()
    {
        if (recruitAction.triggered && canInteract && !gameManager.isDay && canRecruit && what == What.LittleGuy && playerController.interactingWith == this)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.recruitClip);

            LittleGuy littleGuy = GetComponent<LittleGuy>();

            canRecruit = false;

            littleGuy.currentState = LittleGuy.State.FollowingPlayer;

            littleGuyManager.RefreshFollowers();

            playerController.maxHealth += health;
        }
    }

    public void EnableRecruiting()
    {
        canRecruit = true;
    }
}