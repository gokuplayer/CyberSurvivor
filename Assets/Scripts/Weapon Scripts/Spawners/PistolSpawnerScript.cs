using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolSpawnerScript : MonoBehaviour, ISpawner
{
    public float spawnRate { get; set; }
    public double itemCritDmg { get; set; }
    [field: SerializeReference] public GameObject s_Weapon { get; set; }
    public GameObject PlayerObject;
    public GameObject GameManagerObject;
    public int s_Level { get; set; }
    public float projectileMultiplier;
    public float spawnRadius;
    public float projectileVelocity;

    public GameObject knifeObject;

    private float weaponDelay;
    private float weaponTimer;

    Dictionary<int, int> AmountDict = new Dictionary<int, int>()
    {
        {1, 1},
        {2, 2},
        {3, 2},
        {4, 2},
        {5, 2},
        {6, 2},
        {7, 2}
    };

    Dictionary<int, float> SpawnRateDict = new Dictionary<int, float>()
    {
        {1, 2f},
        {2, 2f},
        {3, 2f},
        {4, 2f},
        {5, 2.3f},
        {6, 2.3f},
        {7, 2.3f}
    };

    void Start()
    {
        PlayerObject = GameObject.FindWithTag("Player");
        GameManagerObject = GameObject.Find("Game Manager");
        this.transform.parent = PlayerObject.transform;

        weaponDelay = 1f;
        weaponTimer = 2f;
        spawnRate = 2f;
        s_Level = 1;

        projectileMultiplier = 1f;

        if (GameObject.Find("SmartChip") != null)
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
        //Vector3 offsetVector = new Vector3(offsetX, offsetY, 0f);

        float angleBetweenVelocityAndMouse = Vector2.Angle(PlayerObject.GetComponent<Rigidbody2D>().velocity, direction);

        if (weaponTimer > weaponDelay && PlayerObject.GetComponent<ICharacter>().CharacterHP > 0)
        {
            float distanceBetweenProjectiles = 1.0f;

            if(s_Level == 7)
            {
                Vector3 offsetVector = new Vector3(offsetX, offsetY, 0f);
                GameObject pistolKnife = Instantiate(knifeObject, transform.position + offsetVector, transform.rotation);
                Rigidbody2D pistolKnifeRb = pistolKnife.GetComponent<Rigidbody2D>();

                Vector2 pistolKnifeDirection = direction.normalized;
                float playerXVelocity = PlayerObject.GetComponent<Rigidbody2D>().velocity.x;
                float playerYVelocity = PlayerObject.GetComponent<Rigidbody2D>().velocity.y;
                Vector2 adjustedVelocity = (pistolKnifeDirection * projectileVelocity) + new Vector2(playerXVelocity, playerYVelocity);
                adjustedVelocity = adjustedVelocity.normalized * Mathf.Max(adjustedVelocity.magnitude, projectileVelocity);
                pistolKnifeRb.velocity = adjustedVelocity;
            }

            for (int i = 1; i <= AmountDict[s_Level]; i++)
            {
                Vector3 offsetVector = new Vector3(distanceBetweenProjectiles * i, 0f, 0f);

                offsetVector = Quaternion.Euler(0f, 0f, angle) * offsetVector;

                GameObject pistol = Instantiate(s_Weapon, transform.position + offsetVector, transform.rotation);
                Rigidbody2D pistolRb = pistol.GetComponent<Rigidbody2D>();

                Vector2 pistolDirection = direction.normalized;

                // Calculate the adjusted velocity based on droneDirection, player's x velocity, and projectileVelocity
                float playerXVelocity = PlayerObject.GetComponent<Rigidbody2D>().velocity.x;
                float playerYVelocity = PlayerObject.GetComponent<Rigidbody2D>().velocity.y;
                Vector2 adjustedVelocity = (pistolDirection * projectileVelocity) + new Vector2(playerXVelocity, playerYVelocity);

                // Ensure the bullet's velocity has a minimum value
                adjustedVelocity = adjustedVelocity.normalized * Mathf.Max(adjustedVelocity.magnitude, projectileVelocity);

                // Assign the adjusted velocity to the drone Rigidbody2D
                pistolRb.velocity = adjustedVelocity;
            }

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
        else if (!GameManagerObject.GetComponent<GameManager>().PausedGame)
        {
            spawnRate = SpawnRateDict[s_Level];
        }

        if (GameObject.Find("SmartChip") != null)
        {
            itemCritDmg = GameObject.Find("SmartChip").GetComponent<SmartChipScript>().critDmgBonus;
        }
    }
}