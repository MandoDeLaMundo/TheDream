using UnityEngine;

public class FleeState : IState
{
    CowardEnemy enemy;
    float attackCooldown;
    float attackTimer;

    public FleeState(CowardEnemy _enemy)
    {
        enemy = _enemy;
        attackTimer = attackCooldown;
    }

    public void Enter()
    {
        enemy.agent.speed *= 2;
    }

    public void Update()
    {
        attackTimer += Time.deltaTime;

        if (enemy.CanShoot && attackTimer >= attackCooldown)
        {
            attackTimer = 0f;
            enemy.stateMachine.ChangeState(new AttackState(enemy));
        }

        else if (!enemy.CanShoot && enemy.playerInRange && enemy.CanSeePlayer())
        {
            Vector3 direction = (enemy.transform.position - gameManager.instance.player.transform.position).normalized;
            Vector3 newDestination = enemy.transform.position + direction * enemy.fleeDistance;
            
            if (enemy.agent.remainingDistance < 0.5f)
            {
                enemy.agent.SetDestination(newDestination);
            }
        }

        if (!enemy.playerInRange && !enemy.CanSeePlayer())
        {
            enemy.stateMachine.ChangeState(new IdleState(enemy));
        }
    }

    public void Exit() 
    { 
        enemy.agent.speed /= 2;
        enemy.CanShoot = true;
    }
}
