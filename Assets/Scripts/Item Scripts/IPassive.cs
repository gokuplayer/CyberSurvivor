using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface  IPassive
{
    string ItemName { get; set; }
    int ILevel { get; set; }
    GameObject playerObject { get; set; }

    void ItemLevelUp();
}
