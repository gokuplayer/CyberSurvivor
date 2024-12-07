using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject EnemyParent;
    public GameObject WinScreen;
    public GameObject DeathOverlay;
    public GameObject PauseMenuObject;
    public GameObject StartMenuButton;
    public GameObject ResumeButton;
    public GameObject CharacterSelectScreen;
    public GameObject WeaponSelectScreen;
    public GameObject WeaponDisplay;
    public GameObject ItemDisplay;
    public GameObject PlayerObject;
    public GameObject PlayerParent;
    public GameObject PauseSubmenuObject;
    public GameObject OptionsMenuObject;
    public int MaxSpawn;
    public int WaveNumber;
    public int WeaponIndex;
    public float MaxTimer;
    public float Rate;
    public float collectionSpeed;
    public float playerRecovery;
    public Text GameTimerText;
    public Text WaveNumberText;
    public Text CreditsNumberText;
    public Text PlayerLevelText;
    public Slider XP_Slider;
    public bool CharChosen;
    public bool PausedGame;
    public bool timerOn;
    public bool GameWin;
    public AudioMixer MainMixer;
    public Camera mainCamera;

    public Toggle fullScreenToggle;
    public TMP_Dropdown qualityDropdown;
    public Slider volumeSlider;

    [Header("Map 1 Enemy Waves")]
    public GameObject[] M1EnemyWave1;
    public GameObject[] M1EnemyWave2, M1EnemyWave3, M1EnemyWave4, M1EnemyWave5, M1EnemyWave6, M1EnemyWave7, M1EnemyWave8, M1EnemyWave9, M1EnemyWave10;

    [Header("Map 2 Enemy Waves")]
    public GameObject[] M2EnemyWave1;
    public GameObject[] M2EnemyWave2, M2EnemyWave3, M2EnemyWave4, M2EnemyWave5, M2EnemyWave6, M2EnemyWave7, M2EnemyWave8, M2EnemyWave9, M2EnemyWave10;

    public GameObject[] Bosses;
    public GameObject[] CharacterList;
    public GameObject[] AllWeapons;
    public GameObject[] WeaponSelectButtons;
    public Image[] WeaponDisplayImages;
    public GameObject[] AllItems;
    public GameObject[] ItemSelectButtons;

    private float spawnTimer;
    private float gameTimer;
    private float PlayerObjectHP;
    private float PlayerObjectXP;
    private float PlayerObjectNeededXP;
    private float playerSpeed;
    private int mapSelect;
    private bool newWave;
    private bool endGame;
    private GameObject[] enemyChildren;
    private List<float> enemySpeeds;
    private List<int> enemyAttacks;
    private List<float> weaponSpawnRates;

    void Awake()
    {
        GameInitialize();
    }

    void Start()
    {
        spawnTimer = Rate;
        gameTimer = 0f;
        WeaponIndex = 0;
        MaxSpawn = 50;
        newWave = true;
        endGame = false;
        GameWin = false;
        DeathOverlay = GameObject.Find("DeathOverlay");
        WinScreen = GameObject.Find("WinScreen");
        StartMenuButton = GameObject.Find("StartMenuButton");
        CharacterSelectScreen = GameObject.Find("CharacterSelect");
        WeaponSelectScreen = GameObject.Find("WeaponSelect");
        PauseMenuObject = GameObject.Find("PauseMenu");
        ResumeButton = GameObject.Find("ResumeButton");
        PauseSubmenuObject = GameObject.Find("PauseSubmenu");
        OptionsMenuObject = GameObject.Find("OptionsMenu");
        DeathOverlay.SetActive(false);
        WinScreen.SetActive(false);
        StartMenuButton.SetActive(false);
        WeaponSelectScreen.SetActive(false);
        PauseMenuObject.SetActive(false);
        ResumeButton.SetActive(false);

        PauseSubmenuObject.SetActive(true);
        OptionsMenuObject.SetActive(false);

        PausedGame = false;
    }

    void Update()
    {
        if (CharChosen)
        {
            PlayerObjectHP = PlayerObject.GetComponent<ICharacter>().CharacterHP;
            Debug.Log("Player HP: " + PlayerObjectHP.ToString());
            PlayerObjectXP = PlayerObject.GetComponent<ICharacter>().CharacterXP;
            PlayerObjectNeededXP = PlayerObject.GetComponent<ICharacter>().NeededXPToLevel;
            PlayerLevelText.text = "Level: " + PlayerObject.GetComponent<ICharacter>().CharacterLevel.ToString();

            XP_Slider.value = (PlayerObjectXP / PlayerObjectNeededXP) * 100;

            if (gameTimer % 60 < 0.1f && newWave)
            {
                newWave = false;
                WaveNumber++;
                updateWaveNumber(WaveNumber);

                if(WaveNumber == 6)
                {
                    Debug.Log("Boss spawned...");
                    switch (mapSelect)
                    {
                        case 0:
                            Instantiate(Bosses[0], new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                            break;
                        case 1:
                            Instantiate(Bosses[3], new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                            break;
                        default:
                            Instantiate(Bosses[0], new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                            break;
                    }
                }
            }
            else if (gameTimer % 60 > 0.1f)
            {
                newWave = true;
            }

            if (timerOn)
            {
                spawnTimer -= Time.deltaTime;

                if (spawnTimer <= 0f)
                {
                    if(EnemyParent.transform.childCount <= MaxSpawn)
                    {
                        switch (mapSelect)
                        {
                            case 0:
                                switch (WaveNumber)
                                {
                                    case 1:
                                        foreach (var spawnObject in M1EnemyWave1)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                    case 2:
                                        foreach (var spawnObject in M1EnemyWave2)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                    case 3:
                                        foreach (var spawnObject in M1EnemyWave3)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                    case 4:
                                        foreach (var spawnObject in M1EnemyWave4)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                    case 5:
                                        foreach (var spawnObject in M1EnemyWave5)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                    case 6:
                                        foreach (var spawnObject in M1EnemyWave6)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                    case 7:
                                        foreach (var spawnObject in M1EnemyWave7)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                    case 8:
                                        foreach (var spawnObject in M1EnemyWave8)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                    case 9:
                                        foreach (var spawnObject in M1EnemyWave9)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                    case 10:
                                        foreach (var spawnObject in M1EnemyWave10)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                    default:
                                        foreach (var spawnObject in M1EnemyWave1)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                }
                                break;
                            case 1:
                                switch (WaveNumber)
                                {
                                    case 1:
                                        foreach (var spawnObject in M2EnemyWave1)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                    case 2:
                                        foreach (var spawnObject in M2EnemyWave2)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                    case 3:
                                        foreach (var spawnObject in M2EnemyWave3)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                    case 4:
                                        foreach (var spawnObject in M2EnemyWave4)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                    case 5:
                                        foreach (var spawnObject in M2EnemyWave5)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                    case 6:
                                        foreach (var spawnObject in M2EnemyWave6)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                    case 7:
                                        foreach (var spawnObject in M2EnemyWave7)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                    case 8:
                                        foreach (var spawnObject in M2EnemyWave8)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                    case 9:
                                        foreach (var spawnObject in M2EnemyWave9)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                    case 10:
                                        foreach (var spawnObject in M2EnemyWave10)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                    default:
                                        foreach (var spawnObject in M2EnemyWave1)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                }
                                break;
                            default:
                                switch (WaveNumber)
                                {
                                    case 1:
                                        foreach (var spawnObject in M1EnemyWave1)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                    case 2:
                                        foreach (var spawnObject in M1EnemyWave2)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                    case 3:
                                        foreach (var spawnObject in M1EnemyWave3)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                    case 4:
                                        foreach (var spawnObject in M1EnemyWave4)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                    case 5:
                                        foreach (var spawnObject in M1EnemyWave5)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                    case 6:
                                        foreach (var spawnObject in M1EnemyWave6)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                    case 7:
                                        foreach (var spawnObject in M1EnemyWave7)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                    case 8:
                                        foreach (var spawnObject in M1EnemyWave8)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                    case 9:
                                        foreach (var spawnObject in M1EnemyWave9)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                    case 10:
                                        foreach (var spawnObject in M1EnemyWave10)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                    default:
                                        foreach (var spawnObject in M1EnemyWave1)
                                        {
                                            Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                        }
                                        break;
                                }
                                break;
                        }
                    }

                    spawnTimer = Rate;
                }

                if(gameTimer % 30 < 0.1f)
                {
                    foreach(Transform enemy in EnemyParent.transform)
                    {
                        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(enemy.position);

                        // Check if the enemy is outside the camera view
                        if (viewportPosition.x < 0f || viewportPosition.x > 1f || viewportPosition.y < 0f || viewportPosition.y > 1f)
                        {
                            // Move the enemy using GetModifier()
                            enemy.position = new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier(), enemy.position.z);
                        }
                    }
                }

                if (PlayerObjectHP <= 0) //if player is dead
                {
                    DeathOverlay.SetActive(true);
                    StartMenuButton.SetActive(true);
                    PlayerObject.GetComponent<ICharacter>().CharacterSpeed = 0f;
                    PlayerObject.transform.Find("CollectionRadius").GetComponent<CollectionRadiusScript>().TranslateSpeed = 0f;
                    foreach (Transform enemy in EnemyParent.transform)
                    {
                        enemy.GetComponent<IEnemy>().EnemySpeed = 0f;
                    }

                    foreach (Transform weaponSpawner in PlayerObject.transform)
                    {
                        if (weaponSpawner.GetComponent<ISpawner>() != null)
                        {
                            weaponSpawner.GetComponent<ISpawner>().spawnRate = 0f;
                        }
                    }
                    timerOn = false;
                    //Cursor.visible = true;
                }
                else //if player is alive
                {
                    //Cursor.visible = false;

                    CreditsNumberText.text = "Credits: " + SaveManager.currentCredits;

                    //game pause
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        if (!PausedGame)
                        {
                            Debug.Log("Paused the game.");
                            PauseMenuObject.SetActive(true);
                            StartMenuButton.SetActive(true);
                            ResumeButton.SetActive(true);
                            timerOn = false;
                            //Cursor.visible = true;
                            PausedGame = true;
                            playerSpeed = PlayerObject.GetComponent<ICharacter>().CharacterSpeed;
                            playerRecovery = PlayerObject.GetComponent<ICharacter>().CharacterHPRecovery;
                            PlayerObject.GetComponent<ICharacter>().CharacterSpeed = 0f;
                            PlayerObject.GetComponent<ICharacter>().CharacterHPRecovery = 0f;
                            collectionSpeed = PlayerObject.transform.Find("CollectionRadius").GetComponent<CollectionRadiusScript>().TranslateSpeed;
                            PlayerObject.transform.Find("CollectionRadius").GetComponent<CollectionRadiusScript>().TranslateSpeed = 0f;

                            enemySpeeds = new List<float>();
                            enemyAttacks = new List<int>();
                            foreach(Transform enemy in EnemyParent.transform)
                            {
                                enemySpeeds.Add(enemy.GetComponent<IEnemy>().EnemySpeed);
                                enemyAttacks.Add(enemy.GetComponent<IEnemy>().EnemyAttack);
                                if(enemy.GetComponent<IEnemy>().EnemyStatus != "Stunned")
                                {
                                    enemy.GetComponent<IEnemy>().EnemySpeed = 0f;
                                }
                                enemy.GetComponent<IEnemy>().EnemyAttack = 0;
                            }

                            foreach(Transform weaponSpawner in PlayerObject.transform)
                            {
                                if(weaponSpawner.GetComponent<ISpawner>() != null)
                                {
                                    weaponSpawner.GetComponent<ISpawner>().spawnRate = 0f;
                                }
                            }
                        }
                    }
                }
                if(gameTimer >= MaxTimer)
                {
                    if (!endGame)
                    {
                        foreach (Transform enemies in EnemyParent.transform)
                        {
                            Destroy(enemies.gameObject);
                        }

                        switch (mapSelect)
                        {
                            case 0:
                                Instantiate(Bosses[1], new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                break;
                            case 1:
                                Instantiate(Bosses[4], new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                break;
                            default:
                                Instantiate(Bosses[1], new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                                break;
                        }
                        endGame = true;
                    }

                    if(endGame && GameWin)
                    {
                        Debug.Log("You survived!");
                        timerOn = false;

                        StartMenuButton.SetActive(true);
                        WinScreen.SetActive(true);

                        //Cursor.visible = true;
                        PlayerObject.GetComponent<ICharacter>().CharacterSpeed = 0f;
                        foreach (Transform enemies in EnemyParent.transform)
                        {
                            Destroy(enemies.gameObject);
                        }

                        foreach (Transform weaponSpawner in PlayerObject.transform)
                        {
                            if (weaponSpawner.GetComponent<ISpawner>() != null)
                            {
                                weaponSpawner.GetComponent<ISpawner>().spawnRate = 0f;
                            }
                        }

                        if(SaveManager.currentMapsWon < 2)
                        {
                            SaveManager.currentMapsWon += 1;
                        }
                    }
                }

                Debug.Log("counting up");
                gameTimer += Time.deltaTime;
                updateTimer(gameTimer);
            }
        }
    }

    void updateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        GameTimerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    void updateWaveNumber(int wave)
    {
        WaveNumberText.text = "Wave: " + wave.ToString();
    }

    float GetModifier()
    {
        float modifier = Random.Range(5f, 10f);
        if(Random.Range(0, 2) > 0)
        {
            return -modifier;
        }
        else
        { 
            return modifier;
        }
    }

    // Game Start

    private void GameInitialize()
    {
        int charIndex = ButtonManager.CharSelectNumber;
        mapSelect = ButtonManager.MapSelectNumber;

        Instantiate(CharacterList[charIndex], new Vector3(0, 0, 0), Quaternion.identity);
        PlayerObject = GameObject.FindWithTag("Player");
        PlayerObject.transform.parent = PlayerParent.transform;
        EnemyParent = GameObject.FindWithTag("EnemySpawner");
        WeaponDisplay = GameObject.Find("WeaponDisplay");
        ItemDisplay = GameObject.Find("ItemDisplay");

        WeaponDisplay.SetActive(true);
        ItemDisplay.SetActive(true);

        switch (mapSelect)
        {
            case 0:
                foreach (var spawnObject in M1EnemyWave1)
                {
                    Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                }
                break;
            case 1:
                foreach (var spawnObject in M2EnemyWave1)
                {
                    Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                }
                break;
            default:
                foreach (var spawnObject in M1EnemyWave1)
                {
                    Instantiate(spawnObject, new Vector3(EnemyParent.transform.position.x + GetModifier(), EnemyParent.transform.position.y + GetModifier()), Quaternion.identity, EnemyParent.transform);
                }
                break;
        }

        Instantiate(WeaponDisplayImages[charIndex], WeaponDisplay.transform.GetChild(0));

        CharChosen = true;
        timerOn = true;
    }

    //Reset level up screen

    public void RemoveWeaponSelectButtons()
    {
        GameObject weapon1Parent = GameObject.Find("Weapon1");
        GameObject weapon2Parent = GameObject.Find("Weapon2");
        GameObject weapon3Parent = GameObject.Find("Weapon3");
        foreach (Transform child in weapon1Parent.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in weapon2Parent.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in weapon3Parent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    //Debug Functions

    public void LevelDebug()
    {
        PlayerObject.GetComponent<ICharacter>().CharacterXP += (PlayerObjectNeededXP + 1);
    }

    public void CreditsDebug()
    {
        SaveManager.currentCredits += 100;
    }

    public void WinDebug()
    {
        Debug.Log("You survived!");
        timerOn = false;

        StartMenuButton.SetActive(true);
        WinScreen.SetActive(true);
        PlayerObject.GetComponent<ICharacter>().CharacterSpeed = 0f;
        foreach (Transform enemies in EnemyParent.transform)
        {
            Destroy(enemies.gameObject);
        }

        foreach (Transform weaponSpawner in PlayerObject.transform)
        {
            if (weaponSpawner.GetComponent<ISpawner>() != null)
            {
                weaponSpawner.GetComponent<ISpawner>().spawnRate = 0f;
            }
        }

        if (SaveManager.currentMapsWon < 2)
        {
            SaveManager.currentMapsWon += 1;
        }
    }

    //Pause Menu Functions

    public void UnPause()
    {
        Debug.Log("Unpaused the game.");
        PauseMenuObject.SetActive(false);
        StartMenuButton.SetActive(false);
        ResumeButton.SetActive(false);
        timerOn = true;
        //Cursor.visible = false;
        PausedGame = false;
        PlayerObject.GetComponent<ICharacter>().CharacterSpeed = playerSpeed;
        PlayerObject.transform.Find("CollectionRadius").GetComponent<CollectionRadiusScript>().TranslateSpeed = collectionSpeed;
        PlayerObject.GetComponent<ICharacter>().CharacterHPRecovery = playerRecovery;
        for (int i = 0; i < EnemyParent.transform.childCount; i++)
        {
            GameObject enemyChildObject = EnemyParent.transform.GetChild(i).gameObject;
            if(enemyChildObject.GetComponent<IEnemy>().EnemyStatus != "Stunned")
            {
                enemyChildObject.GetComponent<IEnemy>().EnemySpeed = enemySpeeds[i];
            }
            enemyChildObject.GetComponent<IEnemy>().EnemyAttack = enemyAttacks[i];
        }
    }

    public void StartMenuSwap()
    {
        GameObject ButtonManagerObject = GameObject.Find("Button Manager");
        if(ButtonManagerObject != null)
        {
            ButtonManagerObject.GetComponent<SaveManager>().SavePlayerData();
            Destroy(ButtonManagerObject);
        }
        SceneManager.UnloadSceneAsync("MapScene");
        SceneManager.LoadScene("StartMenu");
    }

    //Options Menu Functions

    public void OptionsMenu()
    {
        PauseSubmenuObject.SetActive(false);
        StartMenuButton.SetActive(false);
        OptionsMenuObject.SetActive(true);

        if(fullScreenToggle != null)
        {
            fullScreenToggle.isOn = Screen.fullScreen;
        }
        if(qualityDropdown != null)
        {
            qualityDropdown.value = QualitySettings.GetQualityLevel();
        }
        if(volumeSlider != null)
        {
            float volVal;
            MainMixer.GetFloat("volume", out volVal);
            volumeSlider.value = volVal;
        }
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Debug.Log("Fullscreen: " + isFullscreen.ToString());

        Screen.fullScreen = isFullscreen;
    }

    public void SetQuality(int qualityIndex)
    {
        Debug.Log("Quality Level: " + qualityIndex.ToString());

        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetVolume(float volume)
    {
        Debug.Log("Volume Level: " + volume.ToString());

        MainMixer.SetFloat("volume", volume);
    }

    public void OptionsBack()
    {
        PauseSubmenuObject.SetActive(true);
        StartMenuButton.SetActive(true);
        OptionsMenuObject.SetActive(false);
    }
}