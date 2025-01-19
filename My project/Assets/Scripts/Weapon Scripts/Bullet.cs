using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour //bullet should inherit it's damage value from the weapon it was shot from
{
    private Rigidbody2D rig;
    [SerializeField] private float bulletSpeed; //speed of the projectile
    [SerializeField] private int destroyTime; //time it exists untill it's destroyed.
    [SerializeField] private int bulletDamage;
    void Start()
    {
        rig = this.GetComponent<Rigidbody2D>();

        Destroy(gameObject,destroyTime); //added object destruction after X ammount of time (5 seconds as of now)

    }

    void FixedUpdate()
    {
        rig.velocity = transform.TransformDirection(new Vector3(0, bulletSpeed, 0)); //Changed the position of the projectile
    }

    private void OnTriggerEnter2D(Collider2D collider) //handles bullet collision
    {
        if (collider.gameObject.tag=="Enemy")
        {
            collider.gameObject.GetComponent<Health>().TakeDamage(bulletDamage);
        }
        if (collider.gameObject.tag != "Projectile")
        {
            Destroy(this.gameObject);
        }

    }
}
