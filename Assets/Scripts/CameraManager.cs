using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject Player;

    void Start()
    {
        Player = GameObject.FindWithTag("PlayerSpawner");
    }

    void Update()
    {
        if(Player.transform.childCount != 0)
        {
            transform.position = new Vector3(Player.transform.GetChild(0).transform.position.x, Player.transform.GetChild(0).transform.position.y, -10);
        }
    }
}
