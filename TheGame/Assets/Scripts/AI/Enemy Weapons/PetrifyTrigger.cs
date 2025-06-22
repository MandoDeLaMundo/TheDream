using UnityEngine;

public class PetrifyTrigger : MonoBehaviour
{
    [SerializeField] private float petrifyTime;
    private float timer = 0f;
    private bool playerInZone = false;
    private playerController player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            player = other.GetComponent<playerController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            timer = 0f;
        }
    }

    private void Update()
    {
        if (playerInZone && player != null)
        {
            timer += Time.deltaTime;

            if (timer >= petrifyTime)
            {
                player.Stun(2f);
                timer = 0f;
            }
        }
    }
}
