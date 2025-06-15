using JetBrains.Annotations;
using UnityEngine;

public class IdleState : IState
{
    private readonly EnemyBase enemy;
    public float idleTime;
    public float timer;

    public IdleState(EnemyBase _enemy, float _idleDuration)
    {
        enemy = _enemy;
        idleTime = _idleDuration;
    }

    public void Enter()
    {
        timer = 0f;
        // mEnemy.Animator.Play("Idle");
        enemy.agent.isStopped = true;
        Debug.Log($"{enemy.name} entered Idle State.");
    }

    public void Update()
    {
        timer += Time.deltaTime;

        if (timer >= idleTime)
        {
            enemy.StateMachine.ChangeState(new PatrolState(enemy));
        }
    }

    public void Exit()
    {
        Debug.Log($"{enemy.name} exiting Idle State.");
    }
}
