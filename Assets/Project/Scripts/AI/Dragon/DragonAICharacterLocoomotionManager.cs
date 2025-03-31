using UnityEngine;

public class DragonAICharacterLocoomotionManager : AICharacterLocomotionManager
{
    protected override void HandleGroundCheck()
    {
        base.HandleGroundCheck();

        character.isGrounded = true;
    }
}
