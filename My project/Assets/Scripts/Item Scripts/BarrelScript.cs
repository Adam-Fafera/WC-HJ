using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelScript : MonoBehaviour
{
    [Header("Ustawienia podnoszenia")]
    [SerializeField] private Transform player;         // Referencja do obiektu gracza
    [SerializeField] private Transform holdPoint;      // Punkt, w którym chcemy "trzymaæ" beczkê
    [SerializeField] private float pickUpRange = 6f;   // Zasiêg podnoszenia

    private bool isCarried = false;
    private int previousWeaponIndex = -1;  

    public void TogglePickup()
    {
        if (!isCarried)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= pickUpRange)
            {
                previousWeaponIndex = ItemManagement.Instance.GetCurrentIndex();

                ItemManagement.Instance.SetCurrentWeapon(0);

                transform.SetParent(holdPoint);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;

                isCarried = true;
            }
            else
            {
                Debug.Log("Beczka jest za daleko, ¿eby j¹ podnieœæ.");
            }
        }
        else
        {
            if (previousWeaponIndex != -1)
            {
                ItemManagement.Instance.SetCurrentWeapon(previousWeaponIndex);
            }

            // Odpinamy od gracza
            transform.SetParent(null);

            isCarried = false;
        }
    }
}
