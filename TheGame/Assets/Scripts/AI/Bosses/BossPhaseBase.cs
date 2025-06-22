using UnityEngine;

public abstract class BossPhaseBase
{
    protected BossCoreAI boss;

    public BossPhaseBase(BossCoreAI _boss)
    {
        boss = _boss;
    }

    public abstract void Enter();

    public abstract void Update();

    public abstract void Exit();
}
