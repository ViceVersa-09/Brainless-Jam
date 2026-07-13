using UnityEngine;

public class Pickup : MonoBehaviour
{
    enum WhatMaterial
    {
        Wood,
        Stone,
    }

    [SerializeField] WhatMaterial material;

    LittleGuy[] littleGuys;
    LittleGuy chosen;

    private void Start()
    {
        littleGuys = FindObjectsByType<LittleGuy>(FindObjectsSortMode.None);

        foreach (var littleGuy in littleGuys)
        {
            if (littleGuy.currentState == LittleGuy.State.FollowingPlayer)
            {
                chosen = littleGuy;
                break;
            }
        }

        chosen.currentState = LittleGuy.State.ReturningHome;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Barn"))
        {
            ResourceManager resourceManager = other.GetComponent<ResourceManager>();

            if (material == WhatMaterial.Wood)
            {
                resourceManager.wood++;
            }
            else if (material == WhatMaterial.Stone)
            {
                resourceManager.stone++;
            }

            chosen.currentState = LittleGuy.State.FarmingHome;
            Destroy(gameObject);
        }
    }
}
