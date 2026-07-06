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
                currentWolf = Instantiate(wolfPrefab, transform);

                yield return new WaitForSeconds(spawnCooldown);
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
