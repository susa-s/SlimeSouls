using UnityEngine;

public class AIDummyCombatManager : AICharacterCombatManager
{
    [Header("Damage Collider")]
    [SerializeField] DummyWeaponDamageCollider weaponCollider;

    [Header("Damage")]
    [SerializeField] int baseDamage = 25;
    [SerializeField] float attack01DamageModifer = 1.0f;
    [SerializeField] float attack02DamageModifer = 1.4f;

    public void SetAttack01Damage()
    {
        weaponCollider.physicalDamage = baseDamage * attack01DamageModifer;
    }

    public void SetAttack02Damage()
    {
        weaponCollider.physicalDamage = baseDamage * attack02DamageModifer;
    }

    public void OpenWeaponCollider()
    {
        weaponCollider.EnableDamageCollider();
    }

    public void CloseWeaponCollider()
    {
        weaponCollider.DisableDamageCollider();
    }
}
