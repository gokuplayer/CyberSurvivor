using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateScript : MonoBehaviour, IBreakable
{
    public string ObjectName { get; set; }
    public double ObjectHP { get; set; }
    public float MaxOHP { get; set; }

    public GameObject ItemDrop;

    void Awake()
    {
        ObjectName = "Crate";
        MaxOHP = 5f;
        ObjectHP = MaxOHP;

        Debug.Log("Crate HP: " + ObjectHP.ToString());
    }

    void Update()
    {
        if (ObjectHP <= 0)
        {
            Instantiate(ItemDrop, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);

            Destroy(this.gameObject);
        }
    }
}
