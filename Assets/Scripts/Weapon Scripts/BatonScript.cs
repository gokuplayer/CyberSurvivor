using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BatonScript : MonoBehaviour, IWeapon
{
    public float w_DestroyMax { get; set; }
    public float w_DestroyTimer { get; set; }
    public double w_Damage { get; set; }
    public float w_Crit { get; set; }
    public float targetHealth { get; set; }
    public double damageMultiplier { get; set; }
    public float totalDamage { get; set; }
    public float playerCrit { get; set; }
    public double critDamage { get; set; }
    public double itemCritDmg { get; set; }
    public float critRoll { get; set; }
    public float velocity { get; set; }
    public float spawnOffset { get; set; }
    public int w_Level { get; set; }
    public GameObject playerObject { get; set; }
    public Rigidbody2D weaponBody { get; set; }
    public float itemSizeBonus;

    public GameObject DmgTextObject;
    public float stunRoll;
    private Transform CanvasTransform;
    private SpriteRenderer batonRenderer;
    private Color awakenedColor;
    private bool awakenedBool;

    Dictionary<int, float> DamageDict = new Dictionary<int, float>()
    {
        {1, 5f},
        {2, 6f},
        {3, 6f},
        {4, 6f},
        {5, 6f},
        {6, 10f},
        {7, 10f}
    };

    Dictionary<int, float> SizeDict = new Dictionary<int, float>()
    {
        {1, 1f},
        {2, 1f},
        {3, 1.25f},
        {4, 1.25f},
        {5, 1.25f},
        {6, 1.25f},
        {7, 1.25f}
    };

    Dictionary<int, float> CritDict = new Dictionary<int, float>()
    {
        {1, 5f},
        {2, 5f},
        {3, 5f},
        {4, 5f},
        {5, 10f},
        {6, 10f},
        {7, 10f}
    };

    void Start()
    {
        playerObject = GameObject.FindWithTag("Player");
        CanvasTransform = GameObject.Find("WorldCanvas").transform;
        batonRenderer = this.transform.GetChild(0).GetComponent<SpriteRenderer>();

        Debug.Log("Weapon Level: " + w_Level.ToString());

        awakenedBool = false;

        w_Level = 1;
        w_DestroyMax = 0.3f;
        w_DestroyTimer = 0f;

        w_Damage = DamageDict[w_Level];

        if (playerObject.GetComponent<ICharacter>().CharacterName == "Sniper Elite")
        {
            critDamage = 1.5 + playerObject.GetComponent<SniperElite>().SniperCritDmg;
        }
        else
        {
            critDamage = 1.5;
        }

        itemCritDmg = GameObject.Find("BatonSpawner").GetComponent<ISpawner>().itemCritDmg;
        spawnOffset = 1.5f;
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
        velocity = 1f;

        Debug.Log("Item crit dmg: " + itemCritDmg.ToString());

        awakenedColor = new Color(0, 1, 1, 1);
    }

    void Update()
    {
        w_Level = playerObject.transform.Find("BatonSpawner").GetComponent<ISpawner>().s_Level;
        w_Damage = DamageDict[w_Level];
        if (GameObject.Find("EnhancedMuscles") != null)
        {
            itemSizeBonus = GameObject.Find("EnhancedMuscles").GetComponent<EnhancedMusclesScript>().sizeBonus;

            this.transform.localScale = new Vector3(SizeDict[w_Level] * itemSizeBonus, SizeDict[w_Level] * itemSizeBonus, 1);
        }
        else
        {
            this.transform.localScale = new Vector3(SizeDict[w_Level], SizeDict[w_Level], 1);
        }
        w_Crit = CritDict[w_Level];

        if (w_Level == 7)
        {
            batonRenderer.color = awakenedColor;
            awakenedBool = true;
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
        if(damageMultiplier <= 0)
        {
            damageMultiplier = 1;
        }
        Debug.Log("Baton Damage Multiplier: " + damageMultiplier.ToString());
        Debug.Log("Baton Damage : " + w_Damage.ToString());
        if (other.gameObject.tag == "Enemy")
        {
            stunRoll = Random.Range(0, 100);
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
                if(stunRoll <= 30 && awakenedBool)
                {
                    if(other.gameObject.GetComponent<IEnemy>().EnemyStatus != "Stunned")
                    {
                        other.gameObject.GetComponent<IEnemy>().EnemyStatus = "Stunned";
                    }
                }
            }
            else
            {
                other.gameObject.GetComponent<IEnemy>().EnemyHP -= (w_Damage * damageMultiplier);
                if(DmgTextObject != null)
                {
                    Debug.Log("Enemy position for baton hit: " + other.transform.position.ToString());
                    GameObject DmgTextInstance = Instantiate(DmgTextObject, other.transform.position, Quaternion.identity);
                    TextMeshProUGUI DmgTextInstanceTMP = DmgTextInstance.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    DmgTextInstanceTMP.text = (w_Damage * damageMultiplier * 10).ToString();
                    DmgTextInstanceTMP.color = new Color32(0, 255, 255, 255);

                    DmgTextInstance.transform.SetParent(CanvasTransform, false);
                }
                if (stunRoll <= 30 && awakenedBool)
                {
                    if (other.gameObject.GetComponent<IEnemy>().EnemyStatus != "Stunned")
                    {
                        other.gameObject.GetComponent<IEnemy>().EnemyStatus = "Stunned";
                        other.gameObject.GetComponent<IEnemy>().EStatusMaxTime = 5f;
                        Debug.Log("Enemy is stunned");
                    }
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
