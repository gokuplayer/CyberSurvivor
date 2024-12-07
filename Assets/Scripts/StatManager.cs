using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatManager : MonoBehaviour
{
    //Pause Menu Text
    public TextMeshProUGUI HPDisplayText;
    public TextMeshProUGUI RecoveryDisplayText;
    public TextMeshProUGUI DefenseDisplayText;
    public TextMeshProUGUI SpeedDisplayText;
    public TextMeshProUGUI AttackDisplayText;
    public TextMeshProUGUI AreaDisplayText;
    public TextMeshProUGUI CritRateDisplayText;
    public TextMeshProUGUI CritDmgDisplayText;
    public TextMeshProUGUI CooldownDisplayText;
    public TextMeshProUGUI XPBoostDisplayText;
    //Level Up Menu Text
    public TextMeshProUGUI HPLevelText;
    public TextMeshProUGUI RecoveryLevelText;
    public TextMeshProUGUI DefenseLevelText;
    public TextMeshProUGUI SpeedLevelText;
    public TextMeshProUGUI AttackLevelText;
    public TextMeshProUGUI AreaLevelText;
    public TextMeshProUGUI CritRateLevelText;
    public TextMeshProUGUI CritDmgLevelText;
    public TextMeshProUGUI CooldownLevelText;
    public TextMeshProUGUI XPBoostLevelText;

    public GameObject PlayerObject;
    public GameObject GameManagerObject;
    public ICharacter PlayerScript;
    public GameManager GameManagerScript;

    private float playerHP;
    private float playerMaxHP;
    private float playerRecover;
    private int playerDefense;
    private float playerSpeed;
    private double playerAttack;
    private float playerArea;
    private float playerCritRate;
    private double playerCritDmg;
    private float playerCooldown;
    private float playerXPBoost;

    //Stat Order: HP, Recover, Defense, Speed, Attack, Area, Crit Rate, Crit Dmg, Cooldown, Growth

    void Start()
    {
        StartCoroutine(WaitForTimer());
    }

    void Update()
    {
        if(PlayerObject != null)
        {
            playerHP = PlayerScript.CharacterHP;
            playerMaxHP = PlayerScript.CharacterMaxHP;
            playerRecover = GameManagerScript.playerRecovery;
            playerDefense = PlayerScript.CharacterDefense;
            playerSpeed = PlayerScript.TempSpeed;
            playerAttack = PlayerScript.CharacterAttackPower;
            if(GameObject.Find("EnhancedMuscles") != null)
            {
                playerArea = GameObject.Find("EnhancedMuscles").GetComponent<EnhancedMusclesScript>().sizeBonus;
            }
            else
            {
                playerArea = 1;
            }
            playerCritRate = PlayerScript.CharacterCritRate;
            if(PlayerScript.CharacterName == "Sniper Elite" && GameObject.Find("SmartChip") == null)
            {
                playerCritDmg = PlayerObject.GetComponent<SniperElite>().SniperCritDmg + 1.5;
            }
            else if (PlayerScript.CharacterName == "Sniper Elite" && GameObject.Find("SmartChip") != null)
            {
                playerCritDmg = PlayerObject.GetComponent<SniperElite>().SniperCritDmg + GameObject.Find("SmartChip").GetComponent<SmartChipScript>().critDmgBonus + 1.5;
            }
            else if (GameObject.Find("SmartChip") != null)
            {
                playerCritDmg = GameObject.Find("SmartChip").GetComponent<SmartChipScript>().critDmgBonus + 1.5;
            }
            else
            {
                playerCritDmg = 1.5;
            }
            if (GameObject.Find("Adrenal Booster") != null)
            {
                playerCooldown = GameObject.Find("Adrenal Booster").GetComponent<AdrenalBoosterScript>().spawnRateBonus - 1;
            }
            else
            {
                playerCooldown = 0;
            }
            playerXPBoost = PlayerScript.itemXPBoost;


            HPDisplayText.text = "HP: " + playerHP.ToString("F0") + "/" + playerMaxHP.ToString("F0");
            HPLevelText.text = "HP: " + playerHP.ToString("F0") + "/" + playerMaxHP.ToString("F0");
            if (playerRecover == 0)
            {
                RecoveryDisplayText.text = "Recovery: -";
                RecoveryLevelText.text = "Recovery: -";
            }
            else
            {
                RecoveryDisplayText.text = "Recovery: " + playerRecover.ToString();
                RecoveryLevelText.text = "Recovery: " + playerRecover.ToString();
            }
            if (playerDefense == 0)
            {
                DefenseDisplayText.text = "Armor: -";
                DefenseLevelText.text = "Armor: -";
            }
            else
            {
                DefenseDisplayText.text = "Armor: " + playerDefense.ToString();
                DefenseLevelText.text = "Armor: " + playerDefense.ToString();
            }
            SpeedDisplayText.text = "Speed: " + playerSpeed.ToString();
            SpeedLevelText.text = "Speed: " + playerSpeed.ToString();
            AttackDisplayText.text = "Attack: " + (playerAttack * 100).ToString() + "%";
            AttackLevelText.text = "Attack: " + (playerAttack * 100).ToString() + "%";
            AreaDisplayText.text = "Area: " + (playerArea * 100).ToString() + "%";
            AreaLevelText.text = "Area: " + (playerArea * 100).ToString() + "%";
            CritRateDisplayText.text = "Crit Rate: " + playerCritRate.ToString() + "%";
            CritRateLevelText.text = "Crit Rate: " + playerCritRate.ToString() + "%";
            CritDmgDisplayText.text = "Crit Dmg: " + (playerCritDmg * 100).ToString() + "%";
            CritDmgLevelText.text = "Crit Dmg: " + (playerCritDmg * 100).ToString() + "%";
            if (playerCooldown == 0)
            {
                CooldownDisplayText.text = "Cooldown: -";
                CooldownLevelText.text = "Cooldown: -";
            }
            else
            {
                CooldownDisplayText.text = "Cooldown: " + Mathf.Round(playerCooldown * 100).ToString() + "%";
                CooldownLevelText.text = "Cooldown: " + Mathf.Round(playerCooldown * 100).ToString() + "%";
            }
            if (playerXPBoost == 0)
            {
                XPBoostDisplayText.text = "XP Boost: -";
                XPBoostLevelText.text = "XP Boost: -";
            }
            else
            {
                XPBoostDisplayText.text = "XP Boost: " + (playerXPBoost * 100).ToString() + "%";
                XPBoostLevelText.text = "XP Boost: " + (playerXPBoost * 100).ToString() + "%";
            }
        }
    }

    private IEnumerator WaitForTimer()
    {
        while (!this.GetComponent<GameManager>().timerOn)
        {
            yield return null;
        }

        StartVariables();
    }

    private void StartVariables()
    {
        PlayerObject = GameObject.FindWithTag("Player");
        PlayerScript = PlayerObject.GetComponent<ICharacter>();
        GameManagerObject = GameObject.Find("Game Manager");
        GameManagerScript = GameManagerObject.GetComponent<GameManager>();
    }
}
