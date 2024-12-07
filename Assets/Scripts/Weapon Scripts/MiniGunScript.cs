using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniGunScript : MonoBehaviour, IWeapon
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
    public int enemiesHit;
    public GameObject playerObject { get; set; }
    public Rigidbody2D weaponBody { get; set; }
    public float itemSizeBonus;

    public GameObject DmgTextObject;
    private Transform CanvasTransform;

    Dictionary<int, float> DamageDict = new Dictionary<int, float>
    {
        {1, 1f},
        {2, 1f},
        {3, 1.25f},
        {4, 1.25f},
        {5, 1.25f},
        {6, 2f},
        {7, 2f}
    };

    Dictionary<int, float> CritDict = new Dictionary<int, float>()
    {
        {1, 3f},
        {2, 3f},
        {3, 3f},
        {4, 6f},
        {5, 6f},
        {6, 6f},
        {7, 6f}
    };

    Dictionary<int, int> HitLimitDict = new Dictionary<int, int>()
    {
        {1, 1},
        {2, 2},
        {3, 2},
        {4, 2},
        {5, 2},
        {6, 2},
        {7, 50}
    };

    Dictionary<int, float> SizeDict = new Dictionary<int, float>()
    {
        {1, 1f},
        {2, 1f},
        {3, 1f},
        {4, 1f},
        {5, 1f},
        {6, 1f},
        {7, 1.10f}
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

        enemiesHit = 0;

        if (playerObject.GetComponent<ICharacter>().CharacterName == "Sniper Elite")
        {
            critDamage = 1.5 + playerObject.GetComponent<SniperElite>().SniperCritDmg;
        }
        else
        {
            critDamage = 1.5;
        }

        itemCritDmg = GameObject.Find("MinigunSpawner").GetComponent<ISpawner>().itemCritDmg;
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
        critRoll = Random.Range(0, 100);

        weaponBody = GetComponent<Rigidbody2D>();
        velocity = 500f;
        spawnOffset = 1f;
    }

    void Update()
    {
        w_Level = playerObject.transform.Find("MinigunSpawner").GetComponent<ISpawner>().s_Level;
        w_Damage = DamageDict[w_Level];
        w_Crit = CritDict[w_Level];
        if (GameObject.Find("EnhancedMuscles") != null)
        {
            itemSizeBonus = GameObject.Find("EnhancedMuscles").GetComponent<EnhancedMusclesScript>().sizeBonus;

            this.transform.localScale = new Vector3(SizeDict[w_Level] * itemSizeBonus, SizeDict[w_Level] * itemSizeBonus, 1);
        }
        else
        {
            this.transform.localScale = new Vector3(SizeDict[w_Level], SizeDict[w_Level], 1);
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
            enemiesHit++;
            Debug.Log("Enemies hit: " + enemiesHit.ToString());
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

            if (enemiesHit >= HitLimitDict[w_Level])
            {
                Debug.Log("Minigun Hit Limit: " + HitLimitDict[w_Level].ToString());
                enemiesHit = 0;
                Destroy(this.gameObject);
            }
        }

        if (other.gameObject.tag == "Breakable")
        {
            enemiesHit++;
            if (critRoll <= (w_Crit + playerCrit))
            {
                other.gameObject.GetComponent<IBreakable>().ObjectHP -= (w_Damage * damageMultiplier * (critDamage + itemCritDmg));
                Debug.Log("Critical Hit!");
            }
            else
            {
                other.gameObject.GetComponent<IBreakable>().ObjectHP -= (w_Damage * damageMultiplier);
            }

            if (enemiesHit >= HitLimitDict[w_Level])
            {
                enemiesHit = 0;
                Destroy(this.gameObject);
            }
        }
    }
}