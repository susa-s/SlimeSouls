using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    CharacterManager Character;

    [Header("Stamina Regneration")]
    private float staminaRegenerationTimer = 0;
    private float staminaTickTimer = 0;
    [SerializeField] float staminaRegenerationDelay = 1;
    [SerializeField] float staminaRegenAmount = 0.25f;

    [Header("Poise")]
    public float totalPoiseDamage;
    public float offensivePoiseBonus;
    public float defaultPoiseDefense;
    public float defaultPoiseResetTime = 8;
    public float poiseResetTimer = 0;

    protected virtual void Awake()
    {
        Character = GetComponent<CharacterManager>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandlePoiseResetTimer();    
    }

    public int CalculateHealthBasedOnVitalityLevel(int vitality)
    {
        float health = 0;
        health = vitality * 10;
        return Mathf.RoundToInt(health);
    }

    public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
    {
        float stamina = 0;
        stamina = endurance * 10;
        return Mathf.RoundToInt(stamina);
    }

    public virtual void RegenerateStamina()
    {
        if (!Character.IsOwner)
            return;

        if (Character.characterNetworkManager.isSprinting.Value)
            return;

        if (Character.isPerformingAction)
            return;

        staminaRegenerationTimer += Time.deltaTime;

        if (staminaRegenerationTimer >= staminaRegenerationDelay)
        {
            if (Character.characterNetworkManager.currentStamina.Value < Character.characterNetworkManager.maxStamina.Value)
            {
                staminaTickTimer += Time.deltaTime;

                if (staminaTickTimer >= 0.1)
                {
                    staminaTickTimer = 0;
                    Character.characterNetworkManager.currentStamina.Value += staminaRegenAmount;
                }
            }
        }
    }

    public virtual void ResetStaminaRegenTimer(float previousStaminaAmount, float currentStaminaAmount)
    {
        if(currentStaminaAmount < previousStaminaAmount)
        {
            staminaRegenerationTimer = 0;
        }
    }

    protected virtual void HandlePoiseResetTimer()
    {
        if(poiseResetTimer > 0)
        {
            poiseResetTimer -= Time.deltaTime;
        }
        else
        {
            totalPoiseDamage = 0;
        }
    }
}
