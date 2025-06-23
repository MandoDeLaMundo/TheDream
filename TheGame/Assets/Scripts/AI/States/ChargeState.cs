using System.Collections;
using UnityEngine;

public class ChargeState : IState
{
    ChargingEnemy enemy;
    Vector3 chargeDir;
    float timer;

    public ChargeState(ChargingEnemy _enemy)
    {
        enemy = _enemy;
    }

    public void Enter()
    {
        enemy.agent.isStopped = true;
        enemy.isCharging = true;
        enemy.canCharge = false;
        enemy.StartCharge();
        enemy.chargeCollider.enabled = true;

        chargeDir = (gameManager.instance.player.transform.position - enemy.transform.position).normalized;
        timer = 0f;

        enemy.anim.SetTrigger("Charge");
    }

    public void Update()
    {
        timer += Time.deltaTime;

        enemy.transform.forward = chargeDir;
        enemy.transform.position += chargeDir * enemy.chargeSpeed * Time.deltaTime;

        if (enemy.hitPlayerDuringCharge)
        {
            enemy.hitPlayerDuringCharge = false;
            enemy.StopCharge();
            enemy.stateMachine.ChangeState(new ChaseState(enemy));
            enemy.StartCoroutine(ChargeCooldown());
            return;
        }

        if (timer >= enemy.chargeDuration)
        {
            enemy.StopCharge();
            enemy.isCharging = false;
            enemy.stateMachine.ChangeState(new ChaseState(enemy));
            enemy.StartCoroutine(ChargeCooldown());
        }
    }

    public void Exit()
    {
        enemy.isCharging = false;
        enemy.agent.isStopped = false;
        enemy.StopCharge();
        enemy.agent.velocity = Vector3.zero;
        enemy.chargeCollider.enabled = false;
    }

    IEnumerator ChargeCooldown()
    {
        yield return new WaitForSeconds(enemy.chargeCooldown);
        enemy.canCharge = true;
    }
}
