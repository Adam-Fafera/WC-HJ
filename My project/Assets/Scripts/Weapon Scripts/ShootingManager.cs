using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using TopDown.Movement;

public class ShootingManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> weaponSounds; // Lista dzwiekow dla kazdej broni
    [SerializeField] UnityEngine.AudioSource audioSource; // zrodlo dzwieku
    [SerializeField] GameObject bullet;
    [SerializeField] Transform weaponPos;
    Vector3 lastPos;
    float cooldown;
    private float lastShotTime = 0f; // czas ostatniego bulleta
    private bool isAiming = false;
    private bool isFirstShot=true;
    private Movement movement;


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

    [SerializeField] GameObject[] Throwables;

    [SerializeField] private LineRenderer leftLine;
    [SerializeField] private LineRenderer rightLine;
    public ParticleSystem BulletTrail;
    public ParticleSystem Sparks;

    public void Start()
    {
        lastPos = this.transform.position;
        movement = GetComponent<Movement>();
    }

    public void FixedUpdate()
    {
        if (this.transform.position != lastPos)
        {
            currentSpreadAngle += (currentSpreadAngle < maxSpreadAngle) ? spreadIncreaseOnMovement : 0;
            lastPos = this.transform.position;
        }
        else if (isAiming == true)
        {
            currentSpreadAngle -= (currentSpreadAngle > baseSpreadAngle) ? spreadDecreaseOnStay : 0;
        }
        if (isAiming == true)
        {
            currentSpreadAngle -= (currentSpreadAngle > baseSpreadAngle) ? spreadDecreaseOnNotShooting : 0;
            Vector3 leftDirection = Quaternion.Euler(0, 0, -currentSpreadAngle / 2) * this.transform.up;
            Vector3 rightDirection = Quaternion.Euler(0, 0, currentSpreadAngle / 2) * this.transform.up;

            leftLine.SetPosition(0, this.transform.position);
            leftLine.SetPosition(1, this.transform.position + leftDirection * 5f); // Dlugosc linii

            rightLine.SetPosition(0, this.transform.position);
            rightLine.SetPosition(1, this.transform.position + rightDirection * 5f);
        }
        else
        {

        }

    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            OnRightClickPress();
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            OnRightClickRelease();
        }
    }
    private void OnRightClickPress()
    {
        switch (ItemManagement.Instance.GetCurrentIndex())
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
                {
                    isAiming = true;
                    leftLine.gameObject.SetActive(true);
                    rightLine.gameObject.SetActive(true);
                    movement.MovementSpeed /= 2;
                    break;
                }
            case 5:
                {
                    Instantiate(Throwables[0], weaponPos.transform.position, this.transform.rotation);
                    ItemManagement.Instance.SetCurrentWeapon(0);
                    break;
                }
            case 6:
                {
                    Instantiate(Throwables[1], weaponPos.transform.position, this.transform.rotation);
                    ItemManagement.Instance.SetCurrentWeapon(0);
                    break;
                }
            case 7:
                {
                    Instantiate(Throwables[2], weaponPos.transform.position, this.transform.rotation);
                    ItemManagement.Instance.SetCurrentWeapon(0);
                    break;
                }
            case 8:
                {
                    Instantiate(Throwables[3], weaponPos.transform.position, this.transform.rotation);
                    ItemManagement.Instance.SetCurrentWeapon(0);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
    private void OnRightClickRelease()
    {
        if (isAiming == true)
        {
            movement.MovementSpeed *= 2;
        }
        leftLine.gameObject.SetActive(false);
        rightLine.gameObject.SetActive(false);
        isAiming = false;
    }
    private void OnFire(InputValue value)
    {
        if (isFirstShot)
        {
            SceneHandler.Instance.SetPanicModeForAll(PanicMode.Panic);
            isFirstShot = false;
        }
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
                        if (isAiming == true) {
                            if (audioSource != null && weaponSounds[currentWeaponIndex] != null)
                            {
                                audioSource.PlayOneShot(weaponSounds[currentWeaponIndex]);
                            }
                           RaySingle();
                                }
                        break;
                    }
                case 2:
                    {
                        if (isAiming == true)
                        {
                            if (audioSource != null && weaponSounds[currentWeaponIndex] != null)
                            {
                                audioSource.PlayOneShot(weaponSounds[currentWeaponIndex]);
                            }
                            RaySingle();
                        }
                        break;
                    }
                case 3:
                    {
                        if (isAiming == true)
                        {
                            if (audioSource != null && weaponSounds[currentWeaponIndex] != null)
                            {
                                audioSource.PlayOneShot(weaponSounds[currentWeaponIndex]);
                            }
                            RaySingle();
                        }
                        break;
                    }
                case 4:
                    {
                        if (isAiming == true) StartCoroutine(RayBurst(3, 0.1f));
                        if (audioSource != null && weaponSounds[currentWeaponIndex] != null)
                        {
                            audioSource.PlayOneShot(weaponSounds[currentWeaponIndex]);
                        }
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
                default:
                    {
                        break;
                    }
            }

           


        }
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

    void RaySingle()
    {
        LayerMask hitLayers = LayerMask.GetMask("Enemy");
        int damage = ItemManagement.Instance.currentWeapon.dmg;

        Vector2 direction = Quaternion.Euler(0, 0, this.transform.rotation.eulerAngles.z + Random.Range(-currentSpreadAngle, currentSpreadAngle)) * Vector2.up;
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction, 100f, hitLayers);

        if (hit.collider != null)
        {
            hit.collider.GetComponent<Health>()?.TakeDamage(damage);
        }

        ItemManagement.Instance.UpdateAmmo(-1);
        currentSpreadAngle += (currentSpreadAngle < maxSpreadAngle) ? 20 : 0;
        BulletTrail.Play();
        Sparks.Play();
    }

    void RayTriple()
    {
        LayerMask hitLayers = LayerMask.GetMask("Enemy");
        int damage = ItemManagement.Instance.currentWeapon.dmg;

        float tempRandom = Random.Range(-currentSpreadAngle, currentSpreadAngle);

        //LeftRay
        Vector2 leftDirection = Quaternion.Euler(0, 0, this.transform.rotation.eulerAngles.z - 10 + tempRandom) * Vector2.up;
        RaycastHit2D leftHit = Physics2D.Raycast(this.transform.position, leftDirection, 100f, hitLayers);
        if (leftHit.collider != null)
        {
            leftHit.collider.GetComponent<Health>()?.TakeDamage(damage);
        }

        //MiddleRay
        Vector2 middleDirection = Quaternion.Euler(0, 0, this.transform.rotation.eulerAngles.z + tempRandom) * Vector2.up;
        RaycastHit2D middleHit = Physics2D.Raycast(this.transform.position, middleDirection, 100f, hitLayers);
        if (middleHit.collider != null)
        {
            middleHit.collider.GetComponent<Health>()?.TakeDamage(damage);
        }

        //RightRay
        Vector2 rightDirection = Quaternion.Euler(0, 0, this.transform.rotation.eulerAngles.z + 10 + tempRandom) * Vector2.up;
        RaycastHit2D rightHit = Physics2D.Raycast(this.transform.position, rightDirection, 100f, hitLayers);
        if (rightHit.collider != null)
        {
            rightHit.collider.GetComponent<Health>()?.TakeDamage(damage);
        }
        BulletTrail.Play();
        Sparks.Play();
        ItemManagement.Instance.UpdateAmmo(-3);
        currentSpreadAngle += (currentSpreadAngle < maxSpreadAngle) ? 40 : 0;
    }

    IEnumerator RayBurst(int shots, float time)
    {
        LayerMask hitLayers = LayerMask.GetMask("Enemy");
        int damage = ItemManagement.Instance.currentWeapon.dmg;

        for (int i = 0; i < shots; i++)
        {
            if (ItemManagement.Instance.currentWeapon.ammo > 0)
            {
                Vector2 direction = Quaternion.Euler(0, 0, this.transform.rotation.eulerAngles.z + Random.Range(-currentSpreadAngle, currentSpreadAngle)) * Vector2.up;
                RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction, 100f, hitLayers);

                if (hit.collider != null)
                {
                    hit.collider.GetComponent<Health>()?.TakeDamage(damage);
                }
                BulletTrail.Play();
                Sparks.Play();
                ItemManagement.Instance.UpdateAmmo(-1);
                currentSpreadAngle += (currentSpreadAngle < maxSpreadAngle) ? 10 : 0;

                yield return new WaitForSeconds(time);
            }
            else
            {
                break;
            }
        }
    }
}
