using UnityEngine;
using UnityEngine.AI;

public class PatrolState : IState
{
    EnemyBase enemy;
    float roamTimer;
    Vector3 roamPoint;

    public PatrolState(EnemyBase _enemy)
    {
        enemy = _enemy;
    }

    public void Enter()
    {
        Debug.Log("I am in the patrol state");
        roamTimer = 0;
        enemy.agent.stoppingDistance = 0;
        SetNewRoamPoint();
    }

    public void Update()
    {
        if (enemy.CanSeePlayer() && enemy.playerInRange)
        {
            if (enemy is CowardEnemy cowardEnemy)
            {
                enemy.stateMachine.ChangeState(new FleeState(cowardEnemy));
                return;
            }
            enemy.stateMachine.ChangeState(new ChaseState(enemy));
            return;
        }

        if (!enemy.agent.pathPending && enemy.agent.remainingDistance < 0.01f)
        {
            roamTimer += Time.deltaTime;

            if (roamTimer >= enemy.roamPauseTime)
            {
                enemy.stateMachine.ChangeState(new IdleState(enemy));
            }
        }
    }
    public void Exit() 
    {
        roamTimer = 0;
    }

    void SetNewRoamPoint()
    {
        Vector3 randPos = Random.insideUnitSphere * enemy.roamDist;
        randPos.y = 0f;
        randPos += enemy.startingPos;

        if (NavMesh.SamplePosition(randPos, out NavMeshHit hit, enemy.roamDist, 1))
        {
            roamPoint = hit.position;
            if (Vector3.Distance(enemy.transform.position, roamPoint) > 0.5f)
            {
                enemy.agent.SetDestination(roamPoint);
            }
        }
        else
        {
            Debug.LogWarning($"{enemy.name} could not find a valid roam point.");
        }
    }
}
