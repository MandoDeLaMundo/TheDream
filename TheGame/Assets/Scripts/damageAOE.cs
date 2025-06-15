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
        GetComponent<MeshRenderer>().enabled = false;
        Destroy(gameObject, 2);
    }
}
