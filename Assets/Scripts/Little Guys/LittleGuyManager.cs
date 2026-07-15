using UnityEngine;

public class LittleGuyManager : MonoBehaviour
{
    [SerializeField] int littleGuysPerRow = 5;
    [SerializeField] float littleGuysSpacing = 2f;
    [SerializeField] bool centeredFormation = false;

    public Vector3[] LittleGuysTarget { get { return littleGuysTarget; } }

    PlayerController player;

    LittleGuy[] littleGuys;
    static Vector3[] littleGuysTarget;
    Vector3 playerPosition;

    Vector2 lastDirection;

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerController>();
    }

    private void Start()
    {
        UpdateLittleGuysTarget();
        GiveLittleGuysIndex();
    }

    private void Update()
    {
        PlayerController controller = player.GetComponent<PlayerController>();

        if (playerPosition != player.transform.position || lastDirection != controller.LastMoveDirection)
        {
            UpdateLittleGuysTarget();
            playerPosition = player.transform.position;
            lastDirection = controller.LastMoveDirection;
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

        Vector2 forward = player.GetComponent<PlayerController>().LastMoveDirection;
        Vector2 behind = -forward;
        Vector2 right = new Vector2(forward.y, -forward.x);

        int totalFollowers = littleGuysFollowingPlayer;
        littleGuysFollowingPlayer = 0;

        for (int i = 0; i < littleGuys.Length; i++)
        {
            if (littleGuys[i].currentState == LittleGuy.State.FollowingPlayer)
            {
                int row = littleGuysFollowingPlayer / littleGuysPerRow;
                int column = littleGuysFollowingPlayer % littleGuysPerRow;

                float offset;

                int guysInThisRow = Mathf.Min(littleGuysPerRow, totalFollowers - row * littleGuysPerRow);

                bool isLastRow = row == (totalFollowers - 1) / littleGuysPerRow;

                if (centeredFormation && isLastRow)
                {
                    offset = column - (guysInThisRow - 1) * 0.5f;
                }
                else
                {
                    offset = column - (littleGuysPerRow - 1) * 0.5f;
                }

                Vector2 sideways = offset * littleGuysSpacing * right;
                Vector2 backwards = (row + 1) * littleGuysSpacing * behind;

                littleGuysTarget[littleGuysFollowingPlayer] = player.transform.position + (Vector3)(sideways + backwards);

                littleGuysFollowingPlayer++;
            }
        }
    }

    public void GiveLittleGuysIndex()
    {
        littleGuys = FindObjectsByType<LittleGuy>(FindObjectsSortMode.None);
        int littleGuysFollowingPlayer = 0;

        for (int i = 0; i < littleGuys.Length; i++)
        {
            if (littleGuys[i].currentState == LittleGuy.State.FollowingPlayer)
            {
                littleGuys[i].targetIndex = littleGuysFollowingPlayer;
                littleGuysFollowingPlayer++;
            }
        }
    }

    public void RefreshFollowers()
    {
        UpdateLittleGuysTarget();
        GiveLittleGuysIndex();
    }
}