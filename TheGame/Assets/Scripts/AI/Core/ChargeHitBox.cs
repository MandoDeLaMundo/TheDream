using UnityEngine;

public class ChargeHitBox : MonoBehaviour
{
    private ChargingEnemy chargingEnemy;

    void Start()
    {
        chargingEnemy = GetComponentInParent<ChargingEnemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!chargingEnemy.isCharging)
            return;

        if (other.CompareTag("Player"))
        {
            playerController player = other.GetComponent<playerController>();
            if (player != null)
            {
                player.TakeDMG(chargingEnemy.chargeDamage);
                chargingEnemy.hitPlayerDuringCharge = true;
            }
        }
    }
}
