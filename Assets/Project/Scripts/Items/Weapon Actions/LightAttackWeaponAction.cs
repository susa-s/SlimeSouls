using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack Action")]
public class LightAttackWeaponAction : WeaponItemAction
{
    [SerializeField] string Light_Attack_01 = "SlimeLightAttack01";
    [SerializeField] string Light_Attack_02 = "SlimeLightAttack02";

    public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

        if (!playerPerformingAction.IsOwner)
            return;

        if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
            return;

        if (!playerPerformingAction.isGrounded)
            return;

        PerformLightAttack(playerPerformingAction, weaponPerformingAction);
    }

    private void PerformLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (playerPerformingAction.playerCombatManager.canComboWithWeapon && playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerCombatManager.canComboWithWeapon = false;

            if(playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == Light_Attack_01)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackAnimation(AttackType.LightAttack02, Light_Attack_02, true);
            }
            else
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackAnimation(AttackType.LightAttack01, Light_Attack_01, true, false);
            }
        }
        else if(!playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAttackAnimation(AttackType.LightAttack01, Light_Attack_01, true);
        }
    }
}
