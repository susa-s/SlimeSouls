using UnityEngine;
using System.Collections.Generic;

public class PlayerInventoryManager : CharacterInventoryManager
{
    public WeaponItem currentWeapon;

    [Header("Quick Slots")]
    public WeaponItem[] weaponsInSlots = new WeaponItem[3];
    public int weaponIndex = 0;

    [Header("Inventory")]
    public List<Item> itemsInInventory;

    public void AddItemToInventory(Item item)
    {
        itemsInInventory.Add(item);
        
        if(item is WeaponItem weapon)
        {
            weaponsInSlots[0] = weapon;
        }
    }

    public void RemoveItemFromInventory(Item item)
    {

    }
}
