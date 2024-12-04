using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShootingManager : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform weaponPos;
    float cooldown;
    private float lastShotTime = 0f; // czas ostatniego bulleta

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            cooldown=ItemManagement.Instance.currentWeapon.cooldown; // dostosowywanie cd do broni
            if (Time.time >= lastShotTime + cooldown) // Sprawdzenie stanu cd
            {
                if (ItemManagement.Instance.currentWeapon.ammo > 0)
                {
                    lastShotTime = Time.time; 
                    switch (ItemManagement.Instance.GetCurrentIndex()) //switch ktory zbiera index  broni i na podstawie tego wybiera rodzaj strzalu
                    {
                        case 0:
                            {
                                break;
                            }
                        case 1:
                            {
                                ShootSingle();
                                break;
                            }
                        case 2:
                            {
                                ShootSingle();
                                break;
                            }
                        case 3:
                            {
                                ShootTriple();
                                break;
                            }
                        case 4:
                            {
                                StartCoroutine(ShootBurst(3, 0.1f));
                                break;
                            }
                        case 5:
                            {
                                break;
                            }
                        case 6:
                            {
                                MeeleAttack();
                                break;
                            }
                        case 7:
                            {
                                MeeleAttack();
                                break;
                            }
                    }

                }
            }
        }
    }
    void ShootSingle()
    {
        ItemManagement.Instance.UpdateAmmo(-1);
        Instantiate(bullet, weaponPos.transform.position, this.transform.rotation);
    }

    void ShootTriple()
    {
        ItemManagement.Instance.UpdateAmmo(-3);
        Instantiate(bullet, weaponPos.transform.position, this.transform.rotation);
        Instantiate(bullet, weaponPos.transform.position, Quaternion.Euler(0, 0, this.transform.rotation.eulerAngles.z + 10));
        Instantiate(bullet, weaponPos.transform.position, Quaternion.Euler(0, 0, this.transform.rotation.eulerAngles.z - 10));
    }

    void MeeleAttack()
    {
        ItemManagement.Instance.UpdateAmmo(-1);
    }
    IEnumerator ShootBurst(int shots, float time) //ienumerator to funkcja ktora dziala w czasie
    {
        for (int i = 0; i < shots; i++)
        {
            if (ItemManagement.Instance.currentWeapon.ammo > 0) 
            {
                ItemManagement.Instance.UpdateAmmo(-1);
                Instantiate(bullet, weaponPos.transform.position, this.transform.rotation);
                yield return new WaitForSeconds(time); // ta linijka kodu to waiting room do nastepnego bulleta
            }
            else
            {
                break; 
            }
        }
    }
}
