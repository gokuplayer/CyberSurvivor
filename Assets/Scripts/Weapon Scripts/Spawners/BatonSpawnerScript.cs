using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatonSpawnerScript : MonoBehaviour, ISpawner
{
    public float spawnRate { get; set; }
    public double itemCritDmg { get; set; }
    [field: SerializeReference] public GameObject s_Weapon { get; set; }
    public GameObject PlayerObject;
    public GameObject GameManagerObject;
    public int s_Level { get; set; }
    public float spawnRadius;
    public float projectileVelocity;

    private float weaponDelay;
    private float weaponTimer;

    Dictionary<int, float> SpawnRateDict = new Dictionary<int, float>()
    {
        {1, 1f},
        {2, 1f},
        {3, 1f},
        {4, 1.15f},
        {5, 1.15f},
        {6, 1.15f},
        {7, 1.15f}
    };

    void Start()
    {
        PlayerObject = GameObject.FindWithTag("Player");
        GameManagerObject = GameObject.Find("Game Manager");
        this.transform.parent = PlayerObject.transform;

        weaponDelay = 1f;
        weaponTimer = 2f;
        s_Level = 1;
        if(GameObject.Find("SmartChip") != null)
        {
            itemCritDmg = GameObject.Find("SmartChip").GetComponent<SmartChipScript>().critDmgBonus;
        }
        else
        {
            itemCritDmg = 0.0;
        }
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f;
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Calculate the direction towards the mouse cursor
        Vector3 direction = targetPosition - transform.position;

        // Calculate the angle between the direction vector and Vector3.right (1, 0, 0)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Update the rotation of the BulletSpawnPoint to face the mouse cursor
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Calculate the offset vector based on the rotation angle and spawnRadius
        float offsetX = spawnRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
        float offsetY = spawnRadius * Mathf.Sin(angle * Mathf.Deg2Rad);
        Vector3 offsetVector = new Vector3(offsetX, offsetY, 0f);

        float angleBetweenVelocityAndMouse = Vector2.Angle(PlayerObject.GetComponent<Rigidbody2D>().velocity, direction);

        if (weaponTimer > weaponDelay && PlayerObject.GetComponent<ICharacter>().CharacterHP > 0)
        {
            GameObject baton = Instantiate(s_Weapon, transform.position + offsetVector, transform.rotation);
            Rigidbody2D batonRb = baton.GetComponent<Rigidbody2D>();

            Vector2 batonDirection = direction.normalized;

            // Calculate the adjusted velocity based on droneDirection, player's x velocity, and projectileVelocity
            float playerXVelocity = PlayerObject.GetComponent<Rigidbody2D>().velocity.x;
            float playerYVelocity = PlayerObject.GetComponent<Rigidbody2D>().velocity.y;
            Vector2 adjustedVelocity = (batonDirection * projectileVelocity) + new Vector2(playerXVelocity, playerYVelocity);

            // Ensure the bullet's velocity has a minimum value
            adjustedVelocity = adjustedVelocity.normalized * Mathf.Max(adjustedVelocity.magnitude, projectileVelocity);

            // Assign the adjusted velocity to the drone Rigidbody2D
            batonRb.velocity = adjustedVelocity;

            weaponTimer = 0;
        }
        else
        {
            weaponTimer += Time.deltaTime * spawnRate;
        }

        if (GameObject.Find("Adrenal Booster") != null)
        {
            if (!GameManagerObject.GetComponent<GameManager>().PausedGame)
            {
                spawnRate = SpawnRateDict[s_Level] * GameObject.Find("Adrenal Booster").GetComponent<AdrenalBoosterScript>().spawnRateBonus;
            }
        }
        else if(!GameManagerObject.GetComponent<GameManager>().PausedGame)
        {
            spawnRate = SpawnRateDict[s_Level];
        }


        if (GameObject.Find("SmartChip") != null)
        {
            itemCritDmg = GameObject.Find("SmartChip").GetComponent<SmartChipScript>().critDmgBonus;
        }
    }
}
