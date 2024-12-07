using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    
    string CharacterName { get; set; }
    string StartingWeapon { get; set; }
    float CharacterHP { get; set; }
    float CharacterMaxHP { get; set; }
    float CharacterXP { get; set; }
    float CharacterSpeed { get; set; }
    float CharacterCritRate { get; set; }
    float CharacterHPPercent { get; set; }
    float CharacterHPRecovery { get; set; }
    float itemXPBoost { get; set; }
    float NeededXPToLevel { get; set; }
    float TempSpeed { get; set; }
    float TempCollectSpeed { get; set; }
    double CharacterAttackPower { get; set; }
    int CharacterDefense { get; set; }
    int CharacterLevel { get; set; }
    int direction { get; set; }
    int weaponsSpawned { get; set; }
    int itemsSpawned { get; set; }

    List<GameObject> CharacterWeapons { get; set; }
    List<float> enemySpeeds { get; set; }

    void XPGain(float XPItemValue);
}
