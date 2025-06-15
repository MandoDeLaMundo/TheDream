using UnityEngine;

public class PatrolState : IState
{
    private readonly EnemyBase enemy;

    Vector3 destination;

    bool playerInRange;

    public PatrolState(EnemyBase _enemy)
    {
        enemy = _enemy;
    }

    public void Enter()
    {
        Debug.Log($"{enemy.name} entering Patrol State.");

        // Set random target or waypoint

        //enemy.agent.SetDestination( destination );
        //enemy.animator.Play("Walk");
    }
    public void Update()
    {
        // Move toward target
        if (enemy.agent.remainingDistance < 0.0f)
            enemy.StateMachine.ChangeState(new IdleState(enemy, enemy.IdleTime));
        // if player is seen:
        // enemy.stateMachine.ChangeState(new ChaseState(enemy))
    }
    public void Exit() 
    {
        Debug.Log($"{enemy.name} exiting Patrol State.");
    }
}
