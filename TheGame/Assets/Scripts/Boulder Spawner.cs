using UnityEngine;

public class BoulderSpawner: MonoBehaviour
{
    [SerializeField] GameObject boulderObj;
    [SerializeField] private BoulderPush boulder;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boulderObj.SetActive(true);
            boulder.ActivateBoulder();
        }
    }
}