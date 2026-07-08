using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    [Header("Delay Settings")]
    [SerializeField] private float treeDelay = 0.1f;
    [SerializeField] private float treeGroupDelay = 0.1f;
    [SerializeField] private float stoneDelay = 0.1f;
    [SerializeField] private float stoneGroupDelay = 0.1f;

    [Header("Limit Settings")]
    [SerializeField] private int treeSpawnLimit = 20;
    [SerializeField] private int treeGroupSpawnLimit = 20;
    [SerializeField] private int stoneSpawnLimit = 20;
    [SerializeField] private int stoneGroupSpawnLimit = 20;

    [Header("Other stuff")]
    [SerializeField] private float startDelay = 0.1f;
    [SerializeField] private float spawnCheckRadius = 0.25f;
    [SerializeField] private Vector2 mini = new(0f, 0f);
    [SerializeField] private Vector2 maxi = new(0f, 0f);

    [SerializeField] GameObject treePrefab;
    [SerializeField] GameObject treeGroupPrefab;
    [SerializeField] GameObject stonePrefab;
    [SerializeField] GameObject stoneGroupPrefab;

    private int currentTreeSpawnCount;
    private int currentTreeGroupSpawnCount;
    private int currentStoneSpawnCount;
    private int currentStoneGroupSpawnCount;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnTree), startDelay, treeDelay);
        InvokeRepeating(nameof(SpawnTreeGroup), startDelay, treeGroupDelay);
        InvokeRepeating(nameof(SpawnStone), startDelay, stoneDelay);
        InvokeRepeating(nameof(SpawnStoneGroup), startDelay, stoneGroupDelay);

        currentTreeSpawnCount = 0;
        currentTreeGroupSpawnCount = 0;
        currentStoneSpawnCount = 0;
        currentStoneGroupSpawnCount = 0;
    }

    void Spawn(ref int spawnCount, int spawnLimit, GameObject prefab, string invokeName)
    {
        if (spawnCount >= spawnLimit)
        {
            CancelInvoke(invokeName);
            return;
        }

        Vector2 spawnPosition = new(Random.Range(mini.x, maxi.x), Random.Range(mini.y, maxi.y));

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

    private void SpawnTreeGroup()
    {
        Spawn(ref currentTreeGroupSpawnCount, treeGroupSpawnLimit, treeGroupPrefab, nameof(SpawnTreeGroup));
    }

    private void SpawnStoneGroup()
    {
        Spawn(ref currentStoneGroupSpawnCount, stoneGroupSpawnLimit, stoneGroupPrefab, nameof(SpawnStoneGroup));
    }
}