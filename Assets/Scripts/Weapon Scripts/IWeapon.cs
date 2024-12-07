using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    float w_DestroyMax { get; set; }
    float w_DestroyTimer { get; set; }
    double w_Damage { get; set; }
    float w_Crit { get; set; }
    float targetHealth { get; set; }
    double damageMultiplier { get; set; }
    float totalDamage { get; set; }
    float playerCrit { get; set; }
    float critRoll { get; set; }
    double critDamage { get; set; }
    double itemCritDmg { get; set; }
    float velocity { get; set; }
    float spawnOffset { get; set; }
    int w_Level { get; set; }
    GameObject playerObject { get; set; }
    Rigidbody2D weaponBody { get; set; }
}
