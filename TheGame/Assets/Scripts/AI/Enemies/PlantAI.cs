using UnityEngine;

public class PlantAI : StationaryEnemy
{
    
    protected override void Start()
    {
        base.Start();
        stateMachine.ChangeState(new StationaryIdleState(this));
    }
}
