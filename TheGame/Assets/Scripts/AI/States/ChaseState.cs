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
        if (Vector3.Distance(enemy.agent.destination, playerPos) > 1f)
        {
            enemy.agent.SetDestination(playerPos);
        }

        if (enemy.agent.remainingDistance <= enemy.meleeRange)
        {
            enemy.stateMachine.ChangeState(new AttackState(enemy));
        }
    }

    public void Exit()
    {

    }
}
