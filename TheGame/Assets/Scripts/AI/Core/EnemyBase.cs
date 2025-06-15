using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class EnemyBase : MonoBehaviour, IDamage
{
    public enum AttackType
    {
        Melee,
        Ranged,
        Hybrid
    }

    [Header("References")]
    [SerializeField] public Renderer model;
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public Animator anim;
    [SerializeField] public Transform headPos;
    [SerializeField] public Transform lootPos;
    [SerializeField] public Collider weaponCol;
    [SerializeField] public GameObject dropItemPrefab;
    public Image enemyHP;
    public Image hpBar;
    public Color colorOrig;

    [Header("Enemy Stats")]
    [SerializeField] public int health;
    [SerializeField] public Transform shootPos;
    [SerializeField] public GameObject projectile;
    [SerializeField] public float shootRate;
    [SerializeField] public float meleeRate;
    public int meleeDmgAmt;
    public float meleeRange;
    public float angleToPlayer;
    public float shootTimer;
    public float roamTimer;
    public bool isAttacking;
    public AttackType attackType;
    int healthOrig;

    [Header("AI Settings")]
    [SerializeField] public int faceTargetSpeed;
    [SerializeField] public int FOV;
    [SerializeField] public int roamDist;
    [SerializeField] public int roamPauseTime;
    [SerializeField] public int animTransSpeed;
    public Vector3 playerDir;
    public Vector3 startingPos;
    public float stoppingDistOrig;
    public bool playerInRange;

    public StateMachine stateMachine;

    void Start()
    {
        colorOrig = model.material.color;
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        healthOrig = health;
        UpdateEnemyUI();

        stateMachine.ChangeState(new IdleState(this));
    }

    void Update()
    {
        stateMachine.Update();
    }

    public virtual void TakeDMG(int amount)
    {
        health -= amount;
        agent.SetDestination(gameManager.instance.player.transform.position);

        if (health <= 0)
        {
            stateMachine.ChangeState(new DeadState(this));
        }

        else
        {
            StartCoroutine(FlashRed());
        }
    }

    IEnumerator FlashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        model.material.color = colorOrig;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            agent.stoppingDistance = 0;
        }
    }

    public bool CanSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);

        if (angleToPlayer <= FOV)
        {
            if (Physics.Raycast(headPos.position, playerDir, out RaycastHit hit))
            {
                return hit.collider.CompareTag("Player");
            }
        }
        return false;
    }


    void DropItem()
    {
        if (dropItemPrefab)
        {
            Instantiate(dropItemPrefab, lootPos.position, Quaternion.identity);
        }
    }

    void Shoot()
    {
        if (projectile)
        {
            Vector3 playerDir = (gameManager.instance.player.transform.position - shootPos.position).normalized;
            Instantiate(projectile, shootPos.position, Quaternion.LookRotation(playerDir));
        }
    }

    void Attack()
    {
        gameManager.instance.player.GetComponent<playerController>().TakeDMG(meleeDmgAmt);
    }

    void UpdateEnemyUI()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = (float)health / healthOrig;
        }    
    }
}
