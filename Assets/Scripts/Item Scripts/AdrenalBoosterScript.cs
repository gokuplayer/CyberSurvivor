using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdrenalBoosterScript : MonoBehaviour, IPassive
{
    public string ItemName { get; set; }
    public int ILevel { get; set; }
    public GameObject playerObject { get; set; }
    public float spawnRateBonus;

    void Start()
    {
        ItemName = "Adrenal Booster";
        ILevel = 1;
        playerObject = GameObject.FindWithTag("Player");
        spawnRateBonus = 1.08f;
    }

    public void ItemLevelUp()
    {
        spawnRateBonus += 0.08f;
    }
}
