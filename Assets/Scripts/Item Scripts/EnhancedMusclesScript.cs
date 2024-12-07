using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhancedMusclesScript : MonoBehaviour, IPassive
{
    public string ItemName { get; set; }
    public int ILevel { get; set; }
    public GameObject playerObject { get; set; }
    public ICharacter playerScript;
    public float sizeBonus;

    void Start()
    {
        ItemName = "Enhanced Muscles";
        ILevel = 1;
        playerObject = GameObject.FindWithTag("Player");
        playerScript = playerObject.GetComponent<ICharacter>();
        sizeBonus = 1.1f;
    }

    public void ItemLevelUp()
    {
        sizeBonus += 0.1f;
    }
}
