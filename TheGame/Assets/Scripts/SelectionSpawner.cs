using UnityEngine;
using System.Collections.Generic;

public class SelectionSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> objectsToSpawn = new List<GameObject>();
    [SerializeField] int numToSpawn;
    [SerializeField] int spawnRate;
    [SerializeField] Transform[] spawnPos;
    int objectListPos;

    int spawnCount;
    float spawnTimer;
    bool playerInTrigger;
    bool startSpawner;

    // Update is called once per frame
    void Update()
    {
        if (playerInTrigger)
        {
            if (Input.GetButtonDown("Interact"))
            {
                startSpawner = true;
            }

            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnRate && spawnCount < numToSpawn && startSpawner)
            {
                spawn();
            }
            selectEnemies();
        }
        else
        {
            startSpawner = false;
            spawnCount = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteraction interaction = other.GetComponent<IInteraction>();
        if (interaction != null)
        {
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IInteraction interaction = other.GetComponent<IInteraction>();
        if (interaction != null)
        {
            playerInTrigger = false;
        }
    }

    void selectEnemies()
    {
        if (Input.GetKeyDown("right") && objectListPos < objectsToSpawn.Count - 1)
        {
            objectListPos++;
            //changeEnemies();
        }
        if (Input.GetKeyDown("left") && objectListPos > 0)
        {
            objectListPos--;
            //changeEnemies();
        }
    }

    void changeEnemies()
    {
        //objects = objectsToSpawn[objectListPos];
    }

    void spawn()
    {
        int arrayPos = Random.Range(0, spawnPos.Length);
        Instantiate(objectsToSpawn[objectListPos], spawnPos[arrayPos].position, spawnPos[arrayPos].rotation);
        spawnCount++;
        spawnTimer = 0;
    }
}