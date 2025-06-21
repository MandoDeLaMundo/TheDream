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
    [SerializeField] Transform positionOG;
    [Header("Position matter")]
    [SerializeField] List<GameObject> objects = new List<GameObject>();

    [Header("Health")]
    [SerializeField] int health;

    [Header("RNG Sum 100%")]
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

    private void DropItem(int number)
    {
        if (number < objects.Count && objects[number] != null)
        {
            Instantiate(objects[number], positionOG.position, Quaternion.identity);
        }
    }

    private void ItemsChance()
    {
        int range = Random.Range(1, 101);
        int randomNumber = 0;

        if(range <= (randomNumber += rareDropChance))
        {
            DropItem(0);
        }
        else if (range <= (randomNumber += lowDropChance))
        {
            int half = Random.value < 0.5f ? 1 : 2;
            DropItem(half);
        }
        else if (range <= (randomNumber += midDropChance))
        {
            int half = Random.value < 0.5f ? 3 : 4;
            DropItem(half);
        }
        else if (range <= (randomNumber += noDropChance))
        {

        }
    }

    private void TriggerDrop()
    {
        if (VFX != null)
        {
            Instantiate(VFX, transform.position, Quaternion.identity);
        }
        ItemsChance();
        VFX.Play();
        StartCoroutine(DestroyDelay());
    }

    void IDamage.TakeDMG(int amount)
    {
        health -= amount;
        if (type == objectType.crate && health <= 0)
        {
            TriggerDrop();
        }
    }
    IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
