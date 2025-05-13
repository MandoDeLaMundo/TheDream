using UnityEngine;
using System.Collections;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [SerializeField] int HP;
    [SerializeField] int faceTargetSpeed;

    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    Color colorOrig;

    Vector3 playerDir;

    float shootTimer;

    bool playerInRange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;
        // Gamemanager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {


        if (playerInRange)
        {
            //  playerDir = (Gamemanager.instance.player.transform.position - transform.position);

            //  agent.SetDestination(Gamemanager.instance.player.transform.position);


            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                faceTarget();
            }
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;

        // agent.SetDestination(Gamemanager.instance.player.transform.position);

        if (HP <= 0)
        {
            //     Gamemanager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(flashRed());
        }
    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        model.material.color = colorOrig;
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, transform.position.y, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }
}
