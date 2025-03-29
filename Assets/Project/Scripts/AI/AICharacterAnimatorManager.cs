using UnityEngine;

public class AICharacterAnimatorManager : CharacterAnimatorManager
{
    AICharacterManager aiCharacter;

    protected override void Awake()
    {
        base.Awake();

        aiCharacter = GetComponent<AICharacterManager>();
    }
    private void OnAnimatorMove()
    {
        if (aiCharacter.IsOwner)
        {
            if (!aiCharacter.isGrounded)
                return;

            Vector3 velocity = aiCharacter.animator.deltaPosition;

            aiCharacter.characterController.Move(velocity);
            aiCharacter.transform.rotation *= aiCharacter.animator.deltaRotation;
        }
        else
        {
            if (!aiCharacter.isGrounded)
                return;

            Vector3 velocity = aiCharacter.animator.deltaPosition;

            aiCharacter.characterController.Move(velocity);
            aiCharacter.transform.position = Vector3.SmoothDamp(transform.position, aiCharacter.characterNetworkManager.networkPosition.Value, ref aiCharacter.characterNetworkManager.networkPositionVelocity, aiCharacter.characterNetworkManager.networkPositionSmoothTime);
            aiCharacter.transform.rotation *= aiCharacter.animator.deltaRotation;
        }
    }
}
