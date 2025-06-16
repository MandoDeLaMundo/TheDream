using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
    Color colorOrig;

    [Header("Enemy Stats")]
    [SerializeField] public int health;
    [SerializeField] public Transform shootPos;
    [SerializeField] public GameObject projectile;
    [SerializeField] public float shootRate;
    [SerializeField] public float meleeRate;
    public int meleeDmgAmt;
    public float meleeRange;
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
    public float angleToPlayer;
    public float stoppingDistOrig;
    public float shootTimer;
    public float meleeTimer;
    public float roamTimer;
    public bool playerInRange;

    public StateMachine stateMachine = new StateMachine();

    void Start()
    {
        if (!agent)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        if (!model)
        {
            model = GetComponentInChildren<Renderer>();
        }
        if (!anim)
        { 
            anim = GetComponentInChildren<Animator>();
        }

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

        enemyHP.transform.rotation = gameManager.instance.player.transform.rotation;
    }

    public virtual void TakeDMG(int amount)
    {
        health -= amount;
        UpdateEnemyUI();

        if (health <= 0)
        {
            stateMachine.ChangeState(new DeadState(this));
            return;
        }

        StartCoroutine(FlashRed());

        if (!(stateMachine.CurrentState is ChaseState || stateMachine.CurrentState is AttackState))
        {
            stateMachine.ChangeState(new ChaseState(this));
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

        if (angleToPlayer > FOV) return false;

        if (Physics.Raycast(headPos.position, playerDir, out RaycastHit hit))
        {
            return hit.collider.CompareTag("Player");
        }

        return false;
    }


    void UpdateEnemyUI()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = (float)health / healthOrig;
        }

    }
}
