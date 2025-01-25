using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int health;


    public int HealthGet  
    {
        get {return health;}
        set {health = value;}
    }

    public virtual void TakeDamage(int damage) //used for taking damage
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(this.gameObject);
            // SceneHandler.Instance.DisplayEnemiesLeft();
        }
    }
}
