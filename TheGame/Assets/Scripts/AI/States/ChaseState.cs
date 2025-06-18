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

        bool shouldAttack = false;

        switch (enemy.attackType)
        {
            case EnemyBase.AttackType.Melee:
                if (distanceToPlayer <= enemy.meleeRange)
                    shouldAttack = true;
                break;
            case EnemyBase.AttackType.Ranged:
                if (enemy.playerInRange)
                    shouldAttack = true;
                break;
            case EnemyBase.AttackType.Hybrid:
                if (enemy.playerInRange && enemy.CanShoot)
                    shouldAttack = true;
                else if (distanceToPlayer <= enemy.meleeRange)
                    shouldAttack = true;
                break;
        };

        if (enemy is ChargingEnemy chargingEnemy)
        {
            if (chargingEnemy.canCharge && Vector3.Distance(enemy.transform.position, gameManager.instance.player.transform.position) <= chargingEnemy.chargeRange)
            {
                enemy.stateMachine.ChangeState(new ChargeState(chargingEnemy));
                return;
            }
        }

        if (shouldAttack)
        {
            enemy.stateMachine.ChangeState(new AttackState(enemy));
        }
    }

    public void Exit()
    {

    }
}
