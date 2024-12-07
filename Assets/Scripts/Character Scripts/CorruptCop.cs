using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorruptCop : MonoBehaviour, ICharacter
{
    public string CharacterName { get; set; }
    public string StartingWeapon { get; set; }
    public float CharacterHP { get; set; }
    public float CharacterMaxHP { get; set; }
    public float CharacterXP { get; set; }
    public float CharacterSpeed { get; set; }
    public float CharacterHPPercent { get; set; }
    public float CharacterCritRate { get; set; }
    public float CharacterHPRecovery { get; set; }
    public float itemXPBoost { get; set; }
    public float NeededXPToLevel { get; set; }
    public float TempSpeed { get; set; }
    public float TempCollectSpeed { get; set; }
    public double CharacterAttackPower { get; set; }
    public int CharacterDefense { get; set; }
    public int CharacterLevel { get; set; }
    public int direction { get; set; }
    public int weaponsSpawned { get; set; }
    public int itemsSpawned { get; set; }
    public List<GameObject> CharacterWeapons { get; set; }
    public List<float> enemySpeeds { get; set; }

    public Transform HealthBar;
    public GameObject EnemyParent;
    public AudioClip hitClip;

    private Rigidbody2D playerBody;
    private float hInput;
    private float vInput;
    private float hpPercent;
    private float damageDelay;
    private float damageTimer;
    private float recoveryTimer;
    private int enemyDamage;
    private GameObject GameManager;
    private GameObject ItemDisplay;
    private GameObject WeaponDisplay;
    private AudioSource hitSource;

    void Awake()
    {
        CharacterName = "CorruptCop";

        CharacterMaxHP = 100;
        CharacterHP = CharacterMaxHP;
        CharacterSpeed = 5.0f;
        CharacterAttackPower = 1.1;
        CharacterHPRecovery = 0.0f;
        CharacterCritRate = 5.0f;
        playerBody = GetComponent<Rigidbody2D>();
        HealthBar = this.gameObject.transform.GetChild(0).GetChild(0);
        CharacterDefense = 0;
        CharacterLevel = 1;
        CharacterXP = 0f;
        damageDelay = 0.2f;
        damageTimer = 5f;
        recoveryTimer = 0.0f;
        itemXPBoost = 1f;
        NeededXPToLevel = 5f;
        direction = 1;
        weaponsSpawned = 0;
        itemsSpawned = 0;
        GameManager = GameObject.Find("Game Manager");
        ItemDisplay = GameManager.GetComponent<GameManager>().ItemDisplay;
        WeaponDisplay = GameManager.GetComponent<GameManager>().WeaponDisplay;
        EnemyParent = GameObject.Find("EnemySpawn");

        StartingWeapon = "BatonSpawner";

        CharacterWeapons = new List<GameObject>();

        GameObject StartingWeaponObject = GameManager.GetComponent<GameManager>().AllWeapons[0];
        if (StartingWeaponObject != null)
        {
            Debug.Log(StartingWeaponObject);
            CharacterWeapons.Add(StartingWeaponObject);
        }
        else
        {
            Debug.Log("Starting Weapon was null");
        }

        //Sound code
        hitSource = GetComponent<AudioSource>();
        if (hitSource == null)
        {
            // If AudioSource is not already attached, add it dynamically
            hitSource = gameObject.AddComponent<AudioSource>();
        }

        // Assign the AudioClip to the AudioSource
        hitSource.clip = hitClip;
    }

    void Update()
    {
        hInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");

        if (hInput > 0)
        {
            //Right
            direction = 1;
        }
        if (hInput < 0)
        {
            //Left
            direction = 2;
        }
        if (vInput > 0)
        {
            //Up
            direction = 3;
        }
        if (vInput < 0)
        {
            //Down
            direction = 4;
        }

        if (CharacterHPRecovery > 0f)
        {
            recoveryTimer += Time.deltaTime;

            if (recoveryTimer >= (1.0f / CharacterHPRecovery))
            {
                CharacterHP += 1;

                CharacterHP = Mathf.Clamp(CharacterHP, 0, CharacterMaxHP);

                HealthBarChange();

                recoveryTimer = 0.0f;
            }
        }
    }

    private void FixedUpdate()
    {
        playerBody.velocity = new Vector2(hInput * CharacterSpeed, vInput * CharacterSpeed);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemyDamage = other.gameObject.GetComponent<IEnemy>().EnemyAttack;
            int totalDamage = enemyDamage - CharacterDefense;
            Debug.Log("Defense: " + CharacterDefense.ToString());
            if (totalDamage <= 0)
            {
                totalDamage = 1;
                Debug.Log("Damage was less than or equal to 0, set to 1");
            }
            Debug.Log("Damage taken: " + totalDamage.ToString());
            CharacterHP -= totalDamage;
            HealthBarChange();
            hitSource.Play();
        }

        if (other.gameObject.tag == "Health")
        {
            float totalHealthValue = other.gameObject.GetComponent<IHealth>().HealthAmount;

            if (CharacterHP + totalHealthValue <= CharacterMaxHP)
            {
                CharacterHP += totalHealthValue;
            }
            else
            {
                CharacterHP = CharacterMaxHP;
            }

            Destroy(other.gameObject);

            HealthBarChange();
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemyDamage = other.gameObject.GetComponent<IEnemy>().EnemyAttack;
            Debug.Log("Enemy Attack: " + enemyDamage.ToString());
            int totalDamage = enemyDamage - CharacterDefense;
            Debug.Log("Defense: " + CharacterDefense.ToString());
            if (totalDamage <= 0)
            {
                totalDamage = 1;
                Debug.Log("Damage was less than or equal to 0, set to 1");
            }
            Debug.Log("Damage taken: " + totalDamage.ToString());
            if (CharacterHP >= 0 && damageTimer > damageDelay)
            {
                CharacterHP -= totalDamage;
                damageTimer = 0;
            }
            else
            {
                damageTimer += Time.deltaTime;
            }
            HealthBarChange();
        }
    }

    void HealthBarChange()
    {
        hpPercent = CharacterHP / CharacterMaxHP;

        if (hpPercent >= 0)
        {
            
            HealthBar.transform.localScale = new Vector3((float)hpPercent, 0.1f, 1f);
        }
    }

    //XP Gain Method

    public void XPGain(float XPItemValue)
    {
        float totalXPValue = XPItemValue * itemXPBoost;
        CharacterXP += totalXPValue;

        if (CharacterXP >= NeededXPToLevel)
        {
            GameManager.GetComponent<GameManager>().PausedGame = true;
            GameManager.GetComponent<GameManager>().timerOn = false;
            TempSpeed = CharacterSpeed;
            CharacterSpeed = 0f;
            TempCollectSpeed = this.transform.Find("CollectionRadius").GetComponent<CollectionRadiusScript>().TranslateSpeed;
            this.transform.Find("CollectionRadius").GetComponent<CollectionRadiusScript>().TranslateSpeed = 0f;

            enemySpeeds = new List<float>();
            foreach (Transform enemy in EnemyParent.transform)
            {
                enemySpeeds.Add(enemy.GetComponent<IEnemy>().EnemySpeed);
                enemy.GetComponent<IEnemy>().EnemySpeed = 0f;
            }

            foreach (Transform weaponSpawner in this.transform)
            {
                if (weaponSpawner.GetComponent<ISpawner>() != null)
                {
                    weaponSpawner.GetComponent<ISpawner>().spawnRate = 0f;
                }
            }

            LevelUp();
        }
    }

    void LevelUp()
    {
        CharacterLevel++;
        CharacterXP -= NeededXPToLevel;
        Debug.Log("Needed XP: " + NeededXPToLevel.ToString());
        //Cursor.visible = true;

        switch (CharacterLevel)
        {
            case int n when (n > 0 && n <= 20):
                NeededXPToLevel += 10;
                break;
            case int n when (n > 20 && n <= 40):
                NeededXPToLevel += 13;
                break;
            case int n when (n > 40):
                NeededXPToLevel += 16;
                break;
            default:
                NeededXPToLevel = 100;
                break;
        }

        //Character Ability

        switch (CharacterLevel)
        {
            case 10:
                CharacterAttackPower += 0.1;
                break;
            case 20:
                CharacterAttackPower += 0.1;
                break;
            case 30:
                CharacterAttackPower += 0.1;
                break;
            case 40:
                CharacterAttackPower += 0.1;
                break;
            case 50:
                CharacterAttackPower += 0.1;
                break;
        }

        GameManager gameManager = GameManager.GetComponent<GameManager>();

        Debug.Log("Leveling up");

        List<GameObject> validWeaponSelections = new List<GameObject>();
        for (int i = 0; i < gameManager.AllWeapons.Length; i++)
        {
            if(weaponsSpawned < 6)
            {
                validWeaponSelections.Add(gameManager.WeaponSelectButtons[i]);
            }
            for (int j = 0; j < transform.childCount; j++)
            {
                ISpawner weaponSpawner = transform.GetChild(j).GetComponent<ISpawner>();
                if (weaponSpawner != null && transform.GetChild(j).name == gameManager.AllWeapons[i].name)
                {
                    if(weaponSpawner.s_Level >= 7)
                    {
                        validWeaponSelections.Remove(gameManager.WeaponSelectButtons[i]);
                    }
                    else if (weaponSpawner.s_Level < 7)
                    {
                        validWeaponSelections.Add(gameManager.WeaponSelectButtons[i]);
                    }
                }
            }
        }

        for (int i = 0; i < gameManager.AllItems.Length; i++)
        {
            if(itemsSpawned < 6)
            {
                validWeaponSelections.Add(gameManager.ItemSelectButtons[i]);
            }
            for (int j = 0; j < ItemDisplay.transform.childCount; j++)
            {
                if (ItemDisplay.transform.GetChild(j).childCount != 0)
                {
                    IPassive passiveItem = ItemDisplay.transform.GetChild(j).GetChild(0).GetComponent<IPassive>();
                    if (passiveItem != null && ItemDisplay.transform.GetChild(j).GetChild(0).name == gameManager.AllItems[i].name)
                    {
                        if(passiveItem.ILevel >= 5)
                        {
                            validWeaponSelections.Remove(gameManager.ItemSelectButtons[i]);
                        }
                        else if(passiveItem.ILevel < 5)
                        {
                            validWeaponSelections.Add(gameManager.ItemSelectButtons[i]);
                        }
                    }
                }
            }
        }

        // Shuffle the list of valid weapon selections
        for (int i = 0; i < validWeaponSelections.Count; i++)
        {
            GameObject temp = validWeaponSelections[i];
            int randomIndex = Random.Range(i, validWeaponSelections.Count);
            validWeaponSelections[i] = validWeaponSelections[randomIndex];
            validWeaponSelections[randomIndex] = temp;
        }

        int numButtons = 0;
        for (int i = 0; i < 3 && i < validWeaponSelections.Count; i++)
        {
            GameObject buttonPrefab = validWeaponSelections[i];
            GameObject button = Instantiate(buttonPrefab, gameManager.WeaponSelectScreen.transform.GetChild(i));

            if(button.transform.GetChild(2) != null)
            {
                button.transform.GetChild(2).GetComponent<Text>().text = "testing text changes";
                if(button.tag == "WeaponSelect")
                {
                    switch(button.name)
                    {
                        case "BatonWeaponButton(Clone)":
                            int batonLevel = 0;
                            if (GameObject.Find("BatonSpawner") != null)
                            {
                                ISpawner batonSpawner = GameObject.Find("BatonSpawner").GetComponent<ISpawner>();
                                batonLevel = batonSpawner.s_Level;
                            }
                            button.transform.GetChild(2).GetComponent<Text>().text = GetBatonDesc(batonLevel);
                            break;
                        case "DroneWeaponButton(Clone)":
                            int droneLevel = 0;
                            if (GameObject.Find("DroneSpawner") != null)
                            {
                                ISpawner droneSpawner = GameObject.Find("DroneSpawner").GetComponent<ISpawner>();
                                droneLevel = droneSpawner.s_Level;
                            }
                            button.transform.GetChild(2).GetComponent<Text>().text = GetDroneDesc(droneLevel);
                            break;
                        case "KatanaWeaponButton(Clone)":
                            int katanaLevel = 0;
                            if (GameObject.Find("KatanaSpawner") != null)
                            {
                                ISpawner katanaSpawner = GameObject.Find("KatanaSpawner").GetComponent<ISpawner>();
                                katanaLevel = katanaSpawner.s_Level;
                            }
                            button.transform.GetChild(2).GetComponent<Text>().text = GetKatanaDesc(katanaLevel);
                            break;
                        case "MinigunWeaponButton(Clone)":
                            int miniLevel = 0;
                            if (GameObject.Find("MinigunSpawner") != null)
                            {
                                ISpawner miniSpawner = GameObject.Find("MinigunSpawner").GetComponent<ISpawner>();
                                miniLevel = miniSpawner.s_Level;
                            }
                            button.transform.GetChild(2).GetComponent<Text>().text = GetMinigunDesc(miniLevel);
                            break;
                        case "PistolWeaponButton(Clone)":
                            int pistolLevel = 0;
                            if (GameObject.Find("PistolSpawner") != null)
                            {
                                ISpawner pistolSpawner = GameObject.Find("PistolSpawner").GetComponent<ISpawner>();
                                pistolLevel = pistolSpawner.s_Level;
                            }
                            button.transform.GetChild(2).GetComponent<Text>().text = GetPistolDesc(pistolLevel);
                            break;
                        case "SniperWeaponButton(Clone)":
                            int sniperLevel = 0;
                            if (GameObject.Find("SniperSpawner") != null)
                            {
                                ISpawner sniperSpawner = GameObject.Find("SniperSpawner").GetComponent<ISpawner>();
                                sniperLevel = sniperSpawner.s_Level;
                            }
                            button.transform.GetChild(2).GetComponent<Text>().text = GetSniperDesc(sniperLevel);
                            break;
                        case "ElectromagnetWeaponButton(Clone)":
                            int electromagnetLevel = 0;
                            if (GameObject.Find("ElectromagnetSpawner") != null)
                            {
                                ISpawner electromagnetSpawner = GameObject.Find("ElectromagnetSpawner").GetComponent<ISpawner>();
                                electromagnetLevel = electromagnetSpawner.s_Level;
                            }
                            button.transform.GetChild(2).GetComponent<Text>().text = GetElectromagnetDesc(electromagnetLevel);
                            break;
                        case "PlasmaShotgunWeaponButton(Clone)":
                            int plasmaShotgunLevel = 0;
                            if (GameObject.Find("PlasmaShotgunSpawner") != null)
                            {
                                ISpawner plasmaShotgunSpawner = GameObject.Find("PlasmaShotgunSpawner").GetComponent<ISpawner>();
                                plasmaShotgunLevel = plasmaShotgunSpawner.s_Level;
                            }
                            button.transform.GetChild(2).GetComponent<Text>().text = GetPlasmaShotgunDesc(plasmaShotgunLevel);
                            break;
                        default:
                            button.transform.GetChild(2).GetComponent<Text>().text = "Couldn't find weapon desc.";
                            break;
                    }
                }
                else if (button.tag == "ItemSelect")
                {
                    switch (button.name)
                    {
                        case "AdrenalBoosterItemButton(Clone)":
                            button.transform.GetChild(2).GetComponent<Text>().text = "Reduces weapons cooldown by 8%.";
                            break;
                        case "BionicArmItemButton(Clone)":
                            button.transform.GetChild(2).GetComponent<Text>().text = "Raises inflicted damage by 10%.";
                            break;
                        case "BionicLegItemButton(Clone)":
                            button.transform.GetChild(2).GetComponent<Text>().text = "Character moves 10% faster.";
                            break;
                        case "CyberneticEyeItemButton(Clone)":
                            button.transform.GetChild(2).GetComponent<Text>().text = "Augments crit rate by 5%.";
                            break;
                        case "EnhancedMusclesItemButton(Clone)":
                            button.transform.GetChild(2).GetComponent<Text>().text = "Augments area of attacks by 10%.";
                            break;
                        case "HackwaresItemButton(Clone)":
                            button.transform.GetChild(2).GetComponent<Text>().text = "Character gains 8% more experience.";
                            break;
                        case "NanomachinesItemButton(Clone)":
                            button.transform.GetChild(2).GetComponent<Text>().text = "Character recovers 0.2 HP per second.";
                            break;
                        case "SecondHeartItemButton(Clone)":
                            button.transform.GetChild(2).GetComponent<Text>().text = "Augments max health by 20%.";
                            break;
                        case "SmartChipItemButton(Clone)":
                            button.transform.GetChild(2).GetComponent<Text>().text = "Augments crit damage by 10%.";
                            break;
                        case "SubdermalArmorItemButton(Clone)":
                            button.transform.GetChild(2).GetComponent<Text>().text = "Reduces incoming damage by 1.";
                            break;
                        default:
                            button.transform.GetChild(2).GetComponent<Text>().text = "Couldn't find item desc.";
                            break;
                    }
                }
            }
            else
            {
                Debug.Log("Could not find button child text");
            }

            numButtons++;
        }

        gameManager.WeaponSelectScreen.SetActive(true);
    }

    public string GetBatonDesc(int batonLevel)
    {
        string batonDesc;
        switch (batonLevel)
        {
            case 0:
                batonDesc = "Attacks in front, passes through enemies.";
                break;
            case 1:
                batonDesc = "Damage increases by 20%.";
                break;
            case 2:
                batonDesc = "Attack area increases by 25%.";
                break;
            case 3:
                batonDesc = "Cooldown reduces by 15%.";
                break;
            case 4:
                batonDesc = "Crit rate increases by 5%.";
                break;
            case 5:
                batonDesc = "Damage increases by 66%.";
                break;
            case 6:
                batonDesc = "Chance to stun enemies.";
                break;
            default:
                batonDesc = "Error making Baton Desc";
                break;
        }
        return batonDesc;
    }

    public string GetDroneDesc(int droneLevel)
    {
        string droneDesc;
        switch (droneLevel)
        {
            case 0:
                droneDesc = "Construct that fires high damage projectiles.";
                break;
            case 1:
                droneDesc = "Damage increases by 25%.";
                break;
            case 2:
                droneDesc = "Cooldown reduces by 50%.";
                break;
            case 3:
                droneDesc = "Projectile passes through an additional enemy.";
                break;
            case 4:
                droneDesc = "Crit rate increases by 3%.";
                break;
            case 5:
                droneDesc = "Cooldown reduces by 33%.";
                break;
            case 6:
                droneDesc = "Bullets gain homing capabilities.";
                break;
            default:
                droneDesc = "Error making Drone Desc";
                break;
        }
        return droneDesc;
    }

    public string GetKatanaDesc(int katanaLevel)
    {
        string katanaDesc;
        switch (katanaLevel)
        {
            case 0:
                katanaDesc = "High crit, high damage.";
                break;
            case 1:
                katanaDesc = "Damage increases by 25%.";
                break;
            case 2:
                katanaDesc = "Crit rate increases by 4%.";
                break;
            case 3:
                katanaDesc = "Cooldown reduces by 15%.";
                break;
            case 4:
                katanaDesc = "Crit rate increases by 6%.";
                break;
            case 5:
                katanaDesc = "Crit damage increases by 25%.";
                break;
            case 6:
                katanaDesc = "Strikes twice in an X shape.";
                break;
            default:
                katanaDesc = "Error making Katana Desc";
                break;
        }
        return katanaDesc;
    }

    public string GetMinigunDesc(int miniLevel)
    {
        string miniDesc;
        switch (miniLevel)
        {
            case 0:
                miniDesc = "Low damage, fast firing projectiles.";
                break;
            case 1:
                miniDesc = "Projectile passes through an additional enemy.";
                break;
            case 2:
                miniDesc = "Damage increases by 50%.";
                break;
            case 3:
                miniDesc = "Crit rate increases by 3%.";
                break;
            case 4:
                miniDesc = "Cooldown reduces by 15%.";
                break;
            case 5:
                miniDesc = "Damage increases by 66%.";
                break;
            case 6:
                miniDesc = "Removes hit limit and increases bullet caliber.";
                break;
            default:
                miniDesc = "Error making Mini Desc";
                break;
        }
        return miniDesc;
    }

    public string GetPistolDesc(int pistolLevel)
    {
        string pistolDesc;
        switch (pistolLevel)
        {
            case 0:
                pistolDesc = "Basic handgun projectile.";
                break;
            case 1:
                pistolDesc = "Shoots an additional projectile.";
                break;
            case 2:
                pistolDesc = "Damage increases by 50%.";
                break;
            case 3:
                pistolDesc = "Projectile passes through an additional enemy.";
                break;
            case 4:
                pistolDesc = "Cooldown reduces by 15%.";
                break;
            case 5:
                pistolDesc = "Damage increases by 66%.";
                break;
            case 6:
                pistolDesc = "Gain a knife attack with the pistol.";
                break;
            default:
                pistolDesc = "Error making Pistol Desc";
                break;
        }
        return pistolDesc;
    }

    public string GetSniperDesc(int sniperLevel)
    {
        string sniperDesc;
        switch (sniperLevel)
        {
            case 0:
                sniperDesc = "High damage, fast projectile.";
                break;
            case 1:
                sniperDesc = "Cooldown reduces by 200%.";
                break;
            case 2:
                sniperDesc = "Damage increases by 77%.";
                break;
            case 3:
                sniperDesc = "Cooldown reduces by 200%.";
                break;
            case 4:
                sniperDesc = "Projectile passes through an additional enemy.";
                break;
            case 5:
                sniperDesc = "Damage increases by 87%.";
                break;
            case 6:
                sniperDesc = "Sniper rounds become explosive.";
                break;
            default:
                sniperDesc = "Error making Sniper Desc";
                break;
        }
        return sniperDesc;
    }

    public string GetElectromagnetDesc(int electromagnetLevel)
    {
        string electromagnetDesc;
        switch (electromagnetLevel)
        {
            case 0:
                electromagnetDesc = "Field of electromagnetic damage.";
                break;
            case 1:
                electromagnetDesc = "Attack area increases by 15%.";
                break;
            case 2:
                electromagnetDesc = "Damage increases by 20%.";
                break;
            case 3:
                electromagnetDesc = "Attack area increases by 30%.";
                break;
            case 4:
                electromagnetDesc = "Cooldown reduces by 50%.";
                break;
            case 5:
                electromagnetDesc = "Damage increases by 66%.";
                break;
            case 6:
                electromagnetDesc = "Attack area increases by 20%.";
                break;
            default:
                electromagnetDesc = "Error making Electromagnet Desc";
                break;
        }
        return electromagnetDesc;
    }

    public string GetPlasmaShotgunDesc(int plasmaShotgunLevel)
    {
        string plasmaShotgunDesc;
        switch (plasmaShotgunLevel)
        {
            case 0:
                plasmaShotgunDesc = "Spread shot of low-damage blasts.";
                break;
            case 1:
                plasmaShotgunDesc = "Damage increases by 33%.";
                break;
            case 2:
                plasmaShotgunDesc = "Shoots an additional projectile.";
                break;
            case 3:
                plasmaShotgunDesc = "Damage increases by 25%.";
                break;
            case 4:
                plasmaShotgunDesc = "Cooldown reduces by 50%.";
                break;
            case 5:
                plasmaShotgunDesc = "Shoots an additional projectile.";
                break;
            case 6:
                plasmaShotgunDesc = "Projectile size increases by 25%.";
                break;
            default:
                plasmaShotgunDesc = "Error making Plasma Shotgun Desc";
                break;
        }
        return plasmaShotgunDesc;
    }
}