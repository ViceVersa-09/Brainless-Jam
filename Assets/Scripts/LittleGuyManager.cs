using System.Linq;
using UnityEngine;

public class LittleGuyManager : MonoBehaviour
{
    [SerializeField] int littleGuysPerRow = 5;
    [SerializeField] float littleGuysSpacing = 2f;
    [SerializeField] Vector2 firstPosition = new(2, 2);
    public static Vector3[] LittleGuysTarget { get { return littleGuysTarget; } }

    GameObject player;

    LittleGuy[] littleGuys;
    static Vector3[] littleGuysTarget;

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerController>().gameObject;
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
        for (int i = 0; i < littleGuys.Length; i++)
        {
            if (littleGuys[i].currentState == LittleGuy.State.FollowingPlayer)
            {
                littleGuysTarget[i] = (Vector2)player.transform.position + firstPosition - new Vector2(i % littleGuysPerRow * littleGuysSpacing, i / littleGuysPerRow * littleGuysSpacing);
            }
        }
    }
}
