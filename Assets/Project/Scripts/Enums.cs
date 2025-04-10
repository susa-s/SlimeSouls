using UnityEngine;

public class Enums : MonoBehaviour
{
    
}

public enum CharacterSlot
{
    CharacterSlot_01,
    CharacterSlot_02,
    CharacterSlot_03,
    NO_SLOT
}

public enum CharacterGroup
{
    Team01,
    Team02
}

public enum WeaponModelSlot
{
    Body
}

public enum AttackType
{
    LightAttack01,
    LightAttack02,
    HeavyAttack01,
    ChargedAttack01,
    DragonSwipeAttack,
    DragonFireBreathAttack,
    DragonBackStep
}

public enum ItemPickUpType
{
    WorldSpawn
}
