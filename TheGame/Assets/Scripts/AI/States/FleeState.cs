using UnityEngine;
using UnityEngine.AI;

public class FleeState : IState
{
    CowardEnemy enemy;
    float attackCooldown;
    float attackTimer;
    float fleeTimer;
    float fleeInterval = .2f;

    public FleeState(CowardEnemy _enemy)
    {
        enemy = _enemy;
        attackTimer = attackCooldown;
    }

    public void Enter()
    {
        enemy.agent.isStopped = false;
        enemy.agent.speed *= 2;
        fleeTimer = 0f;

        SetNewFleeDestination();
    }

    public void Update()
    {
        attackTimer += Time.deltaTime;
        fleeTimer += Time.deltaTime;

        float distanceToPlayer = Vector3.Distance(enemy.transform.position, gameManager.instance.player.transform.position);

        if (distanceToPlayer >= enemy.fleeRange)
        {
            enemy.stateMachine.ChangeState(new IdleState(enemy));
        }

        if (enemy.CanShoot && attackTimer >= attackCooldown)
        {
            attackTimer = 0f;
            enemy.stateMachine.ChangeState(new AttackState(enemy));
        }

        if (fleeTimer >= fleeInterval)
        {
            SetNewFleeDestination();
            fleeTimer = 0f;

        }
    }

    public void Exit() 
    { 
        enemy.agent.speed /= 2;
        enemy.CanShoot = true;
    }

    void SetNewFleeDestination()
    {
            Vector3 direction = (enemy.transform.position - gameManager.instance.player.transform.position).normalized;
            Vector3 newDestination = enemy.transform.position + direction * enemy.fleeDistance;
            
            if (NavMesh.SamplePosition(newDestination, out NavMeshHit hit, enemy.fleeDistance, NavMesh.AllAreas))
            {
                enemy.agent.SetDestination(hit.position);
            }
            else
            {
                enemy.agent.SetDestination(newDestination);
            }
    }
}
