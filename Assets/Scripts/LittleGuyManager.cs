using UnityEngine;

public class LittleGuyManager : MonoBehaviour
{
    [SerializeField] int littleGuysPerRow = 5;
    [SerializeField] float littleGuysSpacing = 2f;
    [SerializeField] Vector2 firstPosition = new(2, 2);

    public Vector3[] LittleGuysTarget { get { return littleGuysTarget; } }

    GameObject player;

    LittleGuy[] littleGuys;
    static Vector3[] littleGuysTarget;
    Vector3 playerPosition;

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerController>().gameObject;
    }

    private void Start()
    {
        UpdateLittleGuysTarget();
        GiveLittleGuysIndex();
    }

    private void Update()
    {
        if (playerPosition != player.transform.position)
        {
            UpdateLittleGuysTarget();
            playerPosition = player.transform.position;
        }
    }

    public void UpdateLittleGuysTarget()
    {
        littleGuys = FindObjectsByType<LittleGuy>(FindObjectsSortMode.None);
        int littleGuysFollowingPlayer = 0;

        foreach (LittleGuy l in littleGuys)
        {
            if (l.currentState == LittleGuy.State.FollowingPlayer)
            {
                littleGuysFollowingPlayer++;
            }
        }
        littleGuysTarget = new Vector3[littleGuysFollowingPlayer];
        littleGuysFollowingPlayer = 0;
        for (int i = 0; i < littleGuys.Length; i++)
        {
            if (littleGuys[i].currentState == LittleGuy.State.FollowingPlayer)
            {
                littleGuysTarget[littleGuysFollowingPlayer] = (Vector2)player.transform.position + firstPosition - new Vector2(i % littleGuysPerRow * littleGuysSpacing, i / littleGuysPerRow * littleGuysSpacing);
                littleGuysFollowingPlayer++;
            }
        }
    }

    public void GiveLittleGuysIndex()
    {
        littleGuys = FindObjectsByType<LittleGuy>(FindObjectsSortMode.None);
        int littleGuysFollowingPlayer = 0;

        for (int i = 0; i < littleGuys.Length -1; i++)
        {
            if (littleGuys[i].currentState == LittleGuy.State.FollowingPlayer)
            {
                littleGuysFollowingPlayer++;
                littleGuys[i].targetIndex = littleGuysFollowingPlayer;
            }
        }
    }
}
