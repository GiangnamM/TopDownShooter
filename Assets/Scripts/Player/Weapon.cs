using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    int totalWeapon;
    public int currentWeaponIndex;

    public GameObject[] guns;
    public GameObject weaponHolder;
    public GameObject currentGun;
    public bool isChangeGun;
    // Start is called before the first frame update
    void Start()
    {
        totalWeapon = weaponHolder.transform.childCount;
        guns = new GameObject[totalWeapon];
        for (int i = 0; i < totalWeapon; i++)
        {
            guns[i] = weaponHolder.transform.GetChild(i).gameObject;
            guns[i].SetActive(false);
        }
        guns[0].SetActive(true);
        currentGun = guns[0];
        currentWeaponIndex = 0;
    }

    // Update is called once per frame
    public void UpdateGun(int indexGun)
    {
        if (indexGun == 0)
        {
            guns[indexGun].SetActive(true);
            currentGun = guns[indexGun];
            currentWeaponIndex = indexGun;
            guns[guns.Length - 1].SetActive(false);
            isChangeGun = true;
        }
        else if (indexGun < totalWeapon)
        {
            guns[indexGun].SetActive(true);
            currentGun = guns[indexGun];
            currentWeaponIndex = indexGun;
            guns[indexGun - 1].SetActive(false);
            isChangeGun = true;

        }
        else isChangeGun = false;
               

    }
}
