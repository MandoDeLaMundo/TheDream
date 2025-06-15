using UnityEngine;

public class BoarAI : EnemyBase
{
    [Header("Boar AI Settings")]
    [SerializeField] public float idleTime;

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new StateMachine();
        stateMachine.ChangeState(new IdleState(this, idleTime));
    }

    public override void TakeDMG(int amount)
    {
        health -= amount;
        if (health <= 0)
            stateMachine.ChangeState(new DeadState(this));
    }

}
