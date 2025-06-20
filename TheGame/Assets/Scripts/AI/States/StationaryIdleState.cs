using UnityEngine;

public class StationaryIdleState : IState
{
    StationaryEnemy enemy;

    public StationaryIdleState(StationaryEnemy _enemy)
    {
        enemy = _enemy;
    }

    public void Enter()
    {
        if (enemy.agent)
        {
            enemy.agent.ResetPath();
            enemy.agent.isStopped = true;
        }
    }

    public void Update()
    {
        if (enemy.playerInRange && enemy.CanSeePlayer())
        {
            enemy.stateMachine.ChangeState(new AttackState(enemy));
        }
    }

    public void Exit()
    {
        enemy.agent.isStopped = false;
    }
}
