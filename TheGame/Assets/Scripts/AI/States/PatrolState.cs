using UnityEngine;
using UnityEngine.AI;

public class PatrolState : IState
{
    EnemyBase enemy;
    float roamTimer;
    private Vector3 roamPoint;

    public PatrolState(EnemyBase _enemy)
    {
        enemy = _enemy;
    }

    public void Enter()
    {
        SetNewRoamPoint();
    }
    public void Update()
    {
        if (enemy.CanSeePlayer())
        {
            enemy.stateMachine.ChangeState(new ChaseState(enemy));
            return;
        }

        if (enemy.agent.remainingDistance < 0.1f)
        {
            roamTimer += Time.deltaTime;
            if (roamTimer >= enemy.roamPauseTime)
            {
                SetNewRoamPoint() ;
                roamTimer = 0f;
            }
        }
    }
    public void Exit() 
    {

    }

    void SetNewRoamPoint()
    {
        Vector3 randPos = Random.insideUnitSphere * enemy.roamDist + enemy.startingPos;
        NavMesh.SamplePosition(randPos, out NavMeshHit hit, enemy.roamDist, NavMesh.AllAreas);
        roamPoint = hit.position;
        enemy.agent.SetDestination(roamPoint);
    }
}
