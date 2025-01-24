using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] GameObject[] itemPrefabs;
    [SerializeField] Transform playerPosition;
    private void Update() 
    {
        if (Input.GetKey(KeyCode.Q))
        {
            if (ItemManagement.Instance.GetCurrentIndex() !=0)
            {
                Instantiate(itemPrefabs[ItemManagement.Instance.GetCurrentIndex()-1],(playerPosition.position),Quaternion.identity);
                ItemManagement.Instance.SetCurrentWeapon(0);
            }
        }
    }
}
