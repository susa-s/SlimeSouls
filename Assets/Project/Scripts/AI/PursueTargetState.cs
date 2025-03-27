using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "A.I/States/Pursue Target")]
public class PursueTargetState : AIState
{
    public override AIState Tick(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isPerformingAction)
            return this;

        if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
            return SwitchState(aiCharacter, aiCharacter.idle);

        if (!aiCharacter.navMeshAgent.enabled)
            aiCharacter.navMeshAgent.enabled = true;

        aiCharacter.aiCharacterLocomotionManager.RotateTowardsAgent(aiCharacter);

        NavMeshPath path = new NavMeshPath();
        aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
        aiCharacter.navMeshAgent.SetPath(path);

        return this;
    }
}
