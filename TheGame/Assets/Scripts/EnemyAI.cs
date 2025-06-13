using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.UI;
public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Transform headPos;
    [SerializeField] Collider weaponCol;
    [SerializeField] Transform LootPos;
    int HPOrig;
    [SerializeField] Image hpBar;

    [SerializeField] GameObject dropItemPrefab;
    [SerializeField] Rigidbody rb;

    [SerializeField] int HP;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int FOV;
    [SerializeField] int roamDist;
    [SerializeField] int roamPauseTime;
    [SerializeField] int animTransSpeed;

    [SerializeField] Transform shootPos;
    [SerializeField] GameObject projectile;
    [SerializeField] float shootRate;
    [SerializeField] int rangeDmgAmount;

    [SerializeField] float meleeRate;
    [SerializeField] float meleeDistance;
    [SerializeField] int meleeDmgAmount;

    [SerializeField] bool isSentry;
    Color colorOrig;

    Vector3 playerDir;
    Vector3 startingPos;

    float shootTimer;
    float meleeTimer;
    float angleToPlayer;
    float roamTimer;
    float stoppingDistOrig;


    bool playerInRange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        HPOrig = HP;
        updateEnemyUI();

        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (isSentry)
        {
            agent.enabled = false;
        }

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (anim != null)
            setAnimPara();

        meleeTimer += Time.deltaTime;
        shootTimer += Time.deltaTime;

        if (agent.remainingDistance < 0.01f)
        {
            roamTimer += Time.deltaTime;
        }
        if (playerInRange && CanSeePlayer())
        {
            playerDir = gameManager.instance.player.transform.position - transform.position;

            if (!isSentry && agent.isOnNavMesh)
            {
                agent.SetDestination(gameManager.instance.player.transform.position);

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    faceTarget();
                }
            }
            else
            {
                faceTarget3D();
            }
            checkRoam();
        }
        else if (!isSentry)
        {
            checkRoam();
        }


    }

    void setAnimPara()
    {
        float agentSpeedCur = agent.velocity.normalized.magnitude;
        float animSpeedCur = anim.GetFloat("Speed");

        anim.SetFloat("Speed", Mathf.Lerp(animSpeedCur, agentSpeedCur, Time.deltaTime * animTransSpeed));
    }

    void faceTarget3D()
    {
        Vector3 direction = (gameManager.instance.player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * faceTargetSpeed);
    }

    void checkRoam()
    {
        if (roamTimer >= roamPauseTime && agent.remainingDistance < 0.01f)
        {
            roam();
        }
    }

    void roam()
    {
        roamTimer = 0;
        agent.stoppingDistance = 0;

        Vector3 ranPos = Random.insideUnitSphere * roamDist;
        ranPos += startingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(ranPos, out hit, roamDist, 1);
        agent.SetDestination(hit.position);
    }

    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = gameManager.instance.player.transform.position - headPos.position;
        Vector3 horizontalDir = new Vector3(directionToPlayer.x, 0, directionToPlayer.z);

        playerDir = (gameManager.instance.player.transform.position - headPos.position);
        angleToPlayer = Vector3.Angle(horizontalDir, transform.forward);

        Debug.DrawRay(headPos.position, horizontalDir, Color.yellow);
        RaycastHit hit;
        if (Physics.Raycast(headPos.position, directionToPlayer, out hit))
        {
            if (angleToPlayer <= FOV && hit.collider.CompareTag("Player"))
            {
                agent.SetDestination(gameManager.instance.player.transform.position);

                if (playerDir.magnitude <= meleeDistance && meleeTimer >= meleeRate)
                {
                    attackPlayer();
                }

                if (playerDir.magnitude > meleeDistance && shootTimer >= shootRate)
                {
                    shootPlayer();
                }

                agent.stoppingDistance = stoppingDistOrig;
                return true;
            }
        }

        agent.stoppingDistance = 0;
        return false;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("Collision detected with: " + collision.collider.name);
    //    if (collision.collider.CompareTag("Player"))
    //    {
    //        IDamage dmg = collision.collider.GetComponent<IDamage>();
    //        if (dmg != null)
    //        {
    //            dmg.TakeDMG(rangeDmgAmount);
    //        }
    //    }
    //}
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
            agent.stoppingDistance = 0;
        }
    }

    void DropItem()
    {
        if (dropItemPrefab == null) return;

        Instantiate(dropItemPrefab, LootPos.position, Quaternion.identity);
    }

    public void TakeDMG(int amount)
    {
        HP -= amount;
        updateEnemyUI();

        agent.SetDestination(gameManager.instance.player.transform.position);

        if (HP <= 0)
        {
            DropItem();
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

    private void attackPlayer()
    {
        meleeTimer = 0;
        gameManager.instance.player.GetComponent<playerController>().TakeDMG(meleeDmgAmount);
    }

    private void shootPlayer()
    {
        //anim.SetTrigger("Shoot");

        shootTimer = 0;
        if (projectile != null)
        {
            Vector3 dir = (gameManager.instance.player.transform.position - shootPos.position).normalized;
            GameObject proj = Instantiate(projectile, shootPos.position, Quaternion.LookRotation(dir));
        }
    }
    void updateEnemyUI()
    {
        if (hpBar != null)
            hpBar.fillAmount = (float)HP / HPOrig;
    }
}
