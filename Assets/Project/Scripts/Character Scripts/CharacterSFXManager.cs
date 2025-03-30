using UnityEngine;

public class CharacterSFXManager : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Damage Grunts")]
    [SerializeField] protected AudioClip[] damageGrunts;

    [Header("Attack Grunts")]
    [SerializeField] protected AudioClip[] attackGrunts;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySFX(AudioClip sfx, float volume = 1, bool randomizePitch = true, float pitchRandom = 0.1f)
    {
        audioSource.PlayOneShot(sfx, volume);

        audioSource.pitch = 1;

        if (randomizePitch)
        {
            audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
        }
    }

    public void PlayRollSFX()
    {
        audioSource.PlayOneShot(WorldSFXManager.instance.rollSFX);
    }

    public void PlayDamageGrunt()
    {
        if (damageGrunts.Length > 0)
            PlaySFX(WorldSFXManager.instance.ChooseRandomSFXFromArray(damageGrunts));
    }

    public virtual void PlayAttackGrunt()
    {
        if (attackGrunts.Length > 0)
            PlaySFX(WorldSFXManager.instance.ChooseRandomSFXFromArray(attackGrunts));
    }
}
