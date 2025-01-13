using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ShootingManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> weaponSounds; // Lista dzwiekow dla kazdej broni
    [SerializeField] UnityEngine.AudioSource audioSource; // zrodlo dzwieku
    [SerializeField] GameObject bullet;
    [SerializeField] Transform weaponPos;
    Vector3 lastPos;
    float cooldown;
    private float lastShotTime = 0f; // czas ostatniego bulleta

    private void Awake()
    {
        audioSource = GetComponent<UnityEngine.AudioSource>();
    }

    [SerializeField] private float baseSpreadAngle = 0f;
    [SerializeField] private float maxSpreadAngle = 60f;
    [SerializeField] private float spreadIncreaseOnMovement = 0.5f;
    [SerializeField] private float spreadDecreaseOnStay = 0.1f;
    [SerializeField] private float spreadDecreaseOnNotShooting = 0.1f;
    private float currentSpreadAngle;

    [SerializeField] private LineRenderer leftLine;
    [SerializeField] private LineRenderer rightLine;

    public void Start()
    {
        lastPos = this.transform.position;
    }

    public void FixedUpdate()
    {
        if (this.transform.position != lastPos)
        {
            currentSpreadAngle += (currentSpreadAngle<maxSpreadAngle)?spreadIncreaseOnMovement:0;
            lastPos = this.transform.position;
        }
        else
        {
            currentSpreadAngle -= (currentSpreadAngle > baseSpreadAngle) ? spreadDecreaseOnStay : 0;
        }
        currentSpreadAngle -= (currentSpreadAngle > baseSpreadAngle) ? spreadDecreaseOnNotShooting : 0;
        Vector3 leftDirection = Quaternion.Euler(0, 0, -currentSpreadAngle/2) * this.transform.up;
        Vector3 rightDirection = Quaternion.Euler(0, 0, currentSpreadAngle/2) * this.transform.up;

        // Ustaw linie
        leftLine.SetPosition(0, this.transform.position);
        leftLine.SetPosition(1, this.transform.position + leftDirection * 5f); // D�ugo�� linii

        rightLine.SetPosition(0, this.transform.position);
        rightLine.SetPosition(1, this.transform.position + rightDirection * 5f);
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
            }

            if (audioSource != null && weaponSounds[currentWeaponIndex] != null)
            {
                audioSource.PlayOneShot(weaponSounds[currentWeaponIndex]); // Odtwarzaj d�wi�k odpowiadaj�cy indeksowi broni
            }


        }
    }
    void ShootSingle()
    {
        ItemManagement.Instance.UpdateAmmo(-1);
        Instantiate(bullet, weaponPos.transform.position, Quaternion.Euler(0, 0, this.transform.rotation.eulerAngles.z + Random.Range(-currentSpreadAngle, currentSpreadAngle)));
        currentSpreadAngle += (currentSpreadAngle < maxSpreadAngle) ? 20 : 0;
    }

    void ShootTriple()
    {
        float tempRandom = Random.Range(-currentSpreadAngle, currentSpreadAngle);
        ItemManagement.Instance.UpdateAmmo(-3);
        Instantiate(bullet, weaponPos.transform.position, Quaternion.Euler(0, 0, this.transform.rotation.eulerAngles.z+tempRandom));
        Instantiate(bullet, weaponPos.transform.position, Quaternion.Euler(0, 0, this.transform.rotation.eulerAngles.z -10 + tempRandom));
        Instantiate(bullet, weaponPos.transform.position, Quaternion.Euler(0, 0, this.transform.rotation.eulerAngles.z +10 + tempRandom));
        currentSpreadAngle += (currentSpreadAngle < maxSpreadAngle) ? 40 : 0;
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
                Instantiate(bullet, weaponPos.transform.position, Quaternion.Euler(0, 0, this.transform.rotation.eulerAngles.z + Random.Range(-currentSpreadAngle, currentSpreadAngle)));
                currentSpreadAngle += (currentSpreadAngle < maxSpreadAngle) ? 10 : 0;
                yield return new WaitForSeconds(time); // ta linijka kodu to waiting room do nastepnego bulleta
            }
            else
            {
                break;
            }
        }
    }

}
