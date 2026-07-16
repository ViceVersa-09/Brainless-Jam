using UnityEngine;

public class Pickup : MonoBehaviour
{
    enum WhatMaterial
    {
        Wood,
        Stone,
    }

    [SerializeField] WhatMaterial material;
    [SerializeField] Vector2 guyOffset;

    LittleGuy[] littleGuys;
    [HideInInspector] public LittleGuy chosen;

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

        if (chosen == null)
        {
            foreach (var littleGuy in littleGuys)
            {
                if (littleGuy.chosenPickup == null)
                {
                    chosen = littleGuy;
                    break;
                }
            }
        }
        
        if (chosen != null)
        {
            chosen.currentState = LittleGuy.State.ReturningHome;
            chosen.UpdatePickups();
        }

        ResourceManager resourceManager = FindFirstObjectByType<ResourceManager>();

        if (material == WhatMaterial.Wood)
        {
            resourceManager.currentWood++;
        }
        else if (material == WhatMaterial.Stone)
        {
            resourceManager.currentStone++;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Barn"))
        {
            Mission mission = FindFirstObjectByType<Mission>();
            mission.UpdateMissionText();
            chosen.currentState = LittleGuy.State.FarmingHome;
            Destroy(gameObject);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("LittleGuy"))
        {
            LittleGuy littleGuy = other.GetComponent<LittleGuy>();

            if (littleGuy.currentState == LittleGuy.State.ReturningHome)
            {
                transform.SetParent(chosen.transform);
                transform.localPosition = guyOffset;
            }  
        }
    }
}
