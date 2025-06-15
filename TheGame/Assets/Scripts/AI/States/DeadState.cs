using UnityEngine;

public class DeadState : IState
{
    private readonly EnemyBase enemy;

    public DeadState(EnemyBase _enemy)
    {
        enemy = _enemy;
    }

    public void Enter()
    {
        // Play death animation
        // Disable movement/colliders
    }

    public void Update()
    {
        // Wait for animation, then destroy/disable object
    }

    public void Exit()
    {
        // Leave empty - dead enemies don't come back
    }
}
