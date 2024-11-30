using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    bool pickable; // bool ktory sprawdza czy player siedzi na itemie, bo oncollisionstay i ontrigger stay slabo dziala jak ktos sie akurat nie rusza
    int itemId; // zczytuje index itemu ktory podnosi po tagu gameobjectu
    private void OnTriggerEnter2D(Collider2D other)
    {
        itemId = int.Parse(this.gameObject.tag);
        pickable = true;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        pickable = false;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            if (pickable == true)
            {
                ItemManagement.Instance.SetCurrentWeapon(itemId); // zmiana itemu (funkcja) w itemManagement
            }
        }
    }

}
