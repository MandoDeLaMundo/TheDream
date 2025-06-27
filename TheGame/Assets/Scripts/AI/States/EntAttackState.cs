using System.Collections;
using UnityEngine;

public class EntAttackState : IState
{
    EntAI ent;
    float distance;

    public EntAttackState(EntAI _ent)
    {
        ent = _ent;
    }

    public void Enter()
    {
        ent.agent.isStopped = true;
        ent.isAttacking = false;
    }

    public void Update()
    {
        ent.shootTimer += Time.deltaTime;
        ent.meleeTimer += Time.deltaTime;
        ent.entangleTimer += Time.deltaTime;

        distance = Vector3.Distance(ent.transform.position, gameManager.instance.player.transform.position);
        FaceTarget();

        if (!ent.playerInRange || !ent.CanSeePlayer())
        {
            ent.agent.isStopped = false;
            ent.stateMachine.ChangeState(new ChaseState(ent));
            return;
        }

        bool didAttack = false;

        bool canAttack = ent.CanAttack();

        if (canAttack && !ent.isAttacking)
        {
            if (distance <= ent.meleeRange && ent.meleeTimer >= ent.meleeRate)
            {
                HandleMelee();
                didAttack = true;
                return;
            }

            else if (distance <= ent.vineWhipRangeMax && ent.shootTimer >= ent.shootRate)
            {
                HandleVineWhip();
                didAttack = true;
                return;
            }

            else if (ent.CanEntangle())
            {
                ent.CastEntangle();
                didAttack = true;
                return;
            }

            ent.isAttacking = false;
        }

        if (didAttack)
        {
            ent.StartCooldownAndChase();
        }

        else
        {
            ent.stateMachine.ChangeState(new ChaseState(ent));
        }

    }

    public void Exit()
    {
        ent.agent.isStopped = false;
        ent.isAttacking = true;
    }

    void FaceTarget()
    {
        Vector3 dir = (gameManager.instance.player.transform.position - ent.transform.position).normalized;
        dir.y = 0;
        Quaternion rot = Quaternion.LookRotation(dir);
        ent.transform.rotation = Quaternion.Lerp(ent.transform.rotation, rot, Time.deltaTime * ent.faceTargetSpeed);
    }

    void HandleMelee()
    {
        if (ent.meleeTimer >= ent.meleeRange)
        {
            ent.isAttacking = true;
            // ent.anim.SetTrigger("Slash");
            gameManager.instance.player.GetComponent<playerController>().TakeDMG(ent.meleeDmgAmt);
            ent.meleeTimer = 0f;
        }
    }

    void HandleVineWhip()
    {
        ent.isAttacking = true;
        ent.FireVineWhip();
        ent.shootTimer = 0f;
    }
}
