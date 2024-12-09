using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ShootingManager : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform weaponPos;

    /*
    void Update()
    {
       
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (ItemManagement.Instance.currentWeapon.ammo > 0)
            {
                ItemManagement.Instance.UpdateAmmo(-1);
                Instantiate(bullet, weaponPos.transform.position, this.transform.rotation);
            }
        }
        
    }
    */
    private void OnFire(InputValue value)
    {
        Debug.Log("shoot");
        if (ItemManagement.Instance.currentWeapon.ammo > 0)
        {
            ItemManagement.Instance.UpdateAmmo(-1);
            Instantiate(bullet, weaponPos.transform.position, this.transform.rotation);
        }
    }
}
