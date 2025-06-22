using UnityEngine;

public class CockatricePhase2 : BossPhaseBase
{
    public CockatricePhase2(BossCoreAI boss) : base(boss) { }

    public override void Enter()
    {
        Debug.Log("Cockatrice Phase 2 Begins!");
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {
        Debug.Log("Cockatrice Phase 2 Ends!");
    }
}
