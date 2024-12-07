using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tier2_XP : MonoBehaviour, IXP
{
    public float XP_Value { get; set; }

    void Start()
    {
        XP_Value = 2f;
    }
}
