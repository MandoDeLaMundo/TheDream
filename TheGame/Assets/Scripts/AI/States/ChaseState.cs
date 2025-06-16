using UnityEngine;

public class ChaseState : IState
{
    EnemyBase enemy;
    float losePlayerTimer = 0f;
    const float losePlayerDelay = 0.5f;
    
    public ChaseState(EnemyBase _enemy)
    {
        enemy = _enemy;
    }

    public void Enter()
    {
        losePlayerTimer = 0f;
        enemy.agent.stoppingDistance = enemy.stoppingDistOrig;
        enemy.agent.isStopped = false;
    }

    public void Update()
    {
        if (!enemy.CanSeePlayer() || !enemy.playerInRange)
        {
            losePlayerTimer += Time.deltaTime;
            if (losePlayerTimer >= losePlayerDelay)
            {
                enemy.stateMachine.ChangeState(new PatrolState(enemy));
                return;
            }
        }
        else
        {
            losePlayerTimer = 0f;
        }

        Vector3 playerPos = gameManager.instance.player.transform.position;
        if (Vector3.Distance(enemy.agent.destination, playerPos) > enemy.meleeRange)
        {
            if (enemy.agent.destination != playerPos)
                enemy.agent.SetDestination(playerPos);

            enemy.agent.isStopped = false;
        }

        float distanceToPlayer = Vector3.Distance(enemy.transform.position, playerPos);

            // The following is shorthand versions of bool switch case conditions. It's called a C# expression-based switch
        bool shouldAttack = enemy.attackType switch
        {
            EnemyBase.AttackType.Melee => distanceToPlayer <= enemy.meleeRange,
            EnemyBase.AttackType.Ranged => enemy.playerInRange,
            EnemyBase.AttackType.Hybrid => distanceToPlayer <= enemy.meleeRange || enemy.playerInRange,
            _ => false
        };

        if (shouldAttack)
        {
            enemy.stateMachine.ChangeState(new AttackState(enemy));
        }
    }

    public void Exit()
    {

    }
}
