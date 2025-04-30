using UnityEngine;
using Unity.Netcode;

public class AICharacterNetworkManager : CharacterNetworkManager
{
    AIBossCharacterManager aiBossCharacter;

    protected override void Awake()
    {
        base.Awake();

        aiBossCharacter = GetComponent<AIBossCharacterManager>();
    }

    public override void CheckHP(int oldvalue, int newValue)
    {
        base.CheckHP(oldvalue, newValue);

        if (aiBossCharacter.IsOwner)
        {
            if (currentHealth.Value == 0)
                return;

            float phaseShiftHPNeeded = maxHealth.Value * (aiBossCharacter.phaseShiftHPPercentage / 100);

            if (currentHealth.Value <= phaseShiftHPNeeded)
            {
                aiBossCharacter.PhaseShift();
            }
        }
    }
}
