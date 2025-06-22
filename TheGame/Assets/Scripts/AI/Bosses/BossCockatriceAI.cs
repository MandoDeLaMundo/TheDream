using UnityEngine;

public class BossCockatriceAI : BossCoreAI
{
    protected override void Start()
    {
        base.Start();
        phase1 = new CockatricePhase1(this);
        phase2 = new CockatricePhase2(this);
        currentPhase = phase1;
        currentPhase.Enter();
    }
}
