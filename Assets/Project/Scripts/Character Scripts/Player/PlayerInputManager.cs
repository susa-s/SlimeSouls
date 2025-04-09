using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;
    public PlayerManager player;

    PlayerControls playerControls;

    [Header("CAMERA MOVEMENT INPUT")]
    [SerializeField] Vector2 cameraInput;
    public float cameraVerticalInput;
    public float cameraHorizontalInput;

    [Header("PLAYER MOVEMENT INPUT")]
    [SerializeField] Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;

    [Header("PLAYER ACTION INPUT")]
    [SerializeField] bool dodgeInput = false;
    [SerializeField] bool sprintInput = false;
    [SerializeField] bool jumpInput = false;
    [SerializeField] bool switchWeaponInput = false;
    [SerializeField] bool interactionInput = false;

    [Header("BUMPER INPUTS")]
    [SerializeField] bool rbInput = false;

    [Header("TRIGGER INPUTS")]
    [SerializeField] bool rtInput = false;
    [SerializeField] bool hold_rtInput = false;

    [Header("QUED INPUT")]
    private bool inputQueIsActive = false;
    [SerializeField] float defaultQueInputTime = 0.35f;
    [SerializeField] float queInputTimer = 0;
    [SerializeField] bool queRBInput = false;

    [Header("LOCK ON INPUT")]
    [SerializeField] bool lockOnInput = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        SceneManager.activeSceneChanged += OnSceneChange;

        instance.enabled = false;

        if(playerControls != null)
        {
            playerControls.Disable();
        }
    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        if(newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
        {
            instance.enabled = true;

            if (playerControls != null)
            {
                playerControls.Enable();
            }
        }
        else
        {
            instance.enabled = false;

            if (playerControls != null)
            {
                playerControls.Disable();
            }
        }
    }

    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
            playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
            playerControls.PlayerActions.SwichWeapon.performed += i => switchWeaponInput = true;
            playerControls.PlayerActions.Interact.performed += i => interactionInput = true;

            playerControls.PlayerActions.RB.performed += i => rbInput = true;

            playerControls.PlayerActions.RT.performed += i => rtInput = true;
            playerControls.PlayerActions.ChargeRT.performed += i => hold_rtInput = true;
            playerControls.PlayerActions.ChargeRT.canceled += i => hold_rtInput = false;

            playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
            playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;

            playerControls.PlayerActions.LockOn.performed += i => lockOnInput = true;

            playerControls.PlayerActions.QueRB.performed += i => QueInput(ref queRBInput);
        }

        playerControls.Enable();
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (enabled)
        {
            if (focus)
            {
                playerControls.Enable();
            }
            else
            {
                playerControls.Disable();
            }
        }
    }

    private void Update()
    {
        HandleAllInputs();
    }

    private void HandleAllInputs()
    {
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
        HandleDodgeInput();
        HandleSprintInput();
        HandleJumpInput();
        HandleRBInput();
        HandleRTInput();
        HandleChargeRTInput();
        HandleSwitchWeaponInput();
        HandleQuedInputs();
        HandleLockOnInput();
        HandleInteractionInput();
    }

    private void HandlePlayerMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

        if(moveAmount <= 0.5 && moveAmount > 0)
        {
            moveAmount = 0.5f;
        }
        else if(moveAmount > 0.5 && moveAmount <= 1)
        {
            moveAmount = 1;
        }

        if (player == null)
            return;

        if(moveAmount != 0)
        {
            player.playerNetworkManager.isMoving.Value = true;
        }
        else
        {
            player.playerNetworkManager.isMoving.Value = false;
        }

        player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
    }

    private void HandleCameraMovementInput()
    {
        cameraVerticalInput = cameraInput.y;
        cameraHorizontalInput = cameraInput.x;
    }

    private void HandleDodgeInput()
    {
        if (player.playerNetworkManager.isJumping.Value)
            return;
        if (dodgeInput)
        {
            dodgeInput = false;

            player.playerLocomotionManager.AttemptToPerformDodge();
        }
    }

    private void HandleSprintInput()
    {
        if (sprintInput)
        {
            player.playerLocomotionManager.HandleSprinting();
        }
        else
        {
            player.playerNetworkManager.isSprinting.Value = false;
        }
    }

    private void HandleJumpInput()
    {
        if (jumpInput)
        {
            jumpInput = false;

            player.playerLocomotionManager.AttemptToPerformJump();
        }
    }

    private void HandleRBInput()
    {
        if (player.isDead.Value)
            return;

        if (rbInput)
        {
            rbInput = false;

            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentWeapon.rbAction, player.playerInventoryManager.currentWeapon);
        }
    }

    private void HandleRTInput()
    {
        if (player.isDead.Value)
            return;

        if (rtInput)
        {
            rtInput = false;

            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentWeapon.rtAction, player.playerInventoryManager.currentWeapon);
        }
    }

    private void HandleChargeRTInput()
    {
        if (player.isPerformingAction)
        {
            player.playerNetworkManager.isChargingAttack.Value = hold_rtInput;
        }
    }

    private void HandleSwitchWeaponInput()
    {
        if (player.isDead.Value)
            return;

        if (switchWeaponInput)
        {
            switchWeaponInput = false;
            player.playerEquipmentManager.SwitchWeapon();
        }
    }

    private void HandleInteractionInput()
    {
        if (interactionInput)
        {
            interactionInput = false;

            player.playerInteractionManager.Interact();
        }
    }

    private void HandleLockOnInput()
    {
        if (player.playerNetworkManager.islockedOn.Value)
        {
            if (player.playerCombatManager.currentTarget == null)
                return;

            if (player.playerCombatManager.currentTarget.isDead.Value)
            {
                player.playerNetworkManager.islockedOn.Value = false;
            }

        }

        if (lockOnInput && player.playerNetworkManager.islockedOn.Value)
        {
            lockOnInput = false;
            PlayerCamera.instance.ClearLockOnTargets();
            player.playerNetworkManager.islockedOn.Value = false;

            return;
        }

        if (lockOnInput && !player.playerNetworkManager.islockedOn.Value)
        {
            lockOnInput = false;
            PlayerCamera.instance.HandleLocatingLockOnTarget();
            
            if(PlayerCamera.instance.nearestLockOnTarget != null)
            {
                player.playerCombatManager.SetTarget(PlayerCamera.instance.nearestLockOnTarget);
                player.playerNetworkManager.islockedOn.Value = true;
            }
        }
    }

    private void QueInput(ref bool quedInput)
    {
        queRBInput = false;

        if(player.isPerformingAction || player.playerNetworkManager.isJumping.Value)
        {
            quedInput = true;
            queInputTimer = defaultQueInputTime;
            inputQueIsActive = true;
        }
    }

    private void ProcessQuedInputs()
    {
        if (player.isDead.Value)
            return;

        if (queRBInput)
            rbInput = true;
    }

    private void HandleQuedInputs()
    {
        if (inputQueIsActive)
        {
            if(queInputTimer > 0)
            {
                queInputTimer -= Time.deltaTime;
                ProcessQuedInputs();
            }
            else
            {
                queRBInput = false;
                inputQueIsActive = false;
                queInputTimer = 0;
            }
        }
    }
}
