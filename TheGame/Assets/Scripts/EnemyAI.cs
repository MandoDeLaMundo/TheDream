using UnityEngine;
using System.Collections;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [SerializeField] int HP;
    [SerializeField] int faceTargetSpeed;

    [SerializeField] Transform shootPos;
    [SerializeField] GameObject projectile;
    [SerializeField] float shootRate;
    [SerializeField] float meleeRate;
    [SerializeField] int meleeDamage;
    [SerializeField] float meleeRange;

    Color colorOrig;

    Vector3 playerDir;

    float shootTimer;
    float meleeTimer;
    bool playerInRange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;
         gameManager.instance.UpdateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {


        if (!playerInRange) return;
        {
              playerDir = (gameManager.instance.player.transform.position - transform.position).normalized;

              agent.SetDestination(gameManager.instance.player.transform.position);
                 faceTarget();


                float distToPlayer = Vector3.Distance(transform.position, gameManager.instance.player.transform.position);
                if (distToPlayer <= meleeRange)
                {
                    TryMeleeAttack();
                }
                else
                {
                    TryShootProjectile();
                }
            
        }
        shootTimer += Time.deltaTime;
        meleeTimer += Time.deltaTime;


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

    void TryMeleeAttack()
    {
        if(meleeTimer >= meleeRate)
        {
            meleeTimer = 0f;
            playerController player = gameManager.instance.playerScript;
            player.TakeDMG(meleeDamage);
        }
    }

    void TryShootProjectile()
    {
        if (projectile == null) return;

        if (shootTimer >= shootRate)
        {
            shootTimer = 0f;
            Vector3 playerPos = gameManager.instance.player.transform.position;
            Vector3 shootDir = (playerPos - shootPos.position).normalized;

            GameObject Projectile = Instantiate(projectile, shootPos.position, Quaternion.LookRotation(shootDir));
        }
    }


    public void TakeDMG(int amount)
    {
        HP -= amount;

         agent.SetDestination(gameManager.instance.player.transform.position);

        if (HP <= 0)
        {
            gameManager.instance.UpdateGameGoal(-1);
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
