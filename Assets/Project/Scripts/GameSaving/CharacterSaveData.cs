using UnityEngine;

[System.Serializable]
public class CharacterSaveData
{
    [Header("Character Name")]
    public string characterName = "Character";

    [Header("Time Played")]
    public float secondsPlayed;

    [Header("World Coordinates")]
    public float xPosition;
    public float yPosition;
    public float zPosition;

    [Header("Stats")]
    public int vitality;
    public int endurance;

    [Header("Resources")]
    public int currentHealth;
    public float currentStamina;
}
