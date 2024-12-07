using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credit_Tier1_Script : MonoBehaviour, ICredit
{
    public int Credit_Value { get; set; }

    void Start()
    {
        Credit_Value = 1;
    }
}
