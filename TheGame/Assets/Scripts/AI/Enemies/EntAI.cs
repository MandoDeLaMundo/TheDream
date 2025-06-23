using System.Collections;
using NUnit;
using UnityEngine;

public class EntAI : EnemyBase
{
    [Header("Ent Mini Boss Abilities")]
    public int whipDamage;
    public float vineWhipRangeMin;
    public float vineWhipRangeMax;
    public float entangleCooldown;
    public float immobilizeTimer;
    public float warningDuration;

    public Transform whipPos;

    public GameObject entanglePrefab;
    public GameObject vineWhipPrefab;

    [HideInInspector] public float entangleTimer;
    [HideInInspector] public bool canEntangle;

    protected override void Update()
    {
        base.Update();
        entangleTimer += Time.deltaTime;
    }

    public bool CanEntangle()
    {
        if (entangleTimer >= entangleCooldown)
            return true;

        return false;
    }

    public void ResetEntangleCooldown()
    {
        entangleTimer = 0f;
    }

    public void CastEntangle()
    {
        if (entanglePrefab)
        {
            anim.SetTrigger("Spell");
            ResetEntangleCooldown();
            Vector3 playerPos = (gameManager.instance.player.transform.position);
            playerPos.y = 0;
            GameObject entangle = Instantiate(entanglePrefab, playerPos, Quaternion.identity);

            entangle.SetActive(false);
            StartCoroutine(ActivateEntangle(entangle));
        }

    }

    public void FireVineWhip()
    {
        anim.SetTrigger("Whip");
        GameObject whip = Instantiate(vineWhipPrefab, whipPos.position, whipPos.rotation);
        whip.GetComponent<Whip>().maxLength = vineWhipRangeMax;
        whip.GetComponent<Whip>().damageAmount = whipDamage;
    }

    public bool ShouldUseEntAttack()
    {
        float distance = Vector3.Distance(transform.position, gameManager.instance.player.transform.position);

        bool entangleReady = entangleTimer >= entangleCooldown;
        bool whipReady = shootTimer >= shootRate;
        bool inWhipZone = distance > meleeRange && distance <= vineWhipRangeMax;

        return (entangleReady || whipReady) && inWhipZone;
    }

    public void StartCooldownAndChase()
    {
        StartCoroutine(PauseBeforeChase());
    }

    IEnumerator ActivateEntangle(GameObject obj)
    {
        yield return new WaitForSeconds(warningDuration);
        obj.SetActive(true);
    }

    IEnumerator PauseBeforeChase()
    {
        yield return new WaitForSeconds(1f);
        stateMachine.ChangeState(new ChaseState(this));
    }
}
