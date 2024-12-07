using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{

    /*private float p_DestroyMax;
    private float p_DestroyTimer;
    private float p_Damage;
    private float p_Crit;
    private float targetHealth;
    private float damageMultiplier;
    private float totalDamage;
    private float playerCrit;
    private float critRoll;
    private GameObject playerObject;

    void Start()
    {

        p_DestroyMax = 2f;
        p_DestroyTimer = 0f;
        p_Damage = 5f;
        p_Crit = 0;
        playerObject = GameObject.Find("Player1");
        damageMultiplier = playerObject.gameObject.GetComponent<PlayerManager>().PlayerAttack;
        playerCrit = playerObject.gameObject.GetComponent<PlayerManager>().PlayerCrit;

        critRoll = Random.Range(0, 100);

    }

    void Update()
    {

        p_DestroyTimer += Time.deltaTime;
        if (p_DestroyTimer > p_DestroyMax)
        {

            Destroy(this.gameObject);
            p_DestroyTimer = 0f;

        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log("hit!");
        if (other.gameObject.tag == "Enemy")
        {

            if(critRoll <= (p_Crit + playerCrit))
            {

                other.gameObject.GetComponent<IEnemy>().EnemyHP -= (p_Damage * damageMultiplier * 1.5f);
                Debug.Log("Critical Hit!");

            }
            else
            {

                other.gameObject.GetComponent<IEnemy>().EnemyHP -= (p_Damage * damageMultiplier);

            }

            /*if(other.gameObject.GetComponent<IEnemy>().EnemyHP <= 0)
            {

                Destroy(other.gameObject);

            }

            Destroy(this.gameObject);

        }

    }*/

}
