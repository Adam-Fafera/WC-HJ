using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
public class ItemManagement : MonoBehaviour
{
    public SpriteRenderer weapon; //czyta gdzie jest teksturka broni
    public Weapon currentWeapon;
    public Weapon[] weapons;
    private int currentIndex;

    [SerializeField] TMP_Text ammoText; //zwykly tekst

    // to i awake odpowiada za to zeby szlo sie z innego skryptu do tego odnosic
    public static ItemManagement Instance;
    private void LoadValues()
    {
        ammoText.text = "Ammo: " + currentWeapon.ammo;
        weapon.sprite = currentWeapon.image;
        currentIndex = 1;
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadValues();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetCurrentWeapon(int itemId)
    {
        currentIndex = itemId;
        currentWeapon = weapons[itemId];
        weapon.sprite = currentWeapon.image;
        ammoText.text = "Ammo: " + currentWeapon.ammo;
    }

    public void UpdateAmmo(int value)
    {
        currentWeapon.ammo += value;
        if (currentWeapon.ammo <= 0) //domyslnie po skonczeniu sie ammo zmienia sie na lape => weapons[0];
        {
            SetCurrentWeapon(0);
        }
        ammoText.text = "Ammo: " + currentWeapon.ammo;
    }

    public int GetCurrentIndex()
    {
        return currentIndex;
    }
}
[Serializable]
public struct Weapon
{
    public string name;
    public Sprite image;
    public int ammo;
    public int dmg;
    public float cooldown;
}

