using UnityEngine;

public class BoulderSpawner: MonoBehaviour
{
    [SerializeField] private BoulderPush boulder;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boulder.ActivateBoulder();
        }
    }
}