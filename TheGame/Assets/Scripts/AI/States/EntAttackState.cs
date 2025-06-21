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
        distance = Vector3.Distance(ent.transform.position, gameManager.instance.player.transform.position);
        FaceTarget();

        if (!ent.playerInRange || !ent.CanSeePlayer())
        {
            ent.agent.isStopped = false;
            ent.stateMachine.ChangeState(new ChaseState(ent));
            return;
        }

        if (distance <= ent.meleeRange)
        {
            HandleMelee();
        }

        else if (distance <= ent.vineWhipRangeMax && distance >= ent.vineWhipRangeMin)
        {
            HandleVineWhip();
        }

        if (ent.CanEntangle() && distance > ent.vineWhipRangeMin)
        {
            ent.CastEntangle();
        }
        
        ent.shootTimer += Time.deltaTime;
        ent.meleeTimer += Time.deltaTime;
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
            // ent.anim.SetTrigger("Slash");
            gameManager.instance.player.GetComponent<playerController>().TakeDMG(ent.meleeDmgAmt);
            ent.meleeTimer = 0f;
        }
    }

    void HandleVineWhip()
    {
        if (ent.shootTimer >= ent.shootRate)
        {
            ent.FireVineWhip();
            ent.shootTimer = 0f;
        }
    }
}
