using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SashimiScript : MonoBehaviour, IHealth
{
    public string HealthName { get; set; }
    public float HealthAmount { get; set; }

    void Awake()
    {
        HealthName = "Sashimi";
        HealthAmount = 30f;
    }
}
