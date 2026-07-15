using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    [Header("Delay Settings")]
    [SerializeField] private float treeDelay = 0.1f;
    [SerializeField] private float treeGroupDelay = 0.1f;
    [SerializeField] private float stoneDelay = 0.1f;
    [SerializeField] private float stoneGroupDelay = 0.1f;
    [SerializeField] private float wolfDelay = 0.1f;

    [Header("Limit Settings")]
    [SerializeField] private int treeSpawnLimit = 20;
    [SerializeField] private int treeGroupSpawnLimit = 20;
    [SerializeField] private int stoneSpawnLimit = 20;
    [SerializeField] private int stoneGroupSpawnLimit = 20;
    [SerializeField] private int wolfSpawnLimit = 20;

    [Header("Other stuff")]
    [SerializeField] private float startDelay = 0.1f;
    [SerializeField] private float spawnCheckRadius = 0.25f;
    [SerializeField] private Vector2 mini = new(0f, 0f);
    [SerializeField] private Vector2 maxi = new(0f, 0f);
    [SerializeField] private Vector2 baseMin = new(0f, 0f);
    [SerializeField] private Vector2 baseMax = new(0f, 0f);

    [SerializeField] GameObject treePrefab;
    [SerializeField] GameObject treeGroupPrefab;
    [SerializeField] GameObject stonePrefab;
    [SerializeField] GameObject stoneGroupPrefab;
    [SerializeField] GameObject wolfPrefab;

    private int currentTreeSpawnCount;
    private int currentTreeGroupSpawnCount;
    private int currentStoneSpawnCount;
    private int currentStoneGroupSpawnCount;
    private int currentWolfSpawnCount;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnTree), startDelay, treeDelay);
        //InvokeRepeating(nameof(SpawnTreeGroup), startDelay, treeGroupDelay);
        InvokeRepeating(nameof(SpawnStone), startDelay, stoneDelay);
        InvokeRepeating(nameof(SpawnWolf), startDelay, wolfDelay);
        //InvokeRepeating(nameof(SpawnStoneGroup), startDelay, stoneGroupDelay);

        currentTreeSpawnCount = 0;
        currentTreeGroupSpawnCount = 0;
        currentStoneSpawnCount = 0;
        currentStoneGroupSpawnCount = 0;
        currentWolfSpawnCount = 0;

        GameManager.instance.SpawnLittleGuys();
        AudioManager.instance.PlaySFX(AudioManager.instance.dayStartClip);
    }

    void Spawn(ref int spawnCount, int spawnLimit, GameObject prefab, string invokeName)
    {
        if (spawnCount >= spawnLimit)
        {
            CancelInvoke(invokeName);
            return;
        }

        Vector2 spawnPosition = new Vector2(0, 0);

        for (int i = 0; i < 1; i++)
        {
            spawnPosition = new(Random.Range(mini.x, maxi.x), Random.Range(mini.y, maxi.y));

            if (spawnPosition.x > baseMin.x && spawnPosition.y > baseMin.y && spawnPosition.x < baseMax.x && spawnPosition.y < baseMax.y)
            {
                i--;
            }
            else
            {
                i++;
            }
        }
        

        if (Physics2D.OverlapCircle(spawnPosition, spawnCheckRadius) == null)
        {
            Instantiate(prefab, spawnPosition, Quaternion.identity);
            spawnCount++;
        }
    }

    private void SpawnTree()
    {
        Spawn(ref currentTreeSpawnCount, treeSpawnLimit, treePrefab, nameof(SpawnTree));
    }

    private void SpawnStone()
    {
        Spawn(ref currentStoneSpawnCount, stoneSpawnLimit, stonePrefab, nameof(SpawnStone));
    }

    private void SpawnWolf()
    {
        Spawn(ref currentWolfSpawnCount, wolfSpawnLimit, wolfPrefab, nameof(SpawnWolf));
    }

    private void SpawnTreeGroup()
    {
        Spawn(ref currentTreeGroupSpawnCount, treeGroupSpawnLimit, treeGroupPrefab, nameof(SpawnTreeGroup));
    }

    private void SpawnStoneGroup()
    {
        Spawn(ref currentStoneGroupSpawnCount, stoneGroupSpawnLimit, stoneGroupPrefab, nameof(SpawnStoneGroup));
    }
}