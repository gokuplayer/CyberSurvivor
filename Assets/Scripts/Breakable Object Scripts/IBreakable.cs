using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBreakable
{
    string ObjectName { get; set; }
    double ObjectHP { get; set; }
    float MaxOHP { get; set; }
}
