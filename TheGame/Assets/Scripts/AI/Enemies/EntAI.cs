using System.Collections;
using UnityEngine;

public class EntAI : EnemyBase
{
    [Header("Ent Mini Boss Abilities")]
    public float vineWhipRangeMin;
    public float vineWhipRangeMax;
    public float entangleCooldown;
    public float immobilizeTimer;
    public float warningDuration;

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
        if (vineWhipPrefab)
        {
            Vector3 dir = (gameManager.instance.player.transform.position - shootPos.position).normalized;
            Instantiate(vineWhipPrefab, shootPos.position, Quaternion.LookRotation(dir));
        }
    }

    IEnumerator ActivateEntangle(GameObject obj)
    {
        yield return new WaitForSeconds(warningDuration);
        obj.SetActive(true);
    }
}
