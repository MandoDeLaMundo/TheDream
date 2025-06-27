using NUnit;
using UnityEngine;

public class BossEntAI : BossCoreAI
{
    //[Header("Phase 1 Stats")]

    [Header("Melee Settings")]
    public int meleeDamage;
    public float meleeRange;
    public float meleeCooldown;
    [HideInInspector] public float meleeTimer;

    [Header("Whip Settings")]
    public float whipMinRange;
    public float whipMaxRange;
    public float whipCooldown;
    public GameObject whipPrefab;
    public Transform whipPos;
    [HideInInspector] public float whipTimer;

    [Header("Entangle Settings")]
    public GameObject entanglePrefab;
    public float entanglePrefabDuration;
    public float entangleMinRange;
    public float entangleMaxRange;
    public float entangleCooldown;
    [HideInInspector] public float entangleTimer;

    protected override void Start()
    {
        phase1 = new EntPhase1(this);

        currentPhase = phase1;

        base.Start();
        currentPhase.Enter();
    }

    public void MeleeAttack()
    {
        anim.SetTrigger("Melee");
        isAttacking = true;
        gameManager.instance.player.GetComponent<playerController>().TakeDMG(meleeDamage);
    }

    public void WhipAttack()
    {
        anim.SetTrigger("Whip");
        isAttacking = true;
        GameObject whip = Instantiate(whipPrefab, whipPos.position, whipPos.rotation);
        whip.GetComponent<Whip>().maxLength = whipMaxRange;
        //whip.GetComponent<Whip>().damageAmount = whipDamage;
    }

    public void EntangleAttack()
    {
        Debug.Log("EntangleAttack called");
        anim.SetTrigger("Spell");
        isAttacking = true;
        entangleTimer = 0f;

        Vector3 playerPos = gameManager.instance.player.transform.position;
        playerPos.y = 0;
        GameObject entangle = Instantiate(entanglePrefab, playerPos, Quaternion.identity);
    }

    public bool CanMelee()
    {
        bool inMeleeRange = Vector3.Distance(gameManager.instance.player.transform.position, transform.position) <= meleeRange;
        return inMeleeRange;
    }

    public bool CanWhip()
    {
        bool inWhipMaxRange = Vector3.Distance(gameManager.instance.player.transform.position, transform.position) <= whipMaxRange;
        bool inWhipMinRange = Vector3.Distance(gameManager.instance.player.transform.position, transform.position) >= whipMinRange;
        return inWhipMaxRange && inWhipMinRange;
    }

    public bool CanEntangle()
    {
        bool inEntangleMaxRange = Vector3.Distance(gameManager.instance.player.transform.position, transform.position) <= entangleMaxRange;
        bool inEntangleMinRange = Vector3.Distance(gameManager.instance.player.transform.position, transform.position) >= entangleMinRange;
        return inEntangleMaxRange && inEntangleMinRange;
    }

    public void EndAttack()
    {
        isAttacking = false;
        anim.ResetTrigger("Melee");
        anim.ResetTrigger("Whip");
        anim.ResetTrigger("Spell");
    }

    public void TryAttack()
    {
        if (isAttacking)
            return;
        if (attackTimer < attackCooldown)
            return;

        attackTimer = 0f;

        if (CanMelee() && meleeTimer >= meleeCooldown)
        {
            MeleeAttack();
            meleeTimer = 0f;
            return;
        }
        else if (CanWhip() && whipTimer >= whipCooldown)
        {
            WhipAttack();
            whipTimer = 0f;
            return;
        }
        else if (CanEntangle() && entangleTimer >= entangleCooldown)
        {
            EntangleAttack();
            entangleTimer = 0f;
            return;
        }
    }
}
