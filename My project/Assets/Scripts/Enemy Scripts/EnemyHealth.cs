using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int health;
    void Start()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            health -= ItemManagement.Instance.currentWeapon.dmg;
            if (health <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
