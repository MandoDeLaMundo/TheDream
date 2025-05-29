using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shield : MonoBehaviour
{
    
    void OnTriggerEnter(Collider other)
    {
        if (CompareTag("EnemyProjectile"))
        {
            Destroy(other.gameObject);
        }
        else
        {
            return;
        }
    }
}
