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
        enemy.agent.isStopped = true;
        timer = 0;
    }

    public void Update()
    {
        timer += Time.deltaTime;

        if (enemy.playerInRange)
        {
            enemy.stateMachine.ChangeState(new ChaseState(enemy));
        }

        else if (timer >= enemy.roamPauseTime)
        {
            enemy.stateMachine.ChangeState(new PatrolState(enemy));
        }
    }

    public void Exit()
    {
        enemy.agent.isStopped = false;
    }
}
