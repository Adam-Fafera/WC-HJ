using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int health;
 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            health -= ItemManagement.Instance.currentWeapon.dmg;//to be changed
            //^this should not be like this, damage should be in the bullet class
            //and should be inherited from the weapon it was shot from
            //in this case we can switch the weapon mid bullet flight and the damage will change to the other weapon
            //this would also always change the enemies bullet damage
            
            if (health <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
