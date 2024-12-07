using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DmgTextScript : MonoBehaviour
{
    public float ScrollSpeed;
    public Vector3 MaxScale;
    public float DestroyMax;
    public float DestroyTimer;

    void Start()
    {
        DestroyTimer = 0f;
    }

    void Update()
    {
        transform.Translate(Vector3.up * ScrollSpeed * Time.deltaTime);
        float scaleIncrement = ScrollSpeed * Time.deltaTime;
        Vector3 newScale = Vector3.Lerp(transform.localScale, MaxScale, scaleIncrement);

        transform.localScale = newScale;

        DestroyTimer += Time.deltaTime;

        if(DestroyTimer > DestroyMax)
        {
            Destroy(this.gameObject);
            DestroyTimer = 0f;
        }
    }
}
