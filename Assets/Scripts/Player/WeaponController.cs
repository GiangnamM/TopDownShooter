using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    
    Vector3 mousePos;
    void Update()
    {
        RotateGun();
    }
     void RotateGun()
    {
        // Xoay theo huong chuot
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = rotation;

        if (transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270)   
            transform.localScale = new Vector3(transform.localScale.x, -0.4f, 0);
        else transform.localScale = new Vector3(transform.localScale.x, 0.4f, 0);


    }
}
