using UnityEngine;

public class AIDragonCombatManager : AICharacterCombatManager
{
    AIBossCharacterManager aiBossManager;

    private ParticleSystem fireBreath;

    [Header("Damage Collider")]
    [SerializeField] DragonClawDamageCollider rightClawCollider;
    [SerializeField] DragonClawDamageCollider leftClawCollider;
    [SerializeField] DragonFireBreathDamageCollider fireBreathCollider;

    [Header("Damage")]
    [SerializeField] int baseDamage = 25;
    [SerializeField] int fireDamage = 30;
    [SerializeField] float attack01DamageModifer = 1.0f;
    [SerializeField] float attack02DamageModifer = 1.4f;
    [SerializeField] float attack03DamageModifer = 0.8f;

    protected override void Awake()
    {
        base.Awake();

        aiBossManager = GetComponent<AIBossCharacterManager>();
    }

    private void Start()
    {
        fireBreath = GetComponentInChildren<ParticleSystem>();
    }

    public void SetAttack01Damage()
    {
        rightClawCollider.physicalDamage = baseDamage * attack01DamageModifer;
        leftClawCollider.physicalDamage = baseDamage * attack02DamageModifer;
    }

    public void SetAttack02Damage()
    {
        fireBreathCollider.physicalDamage = fireDamage * attack02DamageModifer;
    }

    public void OpenRightClawCollider()
    {
        aiCharacter.characterSFXManager.PlayAttackGrunt();
        rightClawCollider.EnableDamageCollider();
    }

    public void OpenLeftCollider()
    {
        leftClawCollider.EnableDamageCollider();
    }

    public void OpenFireBreathCollider()
    {
        aiCharacter.characterSFXManager.PlayAttackGrunt();
        fireBreathCollider.EnableDamageCollider();
        //aiBossManager.characterSFXManager.PlaySFX(WorldSFXManager.instance.ChooseRandomSFXFromArray(whooshes));
    }

    public void CloseRightClawCollider()
    {
        rightClawCollider.DisableDamageCollider();
    }

    public void CloseLeftClawCollider()
    {
        leftClawCollider.DisableDamageCollider();
    }

    public void CloseFireBreathCollider()
    {
        fireBreathCollider.DisableDamageCollider();
    }

    public void EnableFireBreathParticles()
    {
        fireBreath.Play();
    }

    public void DisableFireBreathParticles()
    {
        fireBreath.Stop();
    }

    public override void PivotTowardsTarget(AICharacterManager aiCharacter)
    {
        base.PivotTowardsTarget(aiCharacter);

        if (aiCharacter.isPerformingAction)
            return;
        
        if (viewableAngle >= 20 && viewableAngle <= 60)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("TurnR45", true);
        }
        else if (viewableAngle <= -20 && viewableAngle >= -60)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("TurnL45", true);
        }
        else if (viewableAngle >= 61 && viewableAngle <= 110)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("TurnR90", true);
        }
        else if (viewableAngle <= -61 && viewableAngle >= -110)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("TurnL90", true);
        }
        else if (viewableAngle >= 110 && viewableAngle <= 145)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("TurnR135", true);
        }
        else if (viewableAngle <= -110 && viewableAngle >= -145)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("TurnL135", true);
        }
        else if (viewableAngle >= 146 && viewableAngle <= 180)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("TurnR180", true);
        }
        else if (viewableAngle <= -146 && viewableAngle >= -180)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("TurnL180", true);
        }
    }
}
