using UnityEngine;

public class PlayerInventoryManager : CharacterInventoryManager
{
    public WeaponItem currentWeapon;

    [Header("Quick Slots")]
    public WeaponItem[] weaponsInSlots = new WeaponItem[3];
    public int weaponIndex = 0;
}
