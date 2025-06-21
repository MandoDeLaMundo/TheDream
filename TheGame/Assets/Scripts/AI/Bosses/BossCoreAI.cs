using UnityEngine;
using UnityEngine.AI;

public class BossCoreAI : MonoBehaviour
{
    [Header("Boss Stats")]
    public int health;
    [SerializeField] public GameObject hotSpot;
    [SerializeField] public NavMeshAgent agent;

    [HideInInspector] public int phase2Threshold;
    [HideInInspector] public BossPhaseBase currentPhase;
    [HideInInspector] public BossPhaseBase phase1;
    [HideInInspector] public BossPhaseBase phase2;

    protected virtual void Start()
    {
        if (!agent)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        phase2Threshold = health / 2;

    }

    protected virtual void Update()
    {

    }

    public void StartPhase(BossPhaseBase phase)
    {
        
    }

    public void TakeDamage(int amount)
    {
        if (hotSpot.activeSelf || currentPhase == phase1)
        {
            health -= amount;
        }

        if (health <= phase2Threshold)
        {
            StartPhase(phase2);
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


        Destroy(this.gameObject);
    }
}
