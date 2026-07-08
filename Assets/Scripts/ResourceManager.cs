using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] GameObject gates;

    [HideInInspector] public int currentWood;
    [HideInInspector] public int currentStone;
    [HideInInspector] public int currentBread;
    [HideInInspector] public int leftoverBread;

    [HideInInspector] public int wood;
    [HideInInspector] public int stone;
    [HideInInspector] public int bread;

    public void CountBread()
    {
        LittleGuy[] everyLittleGuy = FindObjectsByType<LittleGuy>(FindObjectsSortMode.None);

        bread = 0;

        foreach (var littleGuy in everyLittleGuy)
        {
            if (littleGuy.currentState == LittleGuy.State.FarmingHome)
            {
                bread++;
            }
        }
    }

    public void EndOfDayMaterials()
    {
        leftoverBread = bread;

        wood += currentWood;
        currentWood = 0;

        stone += currentStone;
        currentStone = 0;

        bread += currentBread;
        currentBread = 0;
    }
}
