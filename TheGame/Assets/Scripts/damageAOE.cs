using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageAOE : MonoBehaviour
{
    [SerializeField] int damageAmount;
    HashSet<IDamage> damagedTick = new HashSet<IDamage>();
    private void OnTriggerStay(Collider other)
    {
        IDamage dmg = other.GetComponent<IDamage>();
        //Debug.Log("AOE dmg trigger");
        if (damagedTick.Contains(dmg))
        {
            return;
        }   
        if (dmg != null)
        {
            dmg.TakeDMG(damageAmount);
            damagedTick.Add(dmg);
        }
        Cooldown();
        Destroy(gameObject, 0.3f);
    }
    IEnumerator Cooldown()
    {
        yield return new WaitForSecondsRealtime(1);
        GetComponent<MeshRenderer>().enabled = false;
    }
}
