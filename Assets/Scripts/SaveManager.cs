using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public AudioMixer MainMixer;
    public float SavedVolume;
    public int SavedQuality;
    public bool SavedFullScreen;

    public Toggle fullScreenToggle;
    public Dropdown qualityDropdown;
    public Slider volumeSlider;

    private string saveFilePath;
    private bool currentFullScreen;
    private int currentQuality;
    private float currentVolume;

    public int SavedCredits;
    public static int currentCredits;

    public static int SavedMapsWon;
    public static int currentMapsWon;

    // Player data structure
    [System.Serializable]
    public class PlayerData
    {
        public bool gameFullScreen;
        public int gameQuality;
        public float gameVolume;
        public int gameCredits;
        public int gameMapsWon;
    }

    private PlayerData playerData;

    //Scene check variables
    private Scene currentScene;
    private string sceneName;

    private void Awake()
    {
        saveFilePath = Application.persistentDataPath + "/saveData.json";
        LoadPlayerData();
    }

    public void SaveOptions()
    {
        SavePlayerData();

        Debug.Log("Saved Options");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SavePlayerData()
    {
        PlayerData data = new PlayerData();
        currentFullScreen = Screen.fullScreen;
        currentQuality = QualitySettings.GetQualityLevel();
        MainMixer.GetFloat("volume", out currentVolume);
        data.gameVolume = currentVolume;
        data.gameFullScreen = currentFullScreen;
        data.gameQuality = currentQuality;
        data.gameCredits = currentCredits;
        data.gameMapsWon = currentMapsWon;

        string jsonData = JsonUtility.ToJson(data);

        saveFilePath = Application.persistentDataPath + "/saveData.json";

        File.WriteAllText(saveFilePath, jsonData);
    }

    private void LoadPlayerData()
    {
        if (File.Exists(saveFilePath))
        {
            // Read saved data from the file
            string jsonData = File.ReadAllText(saveFilePath);

            // Deserialize JSON data into player data structure
            playerData = JsonUtility.FromJson<PlayerData>(jsonData);

            SavedFullScreen = playerData.gameFullScreen;
            Debug.Log(SavedFullScreen.ToString());
            SavedQuality = playerData.gameQuality;
            Debug.Log(SavedQuality.ToString());
            SavedVolume = playerData.gameVolume;
            Debug.Log(SavedVolume.ToString());

            SavedCredits = playerData.gameCredits;
            Debug.Log("Loaded Credits: " + SavedCredits.ToString());

            SavedMapsWon = playerData.gameMapsWon;
            Debug.Log("Maps Won: " + SavedMapsWon.ToString());
        }
        else
        {
            Debug.Log("Save file not found. Starting with default player data.");
            playerData = new PlayerData();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;

        Screen.fullScreen = SavedFullScreen;
        QualitySettings.SetQualityLevel(SavedQuality);
        MainMixer.SetFloat("volume", SavedVolume);

        currentCredits = SavedCredits;
    }
}
