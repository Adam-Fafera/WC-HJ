using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour //bullet should inherit it's damage value from the weapon it was shot from
{
    private Rigidbody2D rig;
    private Transform bulletTransform;
    [SerializeField] private float bulletSpeed; //speed of the projectile
    [SerializeField] private int destroyTime; //time it exists untill it's destroyed.
    private int bulletDamage;
    void Start()
    {
        rig = this.GetComponent<Rigidbody2D>();
        bulletTransform = this.GetComponent<Transform>();
        //bulletSpeed = 10f; //a bit too slow, seems unused
        //changed to serialize field
        bulletDamage = ItemManagement.Instance.currentWeapon.dmg;

        
        Destroy(gameObject,destroyTime); //added object destruction after X ammount of time (5 seconds as of now)

    }

    void FixedUpdate()
    {
        rig.velocity = transform.TransformDirection(new Vector3(0, bulletSpeed, 0)); //Changed the position of the projectile
    }

    private void OnCollisionEnter2D(Collision2D collision) //handles bullet collision
    {
        if (collision.gameObject.tag=="Enemy")
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(bulletDamage);
        }
        if (collision.gameObject.tag != "Projectile")
        {
            Destroy(this.gameObject);
        }

    }
}
