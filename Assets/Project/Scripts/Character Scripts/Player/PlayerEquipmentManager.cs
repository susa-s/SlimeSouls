using UnityEngine;

public class PlayerEquipmentManager : CharacterEquipmentManager
{
    PlayerManager player;

    public WeaponModelInstantiationLocation body;

    [SerializeField] WeaponManager weaponManager;

    public GameObject weaponModel;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();

        InitializeWeaponSlots();
    }

    protected override void Start()
    {
        base.Start();

        LoadBodyWeapon();
    }

    private void InitializeWeaponSlots()
    {
        WeaponModelInstantiationLocation[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationLocation>();

        foreach(var weaponSlot in weaponSlots)
        {
            if(weaponSlot.weaponSlot == WeaponModelSlot.Body)
            {
                body = weaponSlot;
            }
        }
    }

    public void LoadBodyWeapon()
    {
        if(player.playerInventoryManager.currentWeapon != null)
        {
            body.UnloadWeaponModel();

            weaponModel = Instantiate(player.playerInventoryManager.currentWeapon.weaponModel);
            body.LoadWeapon(weaponModel);
            weaponManager = weaponModel.GetComponent<WeaponManager>();
            weaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentWeapon);
        }
    }

    public void SwitchWeapon()
    {
        if (!player.IsOwner)
            return;

        player.playerAnimatorManager.PlayTargetActionAnimation("Swap_Weapon", false, false, true, true);

        WeaponItem selectedWeapon = null;

        player.playerInventoryManager.weaponIndex += 1;

        if(player.playerInventoryManager.weaponIndex < 0 || player.playerInventoryManager.weaponIndex > 2)
        {
            player.playerInventoryManager.weaponIndex = 0;

            float weaponCount = 0;
            WeaponItem firstWeapon = null;
            int firstWeaponPosition = 0;

            for (int i = 0; i < player.playerInventoryManager.weaponsInSlots.Length; i++)
            {
                if (player.playerInventoryManager.weaponsInSlots[i].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                {
                    weaponCount += 1;

                    if (firstWeapon == null)
                    {
                        firstWeapon = player.playerInventoryManager.weaponsInSlots[i];
                        firstWeaponPosition = i;
                    }
                }
            }

            if (weaponCount <= 1)
            {
                player.playerInventoryManager.weaponIndex = -1;
                selectedWeapon = WorldItemDatabase.Instance.unarmedWeapon;
                player.playerNetworkManager.currentWeaponID.Value = selectedWeapon.itemID;
            }
            else
            {
                player.playerInventoryManager.weaponIndex = firstWeaponPosition;
                player.playerNetworkManager.currentWeaponID.Value = firstWeapon.itemID;
            }

            return;
        }

        foreach(WeaponItem weapon in player.playerInventoryManager.weaponsInSlots)
        {
            if(player.playerInventoryManager.weaponsInSlots[player.playerInventoryManager.weaponIndex].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
            {
                selectedWeapon = player.playerInventoryManager.weaponsInSlots[player.playerInventoryManager.weaponIndex];
                player.playerNetworkManager.currentWeaponID.Value = player.playerInventoryManager.weaponsInSlots[player.playerInventoryManager.weaponIndex].itemID;
                return;
            }
        }

        if(selectedWeapon == null && player.playerInventoryManager.weaponIndex <= 2)
        {
            SwitchWeapon();
        }
    }
}
