using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D;
using UnityEngine.UI;

public class SelectionSpawner : MonoBehaviour
{
    [SerializeField] List<spawnStats> spawnList = new List<spawnStats>();
    GameObject spawnObject;
    [SerializeField] int numToSpawn;
    [SerializeField] int spawnRate;
    [SerializeField] Transform[] spawnPos;
    int objectListPos;

    [SerializeField] GameObject Canvas;
    [SerializeField] Image image;

    [SerializeField] GameObject leftButtonFilled;
    [SerializeField] GameObject leftButtonHole;
    [SerializeField] GameObject rightButtonFilled;
    [SerializeField] GameObject rightButtonHole;

    int spawnCount;
    float spawnTimer;
    bool playerInTrigger;
    bool startSpawner;

    [SerializeField] float buttonTime;
    float buttonTimer;

    void Start()
    {
        image.sprite = spawnList[objectListPos].sprite;
        spawnObject = spawnList[objectListPos].pickup;

        leftButtonFilled.SetActive(true);
        leftButtonHole.SetActive(false);
        rightButtonFilled.SetActive(true);
        rightButtonHole.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        buttonTimer += Time.deltaTime;
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
            selectEverything();

            if (Input.GetKeyDown("q"))
            {
                playerController.instance.enabled = true;
            }

            if (buttonTimer >= buttonTime && leftButtonFilled.activeSelf == false || buttonTimer >= buttonTime && rightButtonFilled.activeSelf == false)
            {
                leftButtonFilled.SetActive(true);
                leftButtonHole.SetActive(false);
                rightButtonFilled.SetActive(true);
                rightButtonHole.SetActive(false);
                buttonTimer = 0;
            }
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
            Canvas.SetActive(true);
            playerController.instance.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IInteraction interaction = other.GetComponent<IInteraction>();
        if (interaction != null)
        {
            playerInTrigger = false;
            Canvas.SetActive(false);
        }
    }

    void selectEverything()
    {
        if (Input.GetKeyDown("right") && objectListPos < spawnList.Count - 1)
        {
            rightButtonFilled.SetActive(false);
            rightButtonHole.SetActive(true);
            objectListPos++;
            changeEverything();
        }
        if (Input.GetKeyDown("left") && objectListPos > 0)
        {
            leftButtonFilled.SetActive(false);
            leftButtonHole.SetActive(true);
            objectListPos--;
            changeEverything();
        }
    }

    void changeEverything()
    {
        image.sprite = spawnList[objectListPos].sprite;
        spawnObject = spawnList[objectListPos].pickup;
    }

    void spawn()
    {
        int arrayPos = Random.Range(0, spawnPos.Length);
        Instantiate(spawnObject, spawnPos[arrayPos].position, spawnPos[arrayPos].rotation);
        spawnCount++;
        spawnTimer = 0;
    }
}