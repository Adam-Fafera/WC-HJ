using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ShootingManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> weaponSounds; // Lista düwiÍkÛw dla kaødej broni
    [SerializeField] UnityEngine.AudioSource audioSource; // èrÛd≥o düwiÍku
    [SerializeField] GameObject bullet;
    [SerializeField] Transform weaponPos;
    float cooldown;
    private float lastShotTime = 0f; // czas ostatniego bulleta

    private void Awake()
    {
        audioSource = GetComponent<UnityEngine.AudioSource>();
    }

    private void OnFire(InputValue value)
    {



        cooldown = ItemManagement.Instance.currentWeapon.cooldown; // dostosowywanie cd do broni
        if (Time.time >= lastShotTime + cooldown && ItemManagement.Instance.currentWeapon.ammo > 0)
        {
            int currentWeaponIndex = ItemManagement.Instance.GetCurrentIndex();

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
                        MeeleAttack(1.2f);
                        break;
                    }
                case 7:
                    {
                        MeeleAttack(1.2f);
                        audioSource.PlayOneShot(weaponSounds[currentWeaponIndex]);
                        break;
                    }
                case 8:
                    {
                        break;
                    }

            }

            if (audioSource != null && weaponSounds[currentWeaponIndex] != null)
            {
                audioSource.PlayOneShot(weaponSounds[currentWeaponIndex]); // Odtwarzaj düwiÍk odpowiadajπcy indeksowi broni
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

    void MeeleAttack(float meleeRange)
    {
        LayerMask hitLayers = LayerMask.GetMask("Enemy");
        int meleeDamage = ItemManagement.Instance.currentWeapon.dmg;

        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(weaponPos.position, meleeRange, hitLayers); // hit detection
        if (hitTargets.Length != 0) //melee weapons should only have their durability go down on hit.
        {
            ItemManagement.Instance.UpdateAmmo(-1);
        }
        foreach (Collider2D target in hitTargets)
        {
            target.GetComponent<Health>().TakeDamage(meleeDamage);
        }


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
