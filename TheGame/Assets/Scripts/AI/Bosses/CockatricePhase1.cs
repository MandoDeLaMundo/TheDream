using UnityEngine;

public class CockatricePhase1 : BossPhaseBase
{
    public CockatricePhase1(BossCoreAI boss) : base(boss) { }

    public override void Enter()
    {
        Debug.Log("Cockatrice Phase 1 begins!");
    }
    public override void Update()
    {
        
    }

    public override void Exit()
    {
        Debug.Log("Cockatrice Phase 1 ends!");

    }
}
