using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTemp : MonoBehaviour
{

    public float EnemyHP;
    public float MaxEHP;
    public float EnemySpeed;
    public Transform Target;

    void Start()
    {

        MaxEHP = 10f;
        EnemyHP = MaxEHP;
        EnemySpeed = 4.0f;

        Target = GameObject.Find("Player1").transform;

    }

    void Update()
    {

        transform.position = Vector2.MoveTowards(transform.position, Target.position, EnemySpeed * Time.deltaTime);

    }

}
