using System.Collections;
using UnityEngine;

public class BossCockatriceAI : BossCoreAI
{
    [Header("Phase 1 Stats")]
    //public float attackCooldown;
    public float stompCooldown;
    public float meleeRange;
    public float stompRange;
    public float stompRadius;
    public int meleeDamage;
    public int stompDamage;
    [HideInInspector] public float stompTimer;

    [Header("Phase 2 Stats")]
    public float phase2AttackCooldown;
    public float phase2StompCooldown;

    [Header("Petrify Settings")]
    public GameObject petrifyTrigger;
    public float petrifyCooldown;
    public float petrifyDuration;
    public float stareDuration;
    public float stunThreshold;
    [HideInInspector] public float stareTimer;
    [HideInInspector] public float stareCooldownTimer;
    [HideInInspector] public bool isPetrifying;
    [HideInInspector] public float faceTargetSpeedOrig;

    [Header("Visuals")]
    public GameObject petrifyConeVisual;

    protected override void Start()
    {
        phase1 = new CockatricePhase1(this);
        phase2 = new CockatricePhase2(this);
        currentPhase = phase1;

        if (petrifyTrigger)
            petrifyTrigger.SetActive(false);

        if (petrifyConeVisual)
            petrifyConeVisual.SetActive(false);

        faceTargetSpeedOrig = faceTargetSpeed;

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

    public void StompAttack()
    {
        isAttacking = true;
        anim.SetTrigger("Stomp");

        float distance = Vector3.Distance(transform.position, gameManager.instance.player.transform.position);
        if (distance <= stompRadius)
        {
            gameManager.instance.player.GetComponent<playerController>().TakeDMG(stompDamage);
        }

        isAttacking = false;
    }

    public void TriggerPetrify()
    {
        if (!isPetrifying)
        {
            StartCoroutine(PetrifyPlayer());
        }
    }

    public IEnumerator PetrifyPlayer()
    {
        isPetrifying = true;
        agent.isStopped = false;
        anim.SetTrigger("Petrify");

        Vector3 toPlayer = gameManager.instance.player.transform.position - transform.position;
        Vector3 retreatDir = -toPlayer.normalized;
        Vector3 retreatTarget = transform.position + retreatDir * 5f;

        agent.SetDestination(retreatTarget);

        float retreatTime = 0f;
        while (retreatTime < 1f && Vector3.Distance(transform.position, retreatTarget) > 0.5f)
        {
            retreatTime += Time.deltaTime;
            yield return null;
        }

        agent.isStopped = true;
        anim.SetBool("isRunning", false);
        faceTargetSpeed *= 0.25f;

        if (petrifyTrigger)
            petrifyTrigger.SetActive(true);
        if (petrifyConeVisual)
            petrifyConeVisual.SetActive(true);

        float stareTimer = 0f;
        float inConeTimer = 0f;

        while (stareTimer < stareDuration)
        {
            stareTimer += Time.deltaTime;
            if (petrifyTrigger.GetComponent<PetrifyTrigger>().IsPlayerInZone)
            {
                inConeTimer += Time.deltaTime;

                if (inConeTimer >= stunThreshold)
                {
                    playerController player = gameManager.instance.player.GetComponent<playerController>();

                    if (player)
                        player.Stun(petrifyDuration);

                    break;
                }
            }

            stareTimer += Time.deltaTime;
            yield return null;
        }


        if (petrifyTrigger)
            petrifyTrigger.SetActive(false);
        if (petrifyConeVisual)
            petrifyConeVisual.SetActive(false);

        agent.isStopped = false;
        faceTargetSpeed = faceTargetSpeedOrig;
        isPetrifying = false;
    }

    //private void LateUpdate()
    //{
    //    if (petrifyConeVisual && headPos)
    //    {
    //        Vector3 targetForward = headPos.forward;

    //        Quaternion targetRotation = Quaternion.LookRotation(targetForward, Vector3.up);

    //        petrifyConeVisual.transform.localRotation = Quaternion.Lerp(petrifyConeVisual.transform.localRotation, targetRotation, Time.deltaTime * 5f);
    //    }
    //}
}
