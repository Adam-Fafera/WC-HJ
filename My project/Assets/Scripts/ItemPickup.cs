using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    bool pickable;
    int itemId;
    private void OnTriggerEnter2D(Collider2D other)
    {
        itemId = int.Parse(this.gameObject.name);
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
                ItemManagement.Instance.SetCurrentWeapon(itemId);
            }        
        }
    }
 
}
