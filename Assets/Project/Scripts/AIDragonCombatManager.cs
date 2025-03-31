using UnityEngine;

public class AIDragonCombatManager : AICharacterCombatManager
{
    private ParticleSystem fireBreath;

    [Header("Damage Collider")]
    [SerializeField] DragonClawDamageCollider rightClawCollider;
    [SerializeField] DragonClawDamageCollider leftClawCollider;
    [SerializeField] DragonFireBreathDamageCollider fireBreathCollider;

    [Header("Damage")]
    [SerializeField] int baseDamage = 25;
    [SerializeField] float attack01DamageModifer = 1.0f;
    [SerializeField] float attack02DamageModifer = 1.4f;

    private void Start()
    {
        fireBreath = GetComponentInChildren<ParticleSystem>();
    }

    public void SetAttack01Damage()
    {
        rightClawCollider.physicalDamage = baseDamage * attack01DamageModifer;
        leftClawCollider.physicalDamage = baseDamage * attack01DamageModifer;
        fireBreathCollider.physicalDamage = baseDamage * attack02DamageModifer;
    }

    public void SetAttack02Damage()
    {

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
        
        if (viewableAngle >= 61 && viewableAngle <= 110)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("TurnR90", true);
        }
        else if (viewableAngle <= -61 && viewableAngle >= -110)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("TurnL90", true);
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
