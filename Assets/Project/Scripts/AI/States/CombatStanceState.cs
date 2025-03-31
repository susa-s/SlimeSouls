using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "A.I/States/Combat Stance")]
public class CombatStanceState : AIState
{
    [Header("Attacks")]
    public List<AICharacterAttackAction> aiCharacterAttacks;
    protected List<AICharacterAttackAction> potentialAttacks;
    private AICharacterAttackAction chosenAttack;
    private AICharacterAttackAction previousAttack;
    protected bool hasAttack = false;

    [Header("Combo")]
    [SerializeField] protected bool canPerformCombo = false;
    [SerializeField] protected int chanceToPerformCombo = 25;
    protected bool hasRolledForComboChance = false;

    [Header("Engagement Distance")]
    [SerializeField] protected float maximumEngagementDistance = 5;

    public override AIState Tick(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isPerformingAction)
            return this;

        if (!aiCharacter.navMeshAgent.enabled)
            aiCharacter.navMeshAgent.enabled = true;

        if (aiCharacter.aiCharacterCombatManager.enablePivot)
        {
            if (!aiCharacter.aiCharacterNetworkManager.isMoving.Value)
            {
                if (aiCharacter.aiCharacterCombatManager.viewableAngle < -30 || aiCharacter.aiCharacterCombatManager.viewableAngle > 30)
                    aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
            }
        }

        aiCharacter.aiCharacterCombatManager.RotateTowardsAgent(aiCharacter);

        if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
            return SwitchState(aiCharacter, aiCharacter.idle);

        if (!hasAttack)
        {
            GetNewAttack(aiCharacter);
        }
        else
        {
            aiCharacter.attack.currentAttack = chosenAttack;

            return SwitchState(aiCharacter, aiCharacter.attack);
        }

        if (aiCharacter.aiCharacterCombatManager.distanceFromTarget > maximumEngagementDistance)
            return SwitchState(aiCharacter, aiCharacter.pursueTarget);

        NavMeshPath path = new NavMeshPath();
        aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
        aiCharacter.navMeshAgent.SetPath(path);

        return this;
    }

    protected virtual void GetNewAttack(AICharacterManager aiCharacter)
    {
        potentialAttacks = new List<AICharacterAttackAction>();

        var totalWeight = 0;

        foreach (var potentialAttack in aiCharacterAttacks)
        {
            if (potentialAttack.minimumAttackDistance > aiCharacter.aiCharacterCombatManager.distanceFromTarget)
                continue;

            if (potentialAttack.maximumAttackDistance < aiCharacter.aiCharacterCombatManager.distanceFromTarget)
                continue;

            if (potentialAttack.minimumAttackAngle > aiCharacter.aiCharacterCombatManager.viewableAngle)
                continue;

            if (potentialAttack.maximumAttackAngle < aiCharacter.aiCharacterCombatManager.viewableAngle)
                continue;

            potentialAttacks.Add(potentialAttack);
            totalWeight += potentialAttack.attackWeight;
        }

        if (potentialAttacks.Count <= 0)
            return;

        var randomWeightValue = Random.Range(1, totalWeight + 1);
        var processedWeight = 0;

        foreach(var attack in potentialAttacks)
        {
            processedWeight += attack.attackWeight;

            if(randomWeightValue <= processedWeight)
            {
                chosenAttack = attack;
                previousAttack = chosenAttack;
                hasAttack = true;
                return;
            }
        }
    }

    protected virtual bool RollForOutcomeChance(int outcomeChance)
    {
        bool outcomeWillBePerformed = false;

        int randomPercentage = Random.Range(0, 100);

        if (randomPercentage < outcomeChance)
            outcomeWillBePerformed = true;

        return outcomeWillBePerformed;
    }

    protected override void ResetStateFlags(AICharacterManager aiCharacter)
    {
        base.ResetStateFlags(aiCharacter);

        hasAttack = false;
        hasRolledForComboChance = false;
    }
}
