using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointerScript : MonoBehaviour
{
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f;
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Calculate the direction towards the mouse cursor
        Vector3 direction = targetPosition - transform.position;

        // Calculate the angle between the direction vector and Vector3.right (1, 0, 0)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Update the rotation of the BulletSpawnPoint to face the mouse cursor
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
