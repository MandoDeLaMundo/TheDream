using UnityEngine;

public class MudZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        playerController player = other.GetComponent<playerController>();
        if (player != null)
        {
            player.EnterMud();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerController player = other.GetComponent<playerController>();
        if (player != null)
        {
            player.ExitMud();
        }
    }
}