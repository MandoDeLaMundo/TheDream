using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestroyObject : MonoBehaviour, IDamage
{
    enum objectType { none, crate }

    [Header("Asset")]
    [SerializeField] objectType type;
    [SerializeField] GameObject propObject;
    [SerializeField] ParticleSystem VFX;
    [SerializeField] Transform position;
    [SerializeField] List<GameObject> objects = new List<GameObject>();

    [Header("Health")]
    [SerializeField] int health;

    [Header("RNG")]
    [SerializeField] int noDropChance;
    [SerializeField] int midDropChance;
    [SerializeField] int lowDropChance;
    [SerializeField] int rareDropChance;

    int healthOG;
    void Start()
    {
        healthOG = health;
    }
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if ( type == objectType.crate)
        {
            if (health <= 0)
            {
                Destroy(gameObject);

            }
        }
    }

    void IDamage.TakeDMG(int amount)
    {
        health -= amount;
    }
}
