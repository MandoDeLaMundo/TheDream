using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossCoreAI : MonoBehaviour, IDamage
{
    [Header("References")]
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public GameObject bossDoor;
    [SerializeField] public GameObject hotSpot;
    [SerializeField] public GameObject dropItemPrefab;
    [SerializeField] public Transform itemDropPos;
    [SerializeField] public Transform headPos;
    [SerializeField] public Animator anim;
    [SerializeField] public Image hpBar;

    [Header("Boss Stats")]
    public int health;
    [HideInInspector] public int phase2Threshold;
    [HideInInspector] public int healthOrig;
    [HideInInspector] public BossPhaseBase currentPhase;
    [HideInInspector] public BossPhaseBase phase1;
    [HideInInspector] public BossPhaseBase phase2;

    [Header("AI Settings")]
    [SerializeField] public float attackCooldown;
    [SerializeField] public float faceTargetSpeed;
    [SerializeField] public int FOV;
    [HideInInspector] public Vector3 playerDir;
    [HideInInspector] public Vector3 startingPos;
    [HideInInspector] public float angleToPlayer;
    [HideInInspector] public float attackTimer;
    [HideInInspector] public bool isAttacking;

    protected virtual void Start()
    {
        if (!agent)
            agent = GetComponent<NavMeshAgent>();
        if (!anim)
            anim = GetComponent<Animator>();

        if (bossDoor)
            bossDoor.SetActive(true);

        healthOrig = health;
        phase2Threshold = healthOrig / 2;
        startingPos = transform.position;
        gameManager.instance.bossHPBar.gameObject.SetActive(true);
        UpdateUI();
    }

    protected virtual void Update()
    {
        currentPhase.Update();

        //if (health <= phase2Threshold && currentPhase != phase2)
        //{
        //    StartPhase(phase2);
        //}
    }

    public void StartPhase(BossPhaseBase phase)
    {
        currentPhase.Exit();
        currentPhase = phase;

        if (hotSpot) hotSpot.SetActive(false);

        currentPhase.Enter();
        StartCoroutine(PhaseTransitionPause(1));
    }

    public virtual void TakeDMG(int amount)
    {
        if (currentPhase == phase1)
        {
            health -= amount;
            UpdateUI();
        }

        if (health <= 0)
        {
            BossDefeated();
        }
    }

    public void BossDefeated()
    {
        if (agent)
            agent.isStopped = true;

        anim.SetTrigger("Die");

        if (dropItemPrefab)
            Instantiate(dropItemPrefab, itemDropPos.position, Quaternion.identity);

        Destroy(this.gameObject);
        gameManager.instance.bossHPBar.gameObject.SetActive(false);
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

    public void FacePlayer()
    {
        Vector3 dir = (gameManager.instance.player.transform.position - transform.position).normalized;
        dir.y = 0;
        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    public void UpdateUI()
    {
        if (hpBar)
            hpBar.fillAmount = (float)health / healthOrig;
    }

    IEnumerator PhaseTransitionPause(float phasePauseTime)
    {
        yield return new WaitForSeconds(phasePauseTime);
    }
}
