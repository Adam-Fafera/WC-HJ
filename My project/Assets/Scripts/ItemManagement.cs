using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemManagement : MonoBehaviour
{
    public GameObject weapon;
    public Weapon currentWeapon;
    public Weapon[] weapons;

    public static ItemManagement Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetCurrentWeapon(int itemId)
    {
        currentWeapon = weapons[itemId];
        weapon.GetComponent<SpriteRenderer>().sprite = currentWeapon.image;
    }

    public void UpdateAmmo(int value)
    {
        currentWeapon.ammo += value;
        if (currentWeapon.ammo <= 0)
        {
            SetCurrentWeapon(0);
        }
    }
}
[Serializable]
public struct Weapon
{
    public string name;
    public Sprite image;
    public int dmg;
    public int ammo;
}

