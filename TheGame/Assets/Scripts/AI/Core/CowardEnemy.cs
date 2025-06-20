using UnityEngine;

public class CowardEnemy : EnemyBase
{
    [Header("Coward Stats")]
    public float fleeDistance;
    public float fleeRange;
    [HideInInspector] public bool canAttack;
}
