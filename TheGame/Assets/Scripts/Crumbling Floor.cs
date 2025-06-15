using UnityEngine;
using System.Collections;

public class crumblingFloor : MonoBehaviour
{
    
    [SerializeField] Material[] stages = new Material[3];

    [SerializeField] float stageInterval;
    [SerializeField] float respawnDelay;

    [SerializeField] Renderer rend;             
    [SerializeField] Collider col;

    bool isCrumbling;

    void Reset()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isCrumbling && other.CompareTag("Player"))
            StartCoroutine(CrumbleRoutine());
    }

    IEnumerator CrumbleRoutine()
    {
        isCrumbling = true;

        for (int i = 0; i < stages.Length; i++)
        {
            rend.material = stages[i];
            yield return new WaitForSeconds(stageInterval);
        }

        rend.enabled = false;
        col.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

      
        rend.enabled = true;
        col.enabled = true;
        rend.material = stages[0];   
        isCrumbling = false;
    }
}