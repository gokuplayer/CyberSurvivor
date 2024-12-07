using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    string EnemyName { get; set; }
    double EnemyHP { get; set; }
    float MaxEHP { get; set; }
    float EnemySpeed { get; set; }
    int EnemyAttack { get; set; }
    Transform Target { get; set; }
    string EnemyStatus { get; set; }
    float EStatusMaxTime { get; set; }
}
