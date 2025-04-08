using UnityEngine;

public class AIDragonCharacterManager : AIBossCharacterManager
{
    public AIDragonSFXManager dragonSFXManager;
    public AIDragonCombatManager dragonCombatManager;

    protected override void Awake()
    {
        base.Awake();

        dragonSFXManager = GetComponent<AIDragonSFXManager>();
        dragonCombatManager = GetComponent<AIDragonCombatManager>();
    }
}
