using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shield : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}
