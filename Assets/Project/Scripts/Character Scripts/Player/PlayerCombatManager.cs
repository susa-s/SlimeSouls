using UnityEngine;
using Unity.Netcode;

public class PlayerCombatManager : CharacterCombatManager
{
    PlayerManager player;

    public WeaponItem currentWeaponBeingUsed;

    [Header("Flags")]
    public bool canComboWithWeapon = false;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
    {

        if (player.IsOwner)
        {
            weaponAction.AttemptToPerformAction(player, weaponPerformingAction);

            player.playerNetworkManager.NotifyServerOfWeaponActionServerRpc(NetworkManager.Singleton.LocalClientId, weaponAction.actionID, weaponPerformingAction.itemID);
        }
        
    }

    public virtual void DrainStaminaBasedOnAttack()
    {
        if (!player.IsOwner)
            return;

        if (currentWeaponBeingUsed == null)
            return;

        float staminaDeducted = 0;

        switch (currentAttackType)
        {
            case AttackType.LightAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                break;
            case AttackType.LightAttack02:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                break;
            case AttackType.HeavyAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;
                break;
            default:
                break;
        }

        player.playerNetworkManager.currentStamina.Value -= Mathf.RoundToInt(staminaDeducted);
    }
}
