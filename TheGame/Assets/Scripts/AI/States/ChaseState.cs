using UnityEngine;

public class ChaseState : IState
{
    private readonly EnemyBase enemy;
    
    public ChaseState(EnemyBase _enemy)
    {
        enemy = _enemy;
    }

    public void Enter()
    {
        // Play chase animation
        // Set target
    }

    public void Update()
    {
        // Move toward player
        // if distance to player < attack range
        //   enemy.stateMachine.ChangeState(new AttackState(enemy))
    }

    public void Exit()
    {
        // Stop movement if needed
    }
}
