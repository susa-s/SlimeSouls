using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public MeleeWeaponDamageCollider meleeDamageCollider;

    private void Awake()
    {
        meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
    }

    public void SetWeaponDamage(CharacterManager characterWeildingWeaon, WeaponItem weapon)
    {
        meleeDamageCollider.characterCausingDamage = characterWeildingWeaon;
        meleeDamageCollider.physicalDamage = weapon.physicalDamage;
        meleeDamageCollider.magicDamage = weapon.magicDamage;

        meleeDamageCollider.light_Attack_01_Modifier = weapon.light_attack_01_Modifier;
    }
}
