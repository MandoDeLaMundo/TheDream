using System.Collections;
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
        enemy.agent.isStopped = true;
        enemy.isAttacking = false;
        // Play attack animation
    }

    public void Update()
    {
        if (!enemy.CanSeePlayer())
        {
            enemy.agent.isStopped = false;
            enemy.stateMachine.ChangeState(new PatrolState(enemy));
            return;
        }

        float distanceToPlayer = Vector3.Distance(enemy.transform.position, gameManager.instance.player.transform.position);

        FaceTarget();

        switch (enemy.attackType)
        {
            case EnemyBase.AttackType.Melee:
                HandleMelee(distanceToPlayer);
                break;
            case EnemyBase.AttackType.Ranged:
                HandleRanged();
                break;
            case EnemyBase.AttackType.Hybrid:
                if (distanceToPlayer <= enemy.meleeRange)
                    HandleMelee(distanceToPlayer);
                else
                    HandleRanged();
                break;
        }
    }

    public void Exit()
    {
        enemy.agent.isStopped = false;
        enemy.isAttacking = false;
    }

    void HandleMelee(float distanceToPlayer)
    {
        if (distanceToPlayer <= enemy.meleeRange && !enemy.isAttacking)
        {
            // enemy.anim.SetTrigger("Attack");
            enemy.isAttacking = true;
            enemy.Attack();
            enemy.StartCoroutine(ResetAttackCooldown(enemy.meleeRate));
        }
        else
        {
            enemy.stateMachine.ChangeState(new ChaseState(enemy));
        }
    }

    void HandleRanged()
    {
        if (!enemy.isAttacking)
        {
            enemy.isAttacking = true;
            enemy.Shoot();
            enemy.StartCoroutine(ResetAttackCooldown(enemy.shootRate));
        }

        enemy.shootTimer += Time.deltaTime;
    }

    IEnumerator ResetAttackCooldown(float delay)
    {
        yield return new WaitForSeconds(delay);
        enemy.isAttacking = false;
    }

    void FaceTarget()
    {
        Vector3 lookDir = (gameManager.instance.player.transform.position - enemy.transform.position).normalized;
        Quaternion rot = Quaternion.LookRotation(new Vector3(lookDir.x, enemy.transform.position.y, lookDir.z));
        enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation, rot, Time.deltaTime * enemy.faceTargetSpeed);
    }
}
