using UnityEngine;

public class ChargingEnemy : EnemyBase
{
    [Header("Charge Settings")]
    public float chargeRange;
    public float chargeSpeed;
    public float chargeDuration;
    public float chargeCooldown;
    public int chargeDamage;
    public Collider chargeCollider;

    [HideInInspector] public bool canCharge = true;
    [HideInInspector] public bool isCharging = false;
    [HideInInspector] public bool hitPlayerDuringCharge = false;

    public void StartCharge()
    {
        isCharging = true;
        agent.isStopped = false;
    }

    public void StopCharge()
    {
        isCharging = false;
        agent.isStopped = true;
    }
}
