using UnityEngine;
using UnityEngine.InputSystem;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] GameObject gates;

    [HideInInspector] public int currentWood;
    [HideInInspector] public int currentStone;

    [HideInInspector] public int wood;
    [HideInInspector] public int stone;

    GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.instance;

        GameManager.Day.CurrentDay++;
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
