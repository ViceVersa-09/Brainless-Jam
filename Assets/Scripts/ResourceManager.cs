using UnityEngine;
using UnityEngine.InputSystem;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] GameObject gates;

    int wood;
    int stone;
    int currentWood;
    int currentStone;

    GameManager gameManager;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        gameManager.currentDay++;
    }

    public void CountBread()
    {
        LittleGuy[] everyLittleGuy = FindObjectsByType<LittleGuy>(FindObjectsSortMode.None);

        gameManager.bread = 0;

        foreach (var littleGuy in everyLittleGuy)
        {
            if (littleGuy.currentState == LittleGuy.State.FarmingHome)
            {
                gameManager.bread++;
            }
        }
    }

    void EndOfDayMaterials()
    {
        wood += currentWood;
        currentWood = 0;

        stone += currentStone;
        currentStone = 0;
    }
}
