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
        enemy.meleeTimer = 0f;
        enemy.shootTimer = 0f;
    }

    void HandleMelee(float distanceToPlayer)
    {
        if (distanceToPlayer <= enemy.meleeRange && enemy.meleeTimer >= enemy.meleeRate)
        {
            // enemy.anim.SetTrigger("Attack");
            Attack();
        }
        
        if (distanceToPlayer > enemy.meleeRange + 0.5f)
        {
            enemy.stateMachine.ChangeState(new ChaseState(enemy));
        }

        enemy.meleeTimer += Time.deltaTime;
    }

    void HandleRanged()
    {
        if (enemy.playerInRange && enemy.shootTimer >= enemy.shootRate)
        {
            Shoot();
        }

        enemy.shootTimer += Time.deltaTime;
    }


    void FaceTarget()
    {
        Vector3 lookDir = (gameManager.instance.player.transform.position - enemy.transform.position).normalized;
        Quaternion rot = Quaternion.LookRotation(new Vector3(lookDir.x, enemy.transform.position.y, lookDir.z));
        enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation, rot, Time.deltaTime * enemy.faceTargetSpeed);
    }

    public void Shoot()
    {
        enemy.shootTimer = 0;
        if (enemy.projectile)
        {
            Vector3 playerDir = (gameManager.instance.player.transform.position - enemy.shootPos.position).normalized;
            Object.Instantiate(enemy.projectile, enemy.shootPos.position, Quaternion.LookRotation(playerDir));
        }
    }

    public void Attack()
    {
        enemy.meleeTimer = 0;
        gameManager.instance.player.GetComponent<playerController>().TakeDMG(enemy.meleeDmgAmt);
    }
}
