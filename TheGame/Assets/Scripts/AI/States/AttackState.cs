using System.Collections;
using UnityEngine;

public class AttackState : IState
{
    EnemyBase enemy;

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
        if (!enemy.CanSeePlayer() || !enemy.playerInRange)
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
        enemy.meleeTimer = 0f;
        enemy.shootTimer = 0f;
    }

    void HandleMelee(float distanceToPlayer)
    {
        if (distanceToPlayer > enemy.meleeRange + 0.5f)
        {
            enemy.agent.isStopped = false;
            enemy.stateMachine.ChangeState(new ChaseState(enemy));
            return;
        }
        
        if (enemy.meleeTimer >= enemy.meleeRate)
        {
            enemy.meleeTimer = 0f;
            gameManager.instance.player.GetComponent<playerController>()?.TakeDMG(enemy.meleeDmgAmt);
            // TODO: enemy.anim.SetTrigger("MeleeAttack");
        }

        enemy.meleeTimer += Time.deltaTime;
    }

    void HandleRanged()
    {
        if (enemy.playerInRange && enemy.shootTimer >= enemy.shootRate)
        {
            enemy.shootTimer = 0f;
            Vector3 playerDir = (gameManager.instance.player.transform.position - enemy.shootPos.position).normalized;
            Object.Instantiate(enemy.projectile, enemy.shootPos.position, Quaternion.LookRotation(playerDir));
            // TODO: enemy.anim.SetTrigger("Shoot");
        }

        enemy.shootTimer += Time.deltaTime;
    }


    void FaceTarget()
    {
        Vector3 lookDir = (gameManager.instance.player.transform.position - enemy.transform.position).normalized;
        lookDir.y = 0;

        Quaternion rot = Quaternion.LookRotation(lookDir);
        enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation, rot, Time.deltaTime * enemy.faceTargetSpeed);
    }
}
