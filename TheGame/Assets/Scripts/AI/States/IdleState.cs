using UnityEngine;

public class IdleState : IState
{
    EnemyBase enemy;
    float timer;

    public IdleState(EnemyBase _enemy)
    {
        enemy = _enemy;
    }

    public void Enter()
    {
        timer = 0;
        if (enemy.agent)
        {
            enemy.agent.ResetPath();
            enemy.agent.isStopped = true;
        }

        // Play Idle animation
    }

    public void Update()
    {
        timer += Time.deltaTime;

        if (enemy.playerInRange && enemy.CanSeePlayer())
        {
            if (enemy is CowardEnemy || enemy is StationaryEnemy)
            {
                enemy.stateMachine.ChangeState(new AttackState(enemy));
                return;
            }

            enemy.stateMachine.ChangeState(new ChaseState(enemy));
            return;
        }

        else if (timer >= enemy.roamPauseTime && !(enemy is StationaryEnemy))
        {
            enemy.stateMachine.ChangeState(new PatrolState(enemy));
        }
    }

    public void Exit()
    {
        if (enemy.agent)
        {
            enemy.agent.isStopped = false;
        }
    }
}
