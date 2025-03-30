using UnityEngine;

public class AIDragonCombatManager : AICharacterCombatManager
{
    [Header("Damage Collider")]
    [SerializeField] DragonClawDamageCollider rightClawCollider;
    [SerializeField] DragonClawDamageCollider leftClawCollider;
    [SerializeField] DragonFireBreathDamageCollider fireBreathCollider;

    [Header("Damage")]
    [SerializeField] int baseDamage = 25;
    [SerializeField] float attack01DamageModifer = 1.0f;
    [SerializeField] float attack02DamageModifer = 1.4f;

    public void SetAttack01Damage()
    {
        rightClawCollider.physicalDamage = baseDamage * attack01DamageModifer;
        leftClawCollider.physicalDamage = baseDamage * attack01DamageModifer;
        fireBreathCollider.physicalDamage = baseDamage * attack02DamageModifer;
    }

    public void SetAttack02Damage()
    {

    }

    public void OpenRightClawCollider()
    {
        aiCharacter.characterSFXManager.PlayAttackGrunt();
        rightClawCollider.EnableDamageCollider();
    }

    public void OpenLeftCollider()
    {
        leftClawCollider.EnableDamageCollider();
    }

    public void OpenFireBreathCollider()
    {
        aiCharacter.characterSFXManager.PlayAttackGrunt();
        fireBreathCollider.EnableDamageCollider();
    }

    public void CloseRightClawCollider()
    {
        rightClawCollider.DisableDamageCollider();
    }

    public void CloseLeftClawCollider()
    {
        leftClawCollider.DisableDamageCollider();
    }

    public void CloseFireBreathCollider()
    {
        fireBreathCollider.DisableDamageCollider();
    }
}
