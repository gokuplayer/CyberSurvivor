using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectromagnetSpawnerScript : MonoBehaviour, ISpawner
{
    public float spawnRate { get; set; }
    public double itemCritDmg { get; set; }
    [field: SerializeReference] public GameObject s_Weapon { get; set; }
    public GameObject PlayerObject;
    public GameObject GameManagerObject;
    public int s_Level { get; set; }

    public float weaponDelay;
    public float weaponTimer;

    Dictionary<int, float> SpawnRateDict = new Dictionary<int, float>()
    {
        {1, 0.5f},
        {2, 0.5f},
        {3, 0.5f},
        {4, 0.5f},
        {5, 0.75f},
        {6, 0.75f},
        {7, 0.75f}
    };

    void Start()
    {
        PlayerObject = GameObject.FindWithTag("Player");
        GameManagerObject = GameObject.Find("Game Manager");
        this.transform.parent = PlayerObject.transform;

        weaponDelay = 1f;
        weaponTimer = 2f;
        spawnRate = 0.5f;
        s_Level = 1;

        if (GameObject.Find("SmartChip") != null)
        {
            itemCritDmg = GameObject.Find("SmartChip").GetComponent<SmartChipScript>().critDmgBonus;
        }
        else
        {
            itemCritDmg = 0.0;
        }

        GameObject newElectromagnet = Instantiate(s_Weapon, transform.position + new Vector3(0, 0 , 0.1f), transform.rotation, PlayerObject.transform);
        //newElectromagnet.transform.SetParent(PlayerObject.transform);
    }

    void Update()
    {
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

        /*if (weaponTimer > weaponDelay && PlayerObject.GetComponent<ICharacter>().CharacterHP > 0)
        {
            Instantiate(s_Weapon, transform.position, transform.rotation);

            weaponTimer = 0;
        }
        else
        {
            weaponTimer += Time.deltaTime * spawnRate;
        }*/

        if (GameObject.Find("SmartChip") != null)
        {
            itemCritDmg = GameObject.Find("SmartChip").GetComponent<SmartChipScript>().critDmgBonus;
        }
    }
}
