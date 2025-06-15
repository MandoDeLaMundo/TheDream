using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] protected int health;
    [SerializeField] protected int speed;

    [Header("AI Settings")]

    protected StateMachine stateMachine;
    public StateMachine StateMachine => stateMachine;
    // ^^^ Shorthand for 
    // public StateMachine StateMachine
    //      { get { return stateMachine; } }

    public NavMeshAgent agent { get; private set; }
    // public Animator animator { get; private set; }

    protected virtual void Awake()
    {
        stateMachine = new StateMachine();

        agent = GetComponent<NavMeshAgent>();
        // animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        stateMachine.ChangeState(new IdleState(this, idleDuration));
    }

    protected virtual void Update()
    {
        stateMachine.Update();
    }

    public abstract void TakeDMG(int amount);

    public float IdleTime => IdleTime;
}
