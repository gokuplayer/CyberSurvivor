using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawner
{
    float spawnRate { get; set; }
    double itemCritDmg { get; set; }
    GameObject s_Weapon { get; set; }
    int s_Level { get; set; }
}
