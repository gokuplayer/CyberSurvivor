using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KatanaScript : MonoBehaviour, IWeapon
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

    Dictionary<int, float> DamageDict = new Dictionary<int, float>
    {
        {1, 4f},
        {2, 5f},
        {3, 5f},
        {4, 5f},
        {5, 5f},
        {6, 5f},
        {7, 5f}
    };

    Dictionary<int, float> CritDict = new Dictionary<int, float>()
    {
        {1, 8f},
        {2, 12f},
        {3, 12f},
        {4, 12f},
        {5, 18f},
        {6, 18f},
        {7, 18f}
    };

    Dictionary<int, double> CritDmgDict = new Dictionary<int, double>()
    {
        {1, 2},
        {2, 2},
        {3, 2},
        {4, 2},
        {5, 2},
        {6, 2.5},
        {7, 2.5}
    };

    void Start()
    {
        playerObject = GameObject.FindWithTag("Player");
        CanvasTransform = GameObject.Find("WorldCanvas").transform;

        w_Level = 1;
        w_DestroyMax = 0.3f;
        w_DestroyTimer = 0f;

        w_Damage = DamageDict[w_Level];

        itemCritDmg = GameObject.Find("KatanaSpawner").GetComponent<ISpawner>().itemCritDmg;
        spawnOffset = 2f;
        if (GameObject.Find("EnhancedMuscles") != null)
        {
            itemSizeBonus = GameObject.Find("EnhancedMuscles").GetComponent<EnhancedMusclesScript>().sizeBonus;

            this.transform.localScale = new Vector3(itemSizeBonus, itemSizeBonus, 1);
        }

        damageMultiplier = playerObject.gameObject.GetComponent<ICharacter>().CharacterAttackPower;
        playerCrit = playerObject.gameObject.GetComponent<ICharacter>().CharacterCritRate;
        critRoll = Random.Range(0, 100);

        weaponBody = GetComponent<Rigidbody2D>();
        velocity = 1f;
    }

    void Update()
    {
        w_Level = playerObject.transform.Find("KatanaSpawner").GetComponent<ISpawner>().s_Level;
        w_Damage = DamageDict[w_Level];
        w_Crit = CritDict[w_Level];
        if (GameObject.Find("EnhancedMuscles") != null)
        {
            itemSizeBonus = GameObject.Find("EnhancedMuscles").GetComponent<EnhancedMusclesScript>().sizeBonus;

            this.transform.localScale = new Vector3(itemSizeBonus, itemSizeBonus, 1);
        }

        if (playerObject.GetComponent<ICharacter>().CharacterName == "Sniper Elite")
        {
            critDamage = CritDmgDict[w_Level] + playerObject.GetComponent<SniperElite>().SniperCritDmg;
        }
        else
        {
            critDamage = CritDmgDict[w_Level];
        }

        w_DestroyTimer += Time.deltaTime;
        if (w_DestroyTimer > w_DestroyMax)
        {
            Destroy(this.gameObject);
            w_DestroyTimer = 0f;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("hit!");
        if (other.gameObject.tag == "Enemy")
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
                    Debug.Log("Enemy position for baton hit: " + other.transform.position.ToString());
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
}
