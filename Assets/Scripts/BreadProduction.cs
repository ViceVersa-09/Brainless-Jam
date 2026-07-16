using System.Collections;
using UnityEngine;

public class BreadProduction : MonoBehaviour
{
    [SerializeField] float breadProductionSpeed = 5f;
    [SerializeField] int breadQuantity = 1;

    int manPower;
    int breadProduced;
    Coroutine produceBread;

    ResourceManager resourceManager;

    private void Awake()
    {
        resourceManager = FindFirstObjectByType<ResourceManager>();
    }

    private void Update()
    {
        if (!GameManager.Day.IsDay)
        {
            produceBread = null;
        }
        else
        {
            produceBread ??= StartCoroutine(ProduceBread());
            //GameManager.instance.bread += breadProduced;
            breadProduced = 0;
        }
    }

    IEnumerator ProduceBread()
    {
        while (true)
        {
            yield return new WaitForSeconds(breadProductionSpeed);
            breadProduced += breadQuantity * manPower;
            Debug.Log(GameManager.instance.bread);
        }
    }

    public void IncreaseManPower(int addedManPower)
    {
        manPower += addedManPower;
    }
}
