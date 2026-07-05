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

    [Header("Sprite")]
    [SerializeField] public Sprite defaultSprite;
    [SerializeField] public Sprite outlinedSprite;

    bool canInteract;

    SpriteRenderer spriteRenderer;
    InputAction mineAction;
    InputAction interactAction;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mineAction = InputSystem.actions.FindAction("Mine");
        interactAction = InputSystem.actions.FindAction("Interact");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        canInteract = true;
        spriteRenderer.sprite = outlinedSprite;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        canInteract = false;
        spriteRenderer.sprite = defaultSprite;
    }

    void CheckMineInput()
    {
        if (mineAction.triggered && canInteract)
        {
            if (what == What.Wood || what == What.Stone || what == What.Wolf)
            {

            }
        }
    }

    void CheckRecruitInput()
    {
        if (interactAction.triggered && canInteract && what == What.LittleGuy)
        {

        }
    }
}
