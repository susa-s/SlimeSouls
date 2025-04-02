using UnityEngine;

public class AIDragonCharacterManager : AIBossCharacterManager
{
    public AIDragonSFXManager dragonSFXManager;

    protected override void Awake()
    {
        base.Awake();

        dragonSFXManager = GetComponent<AIDragonSFXManager>();
    }
}
