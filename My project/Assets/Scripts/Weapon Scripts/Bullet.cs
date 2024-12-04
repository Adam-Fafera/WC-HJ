using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rig;
    private Transform bulletTransform;
    private float bulletSpeed;
    void Start()
    {
        rig = this.GetComponent<Rigidbody2D>();
        bulletTransform = this.GetComponent<Transform>();
        bulletSpeed = 10f;
    }

    void FixedUpdate()
    {
        rig.velocity = transform.TransformDirection(new Vector3(0, 15, 0));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Projectile")
        {
            Destroy(this.gameObject);
        }

    }
}
