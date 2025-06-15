using System.Collections;
using UnityEngine;

public class DeadState : IState
{
    EnemyBase enemy;
    float destroyDelay = 2f;

    public DeadState(EnemyBase _enemy)
    {
        enemy = _enemy;
    }

    public void Enter()
    {
        if (enemy.agent) enemy.agent.isStopped = true;
        // if (enemy.anim) enemy.anim.SetTrigger("Die");

        DropItem();

        //enemy.StartCoroutine(DelayedDestroy());

    }

    public void Update()
    {
        // Wait for animation, then destroy/disable object
    }

    public void Exit()
    {
        // Leave empty - dead enemies don't come back
    }

    void DropItem()
    {
        if (enemy.dropItemPrefab)
        {
            Object.Instantiate(enemy.dropItemPrefab, enemy.lootPos.position, Quaternion.identity);
        }
    }

    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(destroyDelay);

        Object.Destroy(enemy.dropItemPrefab);
    }
}
