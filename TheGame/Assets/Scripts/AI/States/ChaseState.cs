using UnityEngine;

public class ChaseState : IState
{
    EnemyBase enemy;
    
    public ChaseState(EnemyBase _enemy)
    {
        enemy = _enemy;
    }

    public void Enter()
    {
        
    }

    public void Update()
    {
        if (!enemy.CanSeePlayer())
        {
            enemy.stateMachine.ChangeState(new PatrolState(enemy));
            return;
        }

        enemy.agent.SetDestination(gameManager.instance.player.transform.position);

        if (enemy.agent.remainingDistance <= enemy.agent.stoppingDistance)
        {
            enemy.stateMachine.ChangeState(new AttackState(enemy));
        }
    }

    public void Exit()
    {

    }
}
