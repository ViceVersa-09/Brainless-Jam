using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [Header("Tree Settings")]
    [SerializeField] private int treeSpawnLimit = 20;
    [SerializeField] private float treeDelay = 0.1f;
    [SerializeField] private Vector2 miniTree = new(0f, 0f);
    [SerializeField] private Vector2 maxiTree = new(0f, 0f);
    [SerializeField] GameObject treePrefab;

    [Header("Stone Settings")]
    [SerializeField] private int stoneSpawnLimit = 20;
    [SerializeField] private float stoneDelay = 0.1f;
    [SerializeField] private Vector2 miniStone = new(0f, 0f);
    [SerializeField] private Vector2 maxiStone = new(0f, 0f);
    [SerializeField] GameObject stonePrefab;

    [Header("Other stuff")]
    [SerializeField] private float startDelay = 0.1f;
    [SerializeField] private float spawnCheckRadius = 0.25f;

    private int currentTreeSpawnCount;
    private int currentStoneSpawnCount;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnTree), startDelay, treeDelay);
        InvokeRepeating(nameof(SpawnStone), startDelay, stoneDelay);

        currentTreeSpawnCount = 0;
        currentStoneSpawnCount = 0;
    }

    private void SpawnTree()
    {
        if (currentTreeSpawnCount >= treeSpawnLimit)
        {
            CancelInvoke(nameof(SpawnTree));
            return;
        }

        Vector2 spawnPosition = new(Random.Range(miniTree.x, maxiTree.x), Random.Range(miniTree.y, maxiTree.y));

        if (Physics2D.OverlapCircle(spawnPosition, spawnCheckRadius) == null)
        {
            Instantiate(treePrefab, spawnPosition, Quaternion.identity);
            currentTreeSpawnCount++;
        }
    }

    private void SpawnStone()
    {
        if (currentStoneSpawnCount >= stoneSpawnLimit)
        {
            CancelInvoke(nameof(SpawnStone));
            return;
        }

        Vector2 spawnPosition = new(Random.Range(miniStone.x, maxiStone.x), Random.Range(miniStone.y, maxiStone.y));

        if (Physics2D.OverlapCircle(spawnPosition, spawnCheckRadius) == null)
        {
            Instantiate(stonePrefab, spawnPosition, Quaternion.identity);
            currentStoneSpawnCount++;
        }
    }
}