using UnityEngine;

public class BoarAI : EnemyBase
{

    public override void TakeDMG(int amount)
    {
        health -= amount;
        if (health <= 0)
            stateMachine.ChangeState(new DeadState(this));
    }

}
