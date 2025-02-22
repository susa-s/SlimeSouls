using UnityEngine;

public class TitleScreenLoadMenuInputManager : MonoBehaviour
{
    PlayerControls playerControls;

    [Header("Title Screen Inputs")]
    [SerializeField] bool deleteCharacterSlot = false;

    private void Update()
    {
        if (deleteCharacterSlot)
        {
            deleteCharacterSlot = false;
            TitleScreenManager.Instance.AttemptToDeleteCharacterSlot();
        }
    }

    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.UI.Square.performed += i => deleteCharacterSlot = true;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}
