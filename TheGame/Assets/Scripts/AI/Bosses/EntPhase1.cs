using UnityEngine;

public class EntPhase1 : BossPhaseBase
{
    public EntPhase1(BossCoreAI boss) : base(boss) { }


    public override void Enter()
    {
        boss.attackTimer = ((BossEntAI) boss).attackCooldown;
    }

    public override void Update()
    {
        if (!boss.CanSeePlayer())
            return;

        boss.FacePlayer();

        var ent = (BossEntAI) boss;

        if (!ent.isAttacking)
        {
            boss.anim.SetBool("isRunning", true);
            boss.FacePlayer();
            ent.agent.SetDestination(gameManager.instance.player.transform.position);
        }
        else
        {
            ent.agent.ResetPath();
            boss.anim.SetBool("isRunning", false);
        }

        boss.attackTimer += Time.deltaTime;
        ent.meleeTimer += Time.deltaTime;
        ent.whipTimer += Time.deltaTime;
        ent.entangleTimer += Time.deltaTime;

        if (!boss.isAttacking && boss.attackTimer >= boss.attackCooldown)
        {
            boss.isAttacking = true;
            boss.agent.isStopped = true;

            if (ent.CanMelee() && ent.meleeTimer >= ent.meleeCooldown)
            {
                ent.MeleeAttack();
                ent.meleeTimer = 0f;
                return;
            }
            else if (ent.CanWhip() && ent.whipTimer >= ent.whipCooldown)
            {
                ent.WhipAttack();
                ent.whipTimer = 0f;
                return;
            }
            else if (ent.CanEntangle() && ent.entangleTimer >= ent.entangleCooldown)
            {
                ent.EntangleAttack();
                ent.entangleTimer = 0f;
                return;
            }
        }

        boss.agent.isStopped = false;
        boss.isAttacking = false;
    }

    public override void Exit()
    {

    }
}
