using UnityEngine;

public class CockatricePhase1 : BossPhaseBase
{
    float stompTimer = 0f;
    float petrifyTimer = 0f;

    public CockatricePhase1(BossCoreAI boss) : base(boss) { }

    public override void Enter()
    {
        boss.attackTimer = ((BossCockatriceAI)boss).attackCooldown;
    }
    public override void Update()
    {
        if (!boss.CanSeePlayer())
            return;

        boss.FacePlayer();

        stompTimer += Time.deltaTime;
        boss.attackTimer += Time.deltaTime;
        petrifyTimer += Time.deltaTime;

        var cockatrice = (BossCockatriceAI)boss;

        float distance = Vector3.Distance(boss.transform.position, gameManager.instance.player.transform.position);

        if (stompTimer >= cockatrice.stompCooldown && distance <= cockatrice.stompRange)
        {
            StompAttack(cockatrice);
            stompTimer = 0f;
            return;
        }

        if (boss.attackTimer >= cockatrice.attackCooldown && distance <= cockatrice.meleeRange)
        {
            MeleeAttack(cockatrice);
            boss.attackTimer = 0f;
            return;
        }

        //if (petrifyTimer >= cockatrice.petrifyCooldown)
        //{
        //    cockatrice.StartCorou
        //}
    }

    public override void Exit()
    {
        Debug.Log("Cockatrice Phase 1 ends!");

    }

    void MeleeAttack(BossCockatriceAI cockatrice)
    {
        // boss.anim.SetTrigger("Attack");
        gameManager.instance.player.GetComponent<playerController>().TakeDMG(cockatrice.meleeDamage);
    }

    void StompAttack(BossCockatriceAI cockatrice)
    {
        Debug.Log("Stomp!");
        float distance = Vector3.Distance(boss.transform.position, gameManager.instance.player.transform.position);
        if (distance <= cockatrice.stompRadius)
        {
            gameManager.instance.player.GetComponent<playerController>().TakeDMG(cockatrice.stompDamage);
        }
    }
}
