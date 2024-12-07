using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ElectromagnetScript : MonoBehaviour, IWeapon
{
    public float w_DestroyMax { get; set; }
    public float w_DestroyTimer { get; set; }
    public double w_Damage { get; set; }
    public float w_Crit { get; set; }
    public float targetHealth { get; set; }
    public double damageMultiplier { get; set; }
    public float totalDamage { get; set; }
    public float playerCrit { get; set; }
    public float critRoll { get; set; }
    public double critDamage { get; set; }
    public double itemCritDmg { get; set; }
    public float velocity { get; set; }
    public float spawnOffset { get; set; }
    public int w_Level { get; set; }
    public GameObject playerObject { get; set; }
    public Rigidbody2D weaponBody { get; set; }
    public float itemSizeBonus;

    public GameObject DmgTextObject;
    private Transform CanvasTransform;

    Dictionary<int, float> DamageDict = new Dictionary<int, float>()
    {
        {1, 0.5f},
        {2, 0.5f},
        {3, 0.6f},
        {4, 0.6f},
        {5, 0.6f},
        {6, 1f},
        {7, 1f}
    };

    Dictionary<int, float> SizeDict = new Dictionary<int, float>()
    {
        {1, 1f},
        {2, 1.15f},
        {3, 1.15f},
        {4, 1.5f},
        {5, 1.5f},
        {6, 1.5f},
        {7, 1.8f}
    };

    void Start()
    {
        playerObject = GameObject.FindWithTag("Player");
        CanvasTransform = GameObject.Find("WorldCanvas").transform;

        Debug.Log("Weapon Level: " + w_Level.ToString());

        w_Level = 1;
        w_DestroyMax = 1f;
        w_DestroyTimer = 0f;

        w_Damage = DamageDict[w_Level];
        w_Crit = 3f;

        if (playerObject.GetComponent<ICharacter>().CharacterName == "Sniper Elite")
        {
            critDamage = 1.5 + playerObject.GetComponent<SniperElite>().SniperCritDmg;
        }
        else
        {
            critDamage = 1.5;
        }

        itemCritDmg = GameObject.Find("ElectromagnetSpawner").GetComponent<ISpawner>().itemCritDmg;
        velocity = 0f;
        spawnOffset = 0f;
        if (GameObject.Find("EnhancedMuscles") != null)
        {
            Debug.Log("Enhanced Muscles found.");
            itemSizeBonus = GameObject.Find("EnhancedMuscles").GetComponent<EnhancedMusclesScript>().sizeBonus;

            this.transform.localScale = new Vector3(SizeDict[w_Level] * itemSizeBonus, SizeDict[w_Level] * itemSizeBonus, 1);
        }
        else
        {
            Debug.Log("Enhanced Muscles not found.");
            this.transform.localScale = new Vector3(SizeDict[w_Level], SizeDict[w_Level], 1);
        }

        damageMultiplier = playerObject.gameObject.GetComponent<ICharacter>().CharacterAttackPower;
        playerCrit = playerObject.gameObject.GetComponent<ICharacter>().CharacterCritRate;

        critRoll = UnityEngine.Random.Range(0, 100);

        Debug.Log("Item crit dmg: " + itemCritDmg.ToString());
    }

    void Update()
    {
        w_Level = playerObject.transform.Find("ElectromagnetSpawner").GetComponent<ISpawner>().s_Level;
        w_Damage = DamageDict[w_Level];
        itemCritDmg = GameObject.Find("ElectromagnetSpawner").GetComponent<ISpawner>().itemCritDmg;

        damageMultiplier = playerObject.gameObject.GetComponent<ICharacter>().CharacterAttackPower;
        playerCrit = playerObject.gameObject.GetComponent<ICharacter>().CharacterCritRate;

        if (GameObject.Find("EnhancedMuscles") != null)
        {
            itemSizeBonus = GameObject.Find("EnhancedMuscles").GetComponent<EnhancedMusclesScript>().sizeBonus;

            this.transform.localScale = new Vector3(SizeDict[w_Level] * itemSizeBonus, SizeDict[w_Level] * itemSizeBonus, 1);
        }
        else
        {
            this.transform.localScale = new Vector3(SizeDict[w_Level], SizeDict[w_Level], 1);
        }

        if (playerObject.GetComponent<ICharacter>().CharacterName == "Sniper Elite")
        {
            critDamage = 1.5 + playerObject.GetComponent<SniperElite>().SniperCritDmg;
        }
        else
        {
            critDamage = 1.5;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (damageMultiplier <= 0)
        {
            damageMultiplier = 1;
        }
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy entered field!");

            critRoll = UnityEngine.Random.Range(0, 100);

            if (critRoll <= (w_Crit + playerCrit))
            {
                other.gameObject.GetComponent<IEnemy>().EnemyHP -= (w_Damage * damageMultiplier * (critDamage + itemCritDmg));
                Debug.Log("Critical Hit!");
                if (DmgTextObject != null)
                {
                    GameObject DmgTextInstance = Instantiate(DmgTextObject, other.transform.position, Quaternion.identity);
                    TextMeshProUGUI DmgTextInstanceTMP = DmgTextInstance.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    double roundedDamage = Math.Round((w_Damage * damageMultiplier * (critDamage + itemCritDmg) * 10), 2);
                    DmgTextInstanceTMP.text = roundedDamage.ToString();
                    DmgTextInstanceTMP.color = new Color32(255, 0, 0, 255);

                    DmgTextInstance.transform.SetParent(CanvasTransform, false);
                }
            }
            else
            {
                other.gameObject.GetComponent<IEnemy>().EnemyHP -= (w_Damage * damageMultiplier);
                if (DmgTextObject != null)
                {
                    GameObject DmgTextInstance = Instantiate(DmgTextObject, other.transform.position, Quaternion.identity);
                    TextMeshProUGUI DmgTextInstanceTMP = DmgTextInstance.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    DmgTextInstanceTMP.text = (w_Damage * damageMultiplier * 10).ToString();
                    DmgTextInstanceTMP.color = new Color32(0, 255, 255, 255);

                    DmgTextInstance.transform.SetParent(CanvasTransform, false);
                }
            }
        }

        if (other.gameObject.tag == "Breakable")
        {
            if (critRoll <= (w_Crit + playerCrit))
            {
                other.gameObject.GetComponent<IBreakable>().ObjectHP -= (w_Damage * damageMultiplier * (critDamage + itemCritDmg));
                Debug.Log("Critical Hit!");
            }
            else
            {
                other.gameObject.GetComponent<IBreakable>().ObjectHP -= (w_Damage * damageMultiplier);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy in field!");

            critRoll = UnityEngine.Random.Range(0, 100);

            if (playerObject.transform.Find("ElectromagnetSpawner").GetComponent<ElectromagnetSpawnerScript>().weaponTimer > playerObject.transform.Find("ElectromagnetSpawner").GetComponent<ElectromagnetSpawnerScript>().weaponDelay)
            {
                if (critRoll <= (w_Crit + playerCrit))
                {
                    other.gameObject.GetComponent<IEnemy>().EnemyHP -= (w_Damage * damageMultiplier * (critDamage + itemCritDmg));
                    Debug.Log("Critical Hit!");
                    if (DmgTextObject != null)
                    {
                        GameObject DmgTextInstance = Instantiate(DmgTextObject, other.transform.position, Quaternion.identity);
                        TextMeshProUGUI DmgTextInstanceTMP = DmgTextInstance.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                        DmgTextInstanceTMP.text = (w_Damage * damageMultiplier * (critDamage + itemCritDmg) * 10).ToString();
                        DmgTextInstanceTMP.color = new Color32(255, 0, 0, 255);

                        DmgTextInstance.transform.SetParent(CanvasTransform, false);
                    }
                }
                else
                {
                    other.gameObject.GetComponent<IEnemy>().EnemyHP -= (w_Damage * damageMultiplier);
                    if (DmgTextObject != null)
                    {
                        GameObject DmgTextInstance = Instantiate(DmgTextObject, other.transform.position, Quaternion.identity);
                        TextMeshProUGUI DmgTextInstanceTMP = DmgTextInstance.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                        DmgTextInstanceTMP.text = (w_Damage * damageMultiplier * 10).ToString();
                        DmgTextInstanceTMP.color = new Color32(0, 255, 255, 255);

                        DmgTextInstance.transform.SetParent(CanvasTransform, false);
                    }
                }

                playerObject.transform.Find("ElectromagnetSpawner").GetComponent<ElectromagnetSpawnerScript>().weaponTimer = 0;
            }
            else
            {
                playerObject.transform.Find("ElectromagnetSpawner").GetComponent<ElectromagnetSpawnerScript>().weaponTimer += Time.deltaTime * playerObject.transform.Find("ElectromagnetSpawner").GetComponent<ElectromagnetSpawnerScript>().spawnRate;
            }
        }
    }
}
