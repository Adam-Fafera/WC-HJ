using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShootingManager : MonoBehaviour
{
    public static ShootingManager Instance;

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

    [SerializeField] TMP_Text ammoText;
    public void Start()
    {
        ammoText.text = "Ammo: "+ItemManagement.Instance.currentWeapon.ammo; 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ItemManagement.Instance.UpdateAmmo(-2);
            ammoText.text = "Ammo: " + ItemManagement.Instance.currentWeapon.ammo;
        }
    }
}
