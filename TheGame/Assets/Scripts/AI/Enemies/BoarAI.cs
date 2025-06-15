using UnityEngine;

public class BoarAI : EnemyBase
{
    [Header("Boar AI Settings")]
    [SerializeField] public float idleTime;

    public override void TakeDMG(int amount)
    {
        health -= amount;
        if (health <= 0)
            stateMachine.ChangeState(new DeadState(this));
    }

}
