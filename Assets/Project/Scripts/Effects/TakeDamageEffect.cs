using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage")]
public class TakeDamageEffect : InstantCharacterEffect
{
    [Header("Character Causing Damage")]
    public CharacterManager characterCausingDamage;

    [Header("Damage")]
    public float physicalDamage = 0;
    public float magicDamage = 0;
    // public float fireDamage = 0;

    [Header("Final Damage")]
    private int finalDamageDealt = 0;

    [Header("Poise")]
    public float poiseDamage = 0;
    public bool poiseIsBroken = false;

    [Header("Animation")]
    public bool playDamageAnimation = true;
    public bool manuallySelectDamageAnimation = false;
    public string damageAnimation;

    [Header("Sound FX")]
    public bool willPlayDamageSFX = true;
    // public AudioClip elementalDamageSoundFX;

    [Header("Direction Damage Taken From")]
    public float angleHitFrom;
    public Vector3 contactPoint;

    public override void ProcessEffect(CharacterManager character)
    {
        if (character.characterNetworkManager.isInvulnerable.Value)
            return;

        base.ProcessEffect(character);

        if (character.isDead.Value)
            return;

        CalculateDamage(character);
        PLayDirectionalBasedDamageAnimation(character);
        PlayDamageSFX(character);
        PlayDamageVFX(character);
    }

    private void CalculateDamage(CharacterManager character)
    {
        if (!character.IsOwner)
            return;

        if(characterCausingDamage != null)
        {

        }

        finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage);

        if(finalDamageDealt <= 0)
        {
            finalDamageDealt = 1;
        }

        character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;
    }

    private void PlayDamageVFX(CharacterManager character)
    {
        character.characterEffectsManager.PlayBloodSplatterVFX(contactPoint);
    }

    private void PlayDamageSFX(CharacterManager character)
    {
        AudioClip physicalDamageSFX = WorldSFXManager.instance.ChooseRandomSFXFromArray(WorldSFXManager.instance.physicalDamageSFX);

        character.characterSFXManager.PlaySFX(physicalDamageSFX);
        character.characterSFXManager.PlayDamageGrunt();
    }

    private void PLayDirectionalBasedDamageAnimation(CharacterManager character)
    {
        if (!character.IsOwner)
            return;

        if (character.isDead.Value)
            return;

        poiseIsBroken = true;

        if (angleHitFrom >= 145 && angleHitFrom <= 180)
        {
            damageAnimation = character.characterAnimatorManager.hit_Backward_01;
        }
        else if (angleHitFrom <= -145 && angleHitFrom >= -180)
        {
            damageAnimation = character.characterAnimatorManager.hit_Backward_01;
        }
        else if (angleHitFrom >= -45 && angleHitFrom <= 45)
        {
            damageAnimation = character.characterAnimatorManager.hit_Forward_01;
        }
        else if (angleHitFrom >= -144 && angleHitFrom <= -45)
        {
            damageAnimation = character.characterAnimatorManager.hit_Left_01;
        }
        else if (angleHitFrom >= 45 && angleHitFrom <= 144)
        {
            damageAnimation = character.characterAnimatorManager.hit_Right_01;
        }

        if (poiseIsBroken)
        {
            character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, true);
        }
    }
}
