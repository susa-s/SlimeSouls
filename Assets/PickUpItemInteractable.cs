using UnityEngine;
using Unity.Netcode;

public class PickUpItemInteractable : Interactable
{
    public ItemPickUpType pickUpType;

    [Header("Item")]
    [SerializeField] Item item;

    [Header("World Spawn Pick Up")]
    [SerializeField] int itemID;
    [SerializeField] bool hasBeenLooted = false;

    protected override void Start()
    {
        base.Start();

        if (pickUpType == ItemPickUpType.WorldSpawn)
            CheckIfWorldItemWasAlreadyLooted();

        CheckIfWorldItemWasAlreadyLooted();
    }

    private void CheckIfWorldItemWasAlreadyLooted()
    {
        if (!NetworkManager.Singleton.IsHost)
        {
            gameObject.SetActive(false);
            return;
        }

        if (!WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.ContainsKey(itemID))
        {
            WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.Add(itemID, false);
        }

        hasBeenLooted = WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted[itemID];

        if (hasBeenLooted)
            gameObject.SetActive(false);
    }

    public override void Interact(PlayerManager player)
    {
        base.Interact(player);

        player.characterSFXManager.PlaySFX(WorldSFXManager.instance.itemPickUpSFX);

        player.playerInventoryManager.AddItemToInventory(item);

        PlayerUIManager.instance.playerUIPopUpManager.SendItemPopUp(item, 1);

        if(pickUpType == ItemPickUpType.WorldSpawn)
        {
            if (WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.ContainsKey((int)itemID))
            {
                WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.Remove(itemID);
            }

            WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.Add(itemID, true);
        }

        Destroy(gameObject);
    }
}
