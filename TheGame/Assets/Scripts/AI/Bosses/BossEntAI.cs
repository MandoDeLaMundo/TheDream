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
        isAttacking = true;
        anim.SetTrigger("Melee");
        gameManager.instance.player.GetComponent<playerController>().TakeDMG(meleeDamage);
        isAttacking = false;
    }

    public void WhipAttack()
    {
        anim.SetTrigger("Whip");
        GameObject whip = Instantiate(whipPrefab, whipPos.position, whipPos.rotation);
        whip.GetComponent<Whip>().maxLength = whipMaxRange;
        //whip.GetComponent<Whip>().damageAmount = whipDamage;
    }

    public void EntangleAttack()
    {
        isAttacking = true;
        anim.SetTrigger("Spell");
        entangleTimer = 0f;
        Vector3 playerPos = (gameManager.instance.player.transform.position);
        playerPos.y = 0;
        GameObject entangle = Instantiate(entanglePrefab, playerPos, Quaternion.identity);

        entangle.SetActive(false);
        //StartCoroutine(ActivateEntangle(entangle));
    }

    public bool CanMelee()
    {
        bool inMeleeRange = Vector3.Distance(gameManager.instance.player.transform.position, transform.position) <= meleeRange;
        return inMeleeRange;
    }

    public bool CanWhip()
    {
        bool inWhipMaxRange = Vector3.Distance(gameManager.instance.player.transform.position, transform.position) <= whipMaxRange;
        bool inWhipMinRange = Vector3.Distance(gameManager.instance.player.transform.position, transform.position) <= whipMinRange;
        return inWhipMaxRange && inWhipMinRange;
    }

    public bool CanEntangle()
    {
        bool inEntangleMaxRange = Vector3.Distance(gameManager.instance.player.transform.position, transform.position) <= entangleMaxRange;
        bool inEntangleMinRange = Vector3.Distance(gameManager.instance.player.transform.position, transform.position) <= entangleMinRange;
        return inEntangleMaxRange && inEntangleMinRange;
    }
}
