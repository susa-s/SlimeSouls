using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class WorldSaveGameManager : MonoBehaviour
{
    public static WorldSaveGameManager instance;

    public PlayerManager player;

    [Header("Save/Load")]
    [SerializeField] bool saveGame;
    [SerializeField] bool loadGame;

    [Header("World Scene Index")]
    [SerializeField] int worldSceneIndex = 1;

    [Header("Save Data Writer")]
    private SaveFileDataWriter saveFileDataWriter;

    [Header("Current Character Data")]
    public CharacterSlot currentCharacterSlotBeingUsed;
    public CharacterSaveData currentCharacterData;
    private string saveFileName;

    [Header("CharacterSlots")]
    public CharacterSaveData characterSlot01;
    public CharacterSaveData characterSlot02;
    public CharacterSaveData characterSlot03;

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
        LoadAllCharacterProfiles();
    }

    private void Update()
    {
        if (saveGame)
        {
            saveGame = false;
            SaveGame();
        }

        if (loadGame)
        {
            loadGame = false;
            LoadGame();
        }
    }

    public string DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot characterSlot)
    {
        string fileName = "";
        switch (characterSlot)
        {
            case CharacterSlot.CharacterSlot_01:
                fileName = "characterSlot_01";
                break;
            case CharacterSlot.CharacterSlot_02:
                fileName = "characterSlot_02";
                break;
            case CharacterSlot.CharacterSlot_03:
                fileName = "characterSlot_03";
                break;
            default:
                break;
        }

        return fileName;
    }

    public void AttemptToCreateNewGame()
    {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);

        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_01;
            currentCharacterData = new CharacterSaveData();

            NewGame();
            return;
        }

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);

        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_02;
            currentCharacterData = new CharacterSaveData();

            NewGame();
            return;
        }
        
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);

        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_03;
            currentCharacterData = new CharacterSaveData();

            NewGame();
            return;
        }
        
        TitleScreenManager.Instance.DisplayNoFreeCharacterSlotsPopUp();
    }

    private void NewGame()
    {
        player.playerNetworkManager.vitality.Value = 20;
        player.playerNetworkManager.endurance.Value = 20;

        SaveGame();
        LoadWorldScene(worldSceneIndex);
    }

    public void LoadGame()
    {
        saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = saveFileName;
        currentCharacterData = saveFileDataWriter.LoadSaveFile();

        LoadWorldScene(worldSceneIndex);
    }

    public void SaveGame()
    {
        saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = saveFileName;

        player.SaveGameDataToCurrentCharacterData(ref currentCharacterData);

        saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);
    }

    public void DeleteGame(CharacterSlot characterSlot)
    {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

        saveFileDataWriter.DeleteSaveFile();
    }

    private void LoadAllCharacterProfiles()
    {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
        characterSlot01 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
        characterSlot02 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);
        characterSlot03 = saveFileDataWriter.LoadSaveFile();
    }

    public void LoadWorldScene(int buildIndex)
    {
        string worldScene = SceneUtility.GetScenePathByBuildIndex(buildIndex);
        NetworkManager.Singleton.SceneManager.LoadScene(worldScene, LoadSceneMode.Single);

        player.LoadGameDataFromCurrentCharacterData(ref currentCharacterData);
    }
    
    public int GetWorldSceneIndex()
    {
        return worldSceneIndex;
    }
}
