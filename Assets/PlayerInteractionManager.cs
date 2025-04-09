using UnityEngine;
using System.Collections.Generic;

public class PlayerInteractionManager : MonoBehaviour
{
    PlayerManager player;

    private List<Interactable> currentInteractableActions;

    private void Awake()
    {
        player = GetComponent<PlayerManager>();
    }

    private void Start()
    {
        currentInteractableActions = new List<Interactable>();
    }

    private void FixedUpdate()
    {
        if (!player.IsOwner)
            return;

        if (!PlayerUIManager.instance.menuWindowIsOpen && !PlayerUIManager.instance.popUpWindowIsOpen)
            CheckForInteractrable();
    }

    private void CheckForInteractrable()
    {
        if (currentInteractableActions.Count == 0)
            return;

        if (currentInteractableActions[0] == null)
        {
            currentInteractableActions.RemoveAt(0);
            return;
        }

        if (currentInteractableActions[0] != null)
            PlayerUIManager.instance.playerUIPopUpManager.SendPlayerMessagePopUp(currentInteractableActions[0].interactableText);
    }

    private void RefreshInteractionList()
    {
        for (int i = currentInteractableActions.Count - 1; i > -1;  i--)
        {
            if (currentInteractableActions[i] == null)
                currentInteractableActions.RemoveAt(i);
        }
    }

    public void AddInteractionToList(Interactable interactableObject)
    {
        RefreshInteractionList();

        if (!currentInteractableActions.Contains(interactableObject))
            currentInteractableActions.Add(interactableObject);
    }

    public void RemoveInteractionFromList(Interactable interactableObject)
    {
        if (currentInteractableActions.Contains(interactableObject))
            currentInteractableActions.Remove(interactableObject);

        RefreshInteractionList();
    }

    public void Interact()
    {
        if (currentInteractableActions.Count == 0)
            return;

        if (currentInteractableActions[0] != null)
        {
            currentInteractableActions[0].Interact(player);
            RefreshInteractionList();
        }
    }
}
