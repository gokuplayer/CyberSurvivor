using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartChipScript : MonoBehaviour, IPassive
{
    public string ItemName { get; set; }
    public int ILevel { get; set; }
    public GameObject playerObject { get; set; }
    public double critDmgBonus;

    void Start()
    {
        ItemName = "Smart Chip";
        ILevel = 1;
        playerObject = GameObject.FindWithTag("Player");
        Debug.Log("Smart Chip initiated.");
        critDmgBonus = 0.1;
    }

    public void ItemLevelUp()
    {
        critDmgBonus += 0.1;
    }
}