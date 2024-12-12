using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.SceneManagement;
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
        SetCurrentWeapon(1);
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

            SceneManager.sceneLoaded += OnSceneLoaded;

        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (ammoText == null)
        {
            ammoText = GameObject.Find("Ammo txt")?.GetComponent<TMP_Text>();
        }

        if (weapon == null)
        {
            weapon = GameObject.Find("WeaponSprite")?.GetComponent<SpriteRenderer>();
        }

        if (ammoText != null && currentWeapon.image != null) //otherwise LoadValues() would cause problems in menu, because of it's presence in main scene
        {
            LoadValues();
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

