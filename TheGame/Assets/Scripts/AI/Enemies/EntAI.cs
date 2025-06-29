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

    public bool CanAttack()
    {
        bool entangleReady = entangleTimer >= entangleCooldown;
        bool whipReady = shootTimer >= shootRate;
        bool meleeReady = meleeTimer >= meleeRate;

        return (entangleReady || whipReady || meleeReady);
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
