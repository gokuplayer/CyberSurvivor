using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BionicArmScript : MonoBehaviour, IPassive
{
    public string ItemName { get; set; }
    public int ILevel { get; set; }
    public GameObject playerObject { get; set; }
    public ICharacter playerScript;

    void Start()
    {
        ItemName = "Bionic Arm";
        ILevel = 1;
        playerObject = GameObject.FindWithTag("Player");
        playerScript = playerObject.GetComponent<ICharacter>();
        ItemLevelUp();
    }

    public void ItemLevelUp()
    {
        playerScript.CharacterAttackPower += 0.1;
    }
}