using System.Collections;
using UnityEngine;

public class WolfHideout : MonoBehaviour
{
    [SerializeField] GameObject wolfPrefab;
    [SerializeField] float spawnCooldown;

    GameObject currentWolf;

    private void Start()
    {
        StartCoroutine(Spawner());
    }

    IEnumerator Spawner()
    {
        while (true)
        {
            if (currentWolf == null)
            {
                currentWolf = Instantiate(wolfPrefab, transform.position - new Vector3(0, 1), Quaternion.identity);

                yield return new WaitForSeconds(spawnCooldown);
            }
            else
            {
                yield return new WaitForSeconds(spawnCooldown);
            }
        }
    }
}
