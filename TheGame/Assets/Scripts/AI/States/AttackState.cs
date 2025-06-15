using UnityEngine;

public class AttackState : IState
{
    private readonly EnemyBase enemy;

    public AttackState(EnemyBase _enemy)
    {
        enemy = _enemy;
    }

    public void Enter()
    {
        // Play attack animation
        // Deal damage after delay?
    }

    public void Update()
    {
        // if player out of range
        //      enemy.stateMachine.changestate(new chasestate(enemy))
    }

    public void Exit()
    {
        // Reset attack cooldown
    }
}
