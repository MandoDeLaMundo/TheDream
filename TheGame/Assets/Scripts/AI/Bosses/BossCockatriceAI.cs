using System.Collections;
using UnityEngine;

public class BossCockatriceAI : BossCoreAI
{
    [Header("Phase 1 Stats")]
    public float attackCooldown;
    public float stompCooldown;
    public float meleeRange;
    public float stompRange;
    public float stompRadius;
    public int meleeDamage;
    public int stompDamage;

    [Header("Petrify Settings")]
    public GameObject petrifyTrigger;
    public float petrifyCooldown;
    public float petrifyDuration;
    [HideInInspector] public float stareTimer;
    [HideInInspector] public bool isPetrifying;

    [Header("Visuals")]
    public GameObject petrifyConeVisual;

    protected override void Start()
    {
        base.Start();

        phase1 = new CockatricePhase1(this);
        phase2 = new CockatricePhase2(this);
        currentPhase = phase1;

        base.Start();
        currentPhase.Enter();
    }

    public void HandlePetrify()
    {
        if (isPetrifying)
            return;

        Transform player = gameManager.instance.player.transform;
        Vector3 toPlayer = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, toPlayer);
        float distance = Vector3.Distance(transform.position, player.position);
    }

    public IEnumerator PetrifyPlayer()
    {
        agent.isStopped = true;

        //anim.SetTrigger("Petrify");

        if (petrifyConeVisual)
            petrifyConeVisual.SetActive(true);

        playerController player = gameManager.instance.player.GetComponent<playerController>();

        if (player)
            player.Stun(petrifyDuration);

        yield return new WaitForSeconds(petrifyDuration);


        if (petrifyConeVisual)
            petrifyConeVisual.SetActive(false);

        agent.isStopped = false;
    }
}
