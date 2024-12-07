using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ButtonManager : MonoBehaviour
{
    public static int CharSelectNumber;
    public static int MapSelectNumber;
    public TextMeshProUGUI CharNameText;
    public TextMeshProUGUI HPText;
    public TextMeshProUGUI AtkText;
    public TextMeshProUGUI SpdText;
    public TextMeshProUGUI CritText;
    public TextMeshProUGUI WeaponNameText;
    public TextMeshProUGUI WeaponDescText;
    public TextMeshProUGUI AbilityDescText;
    public TextMeshProUGUI MapNameText;
    public Text CreditsNumText;
    public GameObject MapForwardButton;
    public GameObject MapBackwardButton;
    public Sprite[] CharSprites;
    public Sprite[] WeaponSprites;
    public Sprite[] MapSprites;

    [SerializeField] private GameObject GameManagerObject;
    [SerializeField] private GameObject PlayerObject;
    private GameObject EnemyParent;
    private GameManager gameManager;
    private Transform playerTransform;
    private Transform weaponDisplay;
    private Transform itemDisplay;
    private int weaponsSpawned;
    private int weaponLevel;
    private int itemLevel;
    private const int MaxWeapons = 6;

    //Scene check variables
    private Scene currentScene;
    private string sceneName;

    Dictionary<int, int> CharHPDict = new Dictionary<int, int>()
    {
        {0, 100},
        {1, 80},
        {2, 120},
        {3, 110},
        {4, 90},
        {5, 100},
    };

    Dictionary<int, string> CharAtkDict = new Dictionary<int, string>()
    {
        {0, "1.1x"},
        {1, "0.9x"},
        {2, "1.5x"},
        {3, "1.0x"},
        {4, "1.3x"},
        {5, "1.1x"},
    };

    Dictionary<int, string> CharSpdDict = new Dictionary<int, string>()
    {
        {0, "4.0"},
        {1, "6.0"},
        {2, "3.0"},
        {3, "5.0"},
        {4, "4.5"},
        {5, "4.0"},
    };

    Dictionary<int, string> CharCritDict = new Dictionary<int, string>()
    {
        {0, "5%"},
        {1, "7%"},
        {2, "4%"},
        {3, "10%"},
        {4, "4.5%"},
        {5, "5%"},
    };

    Dictionary<int, string> CharNameDict = new Dictionary<int, string>()
    {
        {0, "Corrupt Cop"},
        {1, "Runner"},
        {2, "Smasher"},
        {3, "Samurai"},
        {4, "Sniper Elite"},
        {5, "Cyberneer"},
    };

    Dictionary<int, string> WeaponNameDict = new Dictionary<int, string>()
    {
        {0, "Baton"},
        {1, "Pistol"},
        {2, "Minigun"},
        {3, "Katana"},
        {4, "Sniper"},
        {5, "Drone"},
    };

    Dictionary<int, string> WeaponDescDict = new Dictionary<int, string>()
    {
        {0, "Attacks in front, passes through enemies."},
        {1, "Basic handgun projectile."},
        {2, "Low damage, fast firing projectiles."},
        {3, "High crit, high damage."},
        {4, "High damage, fast projectile."},
        {5, "Construct that fires high damage projectiles."},
    };

    Dictionary<int, string> AbilityDescDict = new Dictionary<int, string>()
    {
        {0, "Gains 10% more damage every 10 levels (max +50%)."},
        {1, "Gains 10% more speed every 10 levels (max +50%)."},
        {2, "Gains 0.1 more recovery every 10 levels (max +0.5)."},
        {3, "Gains 5% more crit rate every 10 levels (max +25%)."},
        {4, "Gains 5% more crit damage every 10 levels (max +25%)."},
        {5, "Gains 5% more XP gain every 10 levels (max +25%)."},
    };

    Dictionary<int, string> MapNameDict = new Dictionary<int, string>()
    {
        {0, "Map 1"},
        {1, "Map 2"},
    };

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        Debug.Log("Button Manager Start");
        SceneManager.sceneLoaded += OnSceneLoaded;

        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;

        if(sceneName == "StartMenu")
        {
            CreditsNumText.text = "Credits: " + SaveManager.currentCredits;
            MapBackwardButton.SetActive(false);
            MapSelectNumber = 0;

            if(SaveManager.SavedMapsWon == 0)
            {
                MapForwardButton.SetActive(false);
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;

        if (sceneName == "MapScene")
        {
            Debug.Log("Scene is Map");
            weaponsSpawned = 1;
            StartCoroutine(WaitForCharChosen());
        }

        Debug.Log("Char select: " + CharSelectNumber.ToString());
        Debug.Log("Char select: " + MapSelectNumber.ToString());
    }

    void Update()
    {
        Debug.Log("Current Scene: " + sceneName.ToString());
        Debug.Log("Char select: " + CharSelectNumber.ToString());
        Debug.Log("Char select: " + MapSelectNumber.ToString());
        if (sceneName == "MapScene")
        {
            if (weaponDisplay != null)
            {
                foreach (Transform weaponImage in weaponDisplay)
                {
                    if (weaponImage.childCount > 0)
                    {
                        string weaponWanted = weaponImage.GetChild(0).name.Replace("Image(Clone)", "Spawner");
                        Debug.Log("Weapon wanted: " + weaponWanted);
                        foreach (Transform playerChild in PlayerObject.transform)
                        {
                            if (playerChild.name == weaponWanted)
                            {
                                Debug.Log("Weapon wanted found");
                                weaponLevel = playerChild.GetComponent<ISpawner>().s_Level;
                            }
                        }
                        weaponImage.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = weaponLevel.ToString();
                    }
                }
            }
            if (itemDisplay != null)
            {
                foreach (Transform itemImage in itemDisplay)
                {
                    if (itemImage.childCount > 0)
                    {
                        string itemWanted = itemImage.GetChild(0).name;
                        Debug.Log("Item wanted: " + itemWanted);
                        if (itemImage.GetChild(0).name == itemWanted)
                        {
                            Debug.Log("Item wanted found");
                            itemLevel = itemImage.GetChild(0).GetComponent<IPassive>().ILevel;
                        }
                        itemImage.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = itemLevel.ToString();
                    }
                }
            }
        }
    }

    //For StartMenu Scene

    public void CharacterSelect(int charIndex)
    {
        CharSelectNumber = charIndex;

        CharNameText.text = CharNameDict[charIndex];
        HPText.text = CharHPDict[charIndex].ToString();
        AtkText.text = CharAtkDict[charIndex];
        SpdText.text = CharSpdDict[charIndex];
        CritText.text = CharCritDict[charIndex];

        WeaponNameText.text = WeaponNameDict[charIndex];
        WeaponDescText.text = WeaponDescDict[charIndex];
        AbilityDescText.text = AbilityDescDict[charIndex];

        GameObject charSpriteObject = GameObject.Find("CharSprite");
        GameObject weaponSpriteObject = GameObject.Find("WeaponImage");

        if(charSpriteObject != null)
        {
            Image charSpriteImage = charSpriteObject.GetComponent<Image>();

            if(charSpriteImage != null)
            {
                charSpriteImage.sprite = CharSprites[charIndex];
            }
        }

        if (weaponSpriteObject != null)
        {
            Image weaponSpriteImage = weaponSpriteObject.GetComponent<Image>();

            if (weaponSpriteImage != null)
            {
                weaponSpriteImage.sprite = WeaponSprites[charIndex];
            }
        }
    }

    public void MapForward()
    {
        Debug.Log("Map Select: " + MapSelectNumber.ToString());
        MapSelectNumber += 1;
        if (MapSelectNumber >= MapSprites.Length - 1)
        {
            MapForwardButton.SetActive(false);
        }
        else
        {
            MapForwardButton.SetActive(true);
        }

        MapNameText.text = MapNameDict[MapSelectNumber];
        GameObject mapSpriteObject = GameObject.Find("MapSprite");
        MapBackwardButton.SetActive(true);

        if (mapSpriteObject != null)
        {
            Image mapSpriteImage = mapSpriteObject.GetComponent<Image>();

            if (mapSpriteImage != null)
            {
                mapSpriteImage.sprite = MapSprites[MapSelectNumber];
            }
        }
    }

    public void MapBackward()
    {
        Debug.Log("Map Select: " + MapSelectNumber.ToString());
        MapSelectNumber -= 1;
        if (MapSelectNumber <= 0)
        {
            MapBackwardButton.SetActive(false);
        }
        else
        {
            MapBackwardButton.SetActive(true);
        }

        MapNameText.text = MapNameDict[MapSelectNumber];
        GameObject mapSpriteObject = GameObject.Find("MapSprite");
        MapForwardButton.SetActive(true);

        if (mapSpriteObject != null)
        {
            Image mapSpriteImage = mapSpriteObject.GetComponent<Image>();

            if (mapSpriteImage != null)
            {
                mapSpriteImage.sprite = MapSprites[MapSelectNumber];
            }
        }
    }

    //For Game Scenes

    public void WeaponButtonAdd(int weaponChosen)
    {
        bool weaponFound = false;
        Debug.Log("Weapon Button pressed");
        GameManagerObject = GameObject.Find("Game Manager");
        gameManager = GameManagerObject.GetComponent<GameManager>();
        PlayerObject = GameObject.Find("PlayerParent").transform.GetChild(0).transform.gameObject;
        playerTransform = PlayerObject.transform;
        EnemyParent = GameObject.FindWithTag("EnemySpawner");
        gameManager.RemoveWeaponSelectButtons();
        gameManager.WeaponSelectScreen.SetActive(false);

        //Cursor.visible = false;

        gameManager.PausedGame = false;
        gameManager.timerOn = true;
        PlayerObject.GetComponent<ICharacter>().CharacterSpeed = PlayerObject.GetComponent<ICharacter>().TempSpeed;
        PlayerObject.transform.Find("CollectionRadius").GetComponent<CollectionRadiusScript>().TranslateSpeed = PlayerObject.GetComponent<ICharacter>().TempCollectSpeed;

        for (int i = 0; i < EnemyParent.transform.childCount; i++)
        {
            GameObject enemyChildObject = EnemyParent.transform.GetChild(i).gameObject;
            enemyChildObject.GetComponent<IEnemy>().EnemySpeed = PlayerObject.GetComponent<ICharacter>().enemySpeeds[i];
        }

        if (weaponsSpawned >= MaxWeapons)
        {
            Debug.Log("Weapons Spawned: " + weaponsSpawned.ToString());
        }

        weaponDisplay = gameManager.WeaponDisplay.transform;
        Debug.Log("Not at max weapons.");

        foreach (Transform child in weaponDisplay)
        {
            foreach (Transform playerChild in playerTransform)
            {
                Debug.Log("playerchild found");
                if (playerChild.name == GameManagerObject.GetComponent<GameManager>().AllWeapons[weaponChosen].name && !weaponFound)
                {
                    Debug.Log("leveling spawner...");
                    weaponFound = true;
                    playerChild.GetComponent<ISpawner>().s_Level++;
                    Debug.Log("Spawner level: " + playerChild.GetComponent<ISpawner>().s_Level.ToString());
                    break;
                }
                else if (playerChild.name == GameManagerObject.GetComponent<GameManager>().AllWeapons[weaponChosen].name + "(Clone)" && !weaponFound)
                {
                    Debug.Log("adding leveling with clone name");
                    weaponFound = true;
                    playerChild.GetComponent<ISpawner>().s_Level++;
                    Debug.Log("Spawner level: " + playerChild.GetComponent<ISpawner>().s_Level.ToString());
                    break;
                }
                continue;
            }

            if (child.childCount != 0)
            {
                Debug.Log("Child count not 0");
                continue;
            }

            Debug.Log("Past child count");

            if (!weaponFound)
            {
                Debug.Log("adding spawner...");
                char[] CloneChar = { ')', 'e', 'n', 'o', 'l', 'C', '(' };
                Instantiate(GameManagerObject.GetComponent<GameManager>().WeaponDisplayImages[weaponChosen], child);
                GameObject newWeapon = Instantiate(GameManagerObject.GetComponent<GameManager>().AllWeapons[weaponChosen], GameManagerObject.GetComponent<GameManager>().PlayerObject.transform);
                newWeapon.name = newWeapon.name.TrimEnd(CloneChar);
                PlayerObject.GetComponent<ICharacter>().CharacterWeapons.Add(newWeapon);
                weaponsSpawned++;
                break;
            }
        }
    }

    public void ItemButtonAdd(int itemChosen)
    {
        bool itemFound = false;
        Debug.Log("Item Button pressed");
        GameManagerObject = GameObject.Find("Game Manager");
        gameManager = GameManagerObject.GetComponent<GameManager>();
        PlayerObject = GameObject.Find("PlayerParent").transform.GetChild(0).transform.gameObject;
        playerTransform = PlayerObject.transform;
        EnemyParent = GameObject.FindWithTag("EnemySpawner");
        gameManager.RemoveWeaponSelectButtons();
        gameManager.WeaponSelectScreen.SetActive(false);
        //Cursor.visible = false;

        itemDisplay = GameManagerObject.GetComponent<GameManager>().ItemDisplay.transform;

        gameManager.PausedGame = false;
        gameManager.timerOn = true;
        PlayerObject.GetComponent<ICharacter>().CharacterSpeed = PlayerObject.GetComponent<ICharacter>().TempSpeed;
        PlayerObject.transform.Find("CollectionRadius").GetComponent<CollectionRadiusScript>().TranslateSpeed = PlayerObject.GetComponent<ICharacter>().TempCollectSpeed;

        for (int i = 0; i < EnemyParent.transform.childCount; i++)
        {
            GameObject enemyChildObject = EnemyParent.transform.GetChild(i).gameObject;
            enemyChildObject.GetComponent<IEnemy>().EnemySpeed = PlayerObject.GetComponent<ICharacter>().enemySpeeds[i];
        }

        if (weaponsSpawned >= MaxWeapons)
        {
            Debug.Log("Weapons Spawned: " + weaponsSpawned.ToString());
        }

        foreach (Transform child in itemDisplay)
        {
            foreach (Transform itemChild in itemDisplay)
            {
                Debug.Log("itemchild found");
                if (itemChild.childCount != 0)
                {
                    if (itemChild.GetChild(0).name == GameManagerObject.GetComponent<GameManager>().AllItems[itemChosen].name && !itemFound)
                    {
                        Debug.Log("leveling item...");
                        itemFound = true;
                        itemChild.GetChild(0).GetComponent<IPassive>().ILevel++;
                        Debug.Log("Item level: " + itemChild.GetChild(0).GetComponent<IPassive>().ILevel.ToString());
                        itemChild.GetChild(0).GetComponent<IPassive>().ItemLevelUp();
                        break;
                    }
                    else if (itemChild.GetChild(0).name == GameManagerObject.GetComponent<GameManager>().AllItems[itemChosen].name + "(Clone)" && !itemFound)
                    {
                        Debug.Log("adding leveling with clone name");
                        itemFound = true;
                        itemChild.GetChild(0).GetComponent<IPassive>().ILevel++;
                        Debug.Log("Item level: " + itemChild.GetChild(0).GetComponent<IPassive>().ILevel.ToString());
                        itemChild.GetChild(0).GetComponent<IPassive>().ItemLevelUp();
                        break;
                    }
                }
                continue;
            }

            if (child.childCount != 0)
            {
                Debug.Log("Child count not 0");
                continue;
            }

            Debug.Log("Past child count");

            if (!itemFound)
            {
                Debug.Log("adding item...");
                char[] CloneChar = { ')', 'e', 'n', 'o', 'l', 'C', '(' };
                GameObject newItem = Instantiate(GameManagerObject.GetComponent<GameManager>().AllItems[itemChosen], child);
                newItem.name = newItem.name.TrimEnd(CloneChar);
                PlayerObject.GetComponent<ICharacter>().itemsSpawned++;
                break;
            }
        }
    }

    private IEnumerator WaitForCharChosen()
    {
        while (!GameObject.Find("Game Manager").GetComponent<GameManager>().CharChosen)
        {
            yield return null;
        }

        StartVariables();
    }

    private void StartVariables()
    {
        PlayerObject = GameObject.Find("PlayerParent").transform.GetChild(0).transform.gameObject;
        weaponDisplay = GameObject.Find("Game Manager").GetComponent<GameManager>().WeaponDisplay.transform;
        itemDisplay = GameObject.Find("Game Manager").GetComponent<GameManager>().ItemDisplay.transform;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}