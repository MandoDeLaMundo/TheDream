using UnityEngine;

public class StationaryEnemy : EnemyBase
{
    public override void TakeDMG(int amount)
    {
        health -= amount;
        UpdateEnemyUI();

        if (health <= 0)
        {
            stateMachine.ChangeState(new DeadState(this));
            return;
        }

        StartCoroutine(FlashRed());

        stateMachine.ChangeState(new AttackState(this));
    }
}
