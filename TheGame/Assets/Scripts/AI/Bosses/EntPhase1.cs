using UnityEngine;
using UnityEngine.AI;

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

        boss.attackTimer += Time.deltaTime;
        ent.meleeTimer += Time.deltaTime;
        ent.whipTimer += Time.deltaTime;
        ent.entangleTimer += Time.deltaTime;

        if (ent.isAttacking)
        {
            ent.agent.isStopped = true;
            ent.agent.ResetPath();
            boss.anim.SetBool("isRunning", false);
            return;
        }

        Vector3 playerPos = gameManager.instance.player.transform.position;
        ent.agent.SetDestination(playerPos);

        //Debug.Log($"SetDestination success: {result}, remaining: {ent.agent.remainingDistance}, stopped: {ent.agent.isStopped}");

        if (!ent.agent.pathPending)
        {
            float distance = ent.agent.remainingDistance;

            if (distance >= ent.agent.stoppingDistance)
            {
                ent.agent.isStopped = false;
                boss.anim.SetBool("isRunning", true);
            }
            else
            {
                boss.anim.SetBool("isRunning", false);
                ent.agent.isStopped = true;
                ent.TryAttack();
            }
        }
    }

    public override void Exit()
    {

    }
}
