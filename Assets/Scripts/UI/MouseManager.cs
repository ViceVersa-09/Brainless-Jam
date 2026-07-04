using UnityEngine;
using UnityEngine.InputSystem;

public class MouseManager : MonoBehaviour
{
    SpriteRenderer interactableSpriterenderer;
    Interactable intereactable;
    Sprite interactableSprite;

    private void Update()
    {
        CheckForObjects();
    }

    void CheckForObjects()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit && hit.transform.CompareTag("Interactable"))
        {
            if (interactableSpriterenderer != null && interactableSprite != null)
            {
                interactableSpriterenderer.sprite = interactableSprite;
            }
            intereactable = hit.transform.GetComponent<Interactable>();
            interactableSpriterenderer = hit.transform.GetComponent<SpriteRenderer>();
            interactableSprite = interactableSpriterenderer.sprite;
            interactableSpriterenderer.sprite = intereactable.outlinedSprite;
        }
        else
        {
            if (interactableSprite != null)
            {
                interactableSpriterenderer.sprite = interactableSprite;
                intereactable = null;
                interactableSprite = null;
                interactableSpriterenderer = null;
            }
        }
    }
}
