using UnityEngine;

public class AIDummyCharacterManager : CharacterManager
{
    [HideInInspector] public AICharacterNetworkManager aiCharacterNetworkManager;
    [HideInInspector] public AICharacterLocomotionManager aiCharacterLocomotionManager;

    protected override void Awake()
    {
        base.Awake();

        aiCharacterNetworkManager = GetComponent<AICharacterNetworkManager>();
        aiCharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // aiCharacterNetworkManager.currentHealth.OnValueChanged += aiCharacterNetworkManager.CheckHP;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        // aiCharacterNetworkManager.currentHealth.OnValueChanged -= aiCharacterNetworkManager.CheckHP;
    }
}
