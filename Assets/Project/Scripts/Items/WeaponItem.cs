using UnityEngine;

public class WeaponItem : Item
{
    [Header("Weapon Model")]
    public GameObject weaponModel;

    [Header("Weapon Requirements")]
    public int strengthREQ = 0;

    [Header("Weapon Base Damage")]
    public int physicalDamage = 0;
    public int magicDamage = 0;

    [Header("Weapon Base Poise Damage")]
    public float poiseDamage = 10;

    [Header("Attack Modifiers")]
    [Header("Attack Modifiers")]
    public float light_attack_01_Modifier = 1.0f;
    public float light_attack_02_Modifier = 1.2f;
    public float heavy_attack_01_Modifier = 1.4f;
    public float charged_attack_01_Modifier = 2.0f;

    [Header("Stamina Cost Modifiers")]
    public int baseStaminaCost = 20;
    public float lightAttackStaminaCostMultiplier = 0.9f;

    [Header("Actions")]
    public WeaponItemAction rbAction;
    public WeaponItemAction rtAction;

    [Header("Whooshes")]
    public AudioClip[] whooshes;
}
