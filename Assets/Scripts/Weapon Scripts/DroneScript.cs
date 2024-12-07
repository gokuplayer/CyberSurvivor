using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DroneScript : MonoBehaviour, IWeapon
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
    public float turnSpeed;
    public float maxDetectDistance;
    public LayerMask EnemyLayer;

    private Transform CanvasTransform;
    private Transform target;
    private Vector2 direction;
    private Rigidbody2D rb;

    Dictionary<int, float> DamageDict = new Dictionary<int, float>()
    {
        {1, 8f},
        {2, 10f},
        {3, 10f},
        {4, 10f},
        {5, 10f},
        {6, 10f},
        {7, 10f}
    };

    Dictionary<int, int> HitLimitDict = new Dictionary<int, int>()
    {
        {1, 1},
        {2, 1},
        {3, 1},
        {4, 2},
        {5, 2},
        {6, 2},
        {7, 2}
    };

    Dictionary<int, float> CritDict = new Dictionary<int, float>()
    {
        {1, 2f},
        {2, 2f},
        {3, 2f},
        {4, 2f},
        {5, 5f},
        {6, 5f},
        {7, 5f}
    };

    void Start()
    {
        playerObject = GameObject.FindWithTag("Player");
        CanvasTransform = GameObject.Find("WorldCanvas").transform;
        w_Level = 1;

        w_DestroyMax = 5.0f;
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

        itemCritDmg = GameObject.Find("DroneSpawner").GetComponent<ISpawner>().itemCritDmg;
        if (GameObject.Find("EnhancedMuscles") != null)
        {
            itemSizeBonus = GameObject.Find("EnhancedMuscles").GetComponent<EnhancedMusclesScript>().sizeBonus;

            this.transform.localScale = new Vector3(itemSizeBonus, itemSizeBonus, 1);
        }

        damageMultiplier = playerObject.gameObject.GetComponent<ICharacter>().CharacterAttackPower;
        playerCrit = playerObject.gameObject.GetComponent<ICharacter>().CharacterCritRate;

        weaponBody = GetComponent<Rigidbody2D>();
        velocity = 10f;

        critRoll = Random.Range(0, 100);

        turnSpeed = 5f;
        maxDetectDistance = 100f;
        direction = transform.right;
        rb = GetComponent<Rigidbody2D>();

        FindTarget();
    }

    void Update()
    {
        w_Level = playerObject.transform.Find("DroneSpawner").GetComponent<ISpawner>().s_Level;
        w_Damage = DamageDict[w_Level];
        if (GameObject.Find("EnhancedMuscles") != null)
        {
            itemSizeBonus = GameObject.Find("EnhancedMuscles").GetComponent<EnhancedMusclesScript>().sizeBonus;

            this.transform.localScale = new Vector3(itemSizeBonus, itemSizeBonus, 1);
        }
        if(w_Level == 7)
        {
            if(target != null)
            {
                Debug.Log("Target found: " + target.ToString());
                direction = Vector3.RotateTowards(direction, target.position - transform.position, turnSpeed * Time.deltaTime, 0f); // rotate the direction towards the target position
                direction.Normalize(); // normalize the direction vector
            }
            rb.velocity = direction * velocity;
        }

        //Vector3 worldDirectionToPointForward = rb.velocity.normalized; // The world direction to point forward
        //Vector3 currentWorldForwardDirection = transform.TransformDirection(direction); // The current world direction of the object
        //float angleDiff = Vector3.SignedAngle(currentWorldForwardDirection, worldDirectionToPointForward, Vector3.forward); // The angle difference between the current and desired directions
        //transform.Rotate(Vector3.forward, angleDiff, Space.World); // Rotate the object around the z axis by the angle difference

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
                enemiesHit = 0;
                Destroy(this.gameObject);
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

            if (enemiesHit >= HitLimitDict[w_Level])
            {
                enemiesHit = 0;
                Destroy(this.gameObject);
            }
        }
    }

    void FindTarget()
    {
        Debug.Log("Finding Target...");

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, maxDetectDistance, EnemyLayer);

        if (colliders.Length > 0) // if there are any colliders
        {
            float minDistance = Mathf.Infinity; // set the minimum distance to infinity
            Transform closestEnemy = null; // set the closest enemy to null

            foreach (Collider2D collider in colliders) // loop through all the colliders
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position); // get the distance between the bullet and the collider

                if (distance < minDistance) // if the distance is less than the minimum distance
                {
                    minDistance = distance; // update the minimum distance
                    closestEnemy = collider.transform; // update the closest enemy
                }
            }

            target = closestEnemy; // set the target to the closest enemy
        }
    }
}
