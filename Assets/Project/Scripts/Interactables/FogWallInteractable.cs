using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class FogWallInteractable : Interactable
{
    [Header("Fog")]
    [SerializeField] GameObject[] fogGameObjects;

    [Header("Collision")]
    [SerializeField] Collider fogWallCollider;

    [Header("I.D")]
    public int fogWallID;

    [Header("Active")]
    public NetworkVariable<bool> isActive = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public override void Interact(PlayerManager player)
    {
        base.Interact(player);
        StartCoroutine(HandleInteract(player));
    }

    private IEnumerator HandleInteract(PlayerManager player)
    {
        Quaternion targetRotation = Quaternion.Euler(0f, 135f, 0f);
        player.transform.rotation = targetRotation;

        AllowPlayerThroughFogWallCollidersServerRpc(player.NetworkObjectId);
        player.playerAnimatorManager.PlayTargetActionAnimation("PassThroughFog", true);

        yield return new WaitForSeconds(4f);
        interactableCollider.enabled = true;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        OnIsActiveChanged(false, isActive.Value);
        isActive.OnValueChanged += OnIsActiveChanged;
        WorldObjectManager.instance.AddFogWallToList(this);
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        isActive.OnValueChanged -= OnIsActiveChanged;
        WorldObjectManager.instance.RemoveFogWallFromList(this);
    }

    private void OnIsActiveChanged(bool oldStatus, bool newStatus)
    {
        if (isActive.Value)
        {
            foreach(var fogObject in fogGameObjects)
            {
                fogObject.SetActive(true);
            }
        }
        else
        {
            foreach (var fogObject in fogGameObjects)
            {
                fogObject.SetActive(false);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void AllowPlayerThroughFogWallCollidersServerRpc(ulong playerObjectID)
    {
        if (IsServer)
        {
            AllowPlayerThroughFogWallCollidersClientRpc(playerObjectID);
        }
    }

    [ClientRpc]
    private void AllowPlayerThroughFogWallCollidersClientRpc(ulong playerObjectID)
    {
        PlayerManager player = NetworkManager.Singleton.SpawnManager.SpawnedObjects[playerObjectID].GetComponent<PlayerManager>();

        if (player != null)
            StartCoroutine(DisableCollisionForTime(player));
    }

    private IEnumerator DisableCollisionForTime(PlayerManager player)
    {
        Physics.IgnoreCollision(player.characterController, fogWallCollider, true);

        yield return new WaitForSeconds(3);

        Physics.IgnoreCollision(player.characterController, fogWallCollider, false);
    }
}
