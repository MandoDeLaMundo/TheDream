using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SelectionSpawner : MonoBehaviour
{
    enum spawntype { Everything, Fairy }
    [SerializeField] spawntype type;
    [SerializeField] List<spawnStats> spawnList = new List<spawnStats>();
    [SerializeField] GameObject Fairy;
    GameObject cloneFairy;
    GameObject spawnObject;
    [SerializeField] int numToSpawn;
    [SerializeField] int spawnRate;
    [SerializeField] Transform[] spawnPos;
    int objectListPos = 0;

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
        if (spawnList != null && type == spawntype.Everything)
        {
            spawnObject = spawnList[objectListPos].pickup;

            if (spawnList[objectListPos].sprite != null)
                image.sprite = spawnList[objectListPos].sprite;
        }

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
            if (Input.GetButtonDown("Interact") && type == spawntype.Everything)
            {
                startSpawner = true;
            }

            if (spawnCount < numToSpawn && type == spawntype.Fairy)
            {
                spawn();
                //gameManager.instance.DisplayDialogue();
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
                if (type == spawntype.Fairy)
                {
                    Destroy(cloneFairy);
                    gameManager.instance.HideDialogue();
                }
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
            if (type == spawntype.Everything)
                Canvas.SetActive(true);

            playerInTrigger = true;
            playerController.instance.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IInteraction interaction = other.GetComponent<IInteraction>();
        if (interaction != null)
        {
            if (type == spawntype.Everything)
                Canvas.SetActive(false);

            playerInTrigger = false;
        }
    }

    void selectEverything()
    {
        if (type == spawntype.Everything)
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
    }

    void changeEverything()
    {
        spawnObject = spawnList[objectListPos].pickup;

        if (spawnList[objectListPos].sprite != null)
            image.sprite = spawnList[objectListPos].sprite;
    }

    void spawn()
    {
        int arrayPos = Random.Range(0, spawnPos.Length);
        if (type == spawntype.Everything)
        {
            Instantiate(spawnObject, spawnPos[arrayPos].position, spawnPos[arrayPos].rotation);
        }
        if (type == spawntype.Fairy)
        {
            cloneFairy = Instantiate(Fairy, spawnPos[arrayPos].position, spawnPos[arrayPos].rotation);
        }

        spawnCount++;
        spawnTimer = 0;
    }
}