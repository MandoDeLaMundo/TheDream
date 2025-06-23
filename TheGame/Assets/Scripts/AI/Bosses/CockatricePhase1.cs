using System.Collections;
using UnityEngine;

public class CockatricePhase1 : BossPhaseBase
{
    public CockatricePhase1(BossCoreAI boss) : base(boss) { }

    public override void Enter()
    {
        boss.meleeTimer = ((BossCockatriceAI)boss).meleeCooldown;
    }
    public override void Update()
    {
        if (!boss.CanSeePlayer())
            return;
        
        boss.FacePlayer();

        var cockatrice = (BossCockatriceAI)boss;

        if (!cockatrice.isPetrifying)
        {
            boss.anim.SetBool("isRunning", true);
            boss.FacePlayer();
            cockatrice.agent.SetDestination(gameManager.instance.player.transform.position);
        }
        else
        {
            cockatrice.agent.ResetPath();
            boss.anim.SetBool("isRunning", false);
        }

        cockatrice.stompTimer += Time.deltaTime;
        boss.meleeTimer += Time.deltaTime;
        cockatrice.stareCooldownTimer += Time.deltaTime;

        float distance = Vector3.Distance(boss.transform.position, gameManager.instance.player.transform.position);

        if (!cockatrice.isPetrifying && cockatrice.stareCooldownTimer >= cockatrice.petrifyStareCooldown && !cockatrice.isAttacking)
        {
            cockatrice.stareCooldownTimer = 0f;
            cockatrice.StartCoroutine(cockatrice.PetrifyPlayer());
            return;
        }

        if (cockatrice.stompTimer >= cockatrice.stompCooldown && distance <= cockatrice.stompRange && !cockatrice.isPetrifying)
        {
            cockatrice.StompAttack();
            cockatrice.stompTimer = 0f;
            return;
        }

        if (boss.meleeTimer >= cockatrice.meleeCooldown && distance <= cockatrice.meleeRange && !cockatrice.isPetrifying)
        {
            cockatrice.MeleeAttack();
            boss.meleeTimer = 0f;
            return;
        }
    }

    public override void Exit()
    {
        Debug.Log("Cockatrice Phase 1 ends!");

    }
}
