using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public float PlayerHP;
    public float MaxHP;
    public float PlayerSpeed = 5.0f;
    public float RateOfAttack = 5.0f;
    public double PlayerAttack = 1;
    public float PlayerCrit = 5f;
    //public float weaponRate;
    public int PlayerLevel;
    public int PlayerXP;
    public int direction;
    public Transform HealthBar;
    public GameObject[] Weapons;

    private Rigidbody2D playerBody;
    private float hInput;
    private float vInput;
    private float hpPercent;
    private float damageDelay;
    private float damageTimer;
    //private float weaponDelay;
    //private float weaponTimer;
    //private float weaponVelocity;
    private int enemyDamage;
    private bool BeingAttacked;
    private GameObject GameManager;

    void Start()
    {

        MaxHP = 100;
        PlayerHP = MaxHP;
        //hpPercent = 1;
        playerBody = GetComponent<Rigidbody2D>();
        HealthBar = this.gameObject.transform.GetChild(0).GetChild(0);
        PlayerLevel = 1;
        PlayerXP = 0;
        damageDelay = 5f;
        damageTimer = 5f;
        //weaponDelay = 1f;
        //weaponTimer = 2f;
        //weaponRate = 1f;
        BeingAttacked = false;
        direction = 1;
        GameManager = GameObject.Find("Game Manager");

        Weapons = new GameObject[8];

        /*int weaponIndex = GameManager.GetComponent<GameManager>().WeaponIndex;
        Debug.Log("Weapon Index: " + weaponIndex.ToString());

        Weapons[weaponIndex] = GameManager.GetComponent<GameManager>().WeaponList[weaponIndex];*/

    }

    void Update()
    {

        hInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");

        TakeDamage(BeingAttacked, enemyDamage);

        if(hInput > 0)
        {

            direction = 1;

        }
        if(hInput < 0)
        {

            direction = 2;

        }
        if(vInput > 0)
        {

            direction = 3;

        }
        if(vInput < 0)
        {

            direction = 4;

        }

        /*foreach (GameObject weapon in Weapons)
        {

            if (weapon != null)
            {

                if (weaponTimer > weaponDelay && PlayerHP > 0)
                {

                    GameObject attackWeapon = Instantiate(weapon, transform.position, transform.rotation);
                    //weaponRate = weapon.GetComponent<IWeapon>().spawnRate;
                    Debug.Log("Spawn Rate: " + weapon.GetComponent<IWeapon>().spawnRate.ToString());
                    Debug.Log("Weapon Spawn: " + weaponRate.ToString());

                    weaponTimer = 0;

                }
                else
                {

                    weaponTimer += Time.deltaTime * weaponRate;
                    //weaponTimer += Time.deltaTime;

                }

            }

        }*/

        /*if (weaponTimer > weaponDelay && PlayerHP > 0)
        {

            foreach(GameObject weapon in Weapons)
            {

                if(weapon != null)
                {

                    GameObject attackWeapon = Instantiate(weapon, transform.position, transform.rotation);
                    //weaponVelocity = attackWeapon.GetComponent<IWeapon>().velocity;
                    //weaponVelocity = 200f;
                    //weaponRate = weapon.GetComponent<IWeapon>().spawnRate;
                    Debug.Log("Weapon Spawn: " + weaponRate.ToString());
                    //Debug.Log("Weapon Velocity: " + weaponVelocity.ToString());
                    if (direction == 1)
                    {

                        attackWeapon.GetComponent<Rigidbody2D>().AddRelativeForce(gameObject.transform.right * weaponVelocity);
                        Debug.Log("Added force");

                    }
                    else if (direction == 2)
                    {

                        attackWeapon.GetComponent<Rigidbody2D>().AddRelativeForce(gameObject.transform.right * -weaponVelocity);

                    }
                    else if (direction == 3)
                    {

                        attackWeapon.GetComponent<Rigidbody2D>().AddRelativeForce(gameObject.transform.up * weaponVelocity);

                    }
                    else if (direction == 4)
                    {

                        attackWeapon.GetComponent<Rigidbody2D>().AddRelativeForce(gameObject.transform.up * -weaponVelocity);

                    }

                }

            }

            weaponTimer = 0;

        }
        else
        {

            weaponTimer += Time.deltaTime * weaponRate;

        }*/

    }

    private void FixedUpdate()
    {

        playerBody.velocity = new Vector2(hInput * PlayerSpeed, vInput * PlayerSpeed);

    }

    void OnCollisionEnter2D(Collision2D other)
    {

        if(other.gameObject.tag == "Enemy")
        {

            BeingAttacked = true;
            enemyDamage = other.gameObject.GetComponent<IEnemy>().EnemyAttack;

        }

        if(other.gameObject.tag == "XP Item")
        {

            //PlayerXP += other.gameObject.GetComponent<IXP>().XP_Value;

            if(PlayerXP >= 100)
            {

                LevelUp();

            }

            Destroy(other.gameObject);

        }

    }

    void OnCollisionExit2D(Collision2D other)
    {

        if (other.gameObject.tag == "Enemy")
        {

            BeingAttacked = false;

        }

    }

    void HealthBarChange()
    {

        hpPercent = PlayerHP / MaxHP;

        if(hpPercent >= 0)
        {

            HealthBar.transform.localScale = new Vector3((float)hpPercent, 0.1f, 1f);

        }

    }

    void TakeDamage(bool attacked, int damage)
    {

        if (attacked)
        {

            if(PlayerHP >= 0 && damageTimer > damageDelay)
            {

                PlayerHP -= damage;
                Debug.Log(damage);
                damageTimer = 0;

            }
            else if(PlayerHP <= 0)
            {

                //Debug.Log("You died");

            }
            else
            {

                damageTimer += Time.deltaTime * RateOfAttack;

            }
            HealthBarChange();

        }

    }

    void LevelUp()
    {

        PlayerLevel++;
        PlayerXP -= 100;
        int weaponIndex = GameManager.GetComponent<GameManager>().WeaponIndex;

        //Weapons[weaponIndex] = GameManager.GetComponent<GameManager>().WeaponList[weaponIndex];

        //GameManager.GetComponent<GameManager>().NewWeaponButton.SetActive(true);

    }

}
