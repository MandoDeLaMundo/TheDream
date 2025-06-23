using UnityEngine;

public class PetrifyTrigger : MonoBehaviour
{
    private bool playerInZone = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
        }
    }

    public bool IsPlayerInZone
    {
        get { return playerInZone; }
    }
}
