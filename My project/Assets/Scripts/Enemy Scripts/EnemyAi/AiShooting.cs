using System.Collections;
using UnityEngine;

public class AiShooting : MonoBehaviour
{
    private float shootRange;
    [SerializeField] private Transform shootPoint;  
    [SerializeField] private LayerMask playerLayer; 

    private GameObject targetPlayer;            
    private EnemyAi enemyAi;                     
    private ItemManagement itemManager;          
    private float shootCooldown;                 
    private bool canShoot = true;                 
    private int idWeapon;
    [SerializeField] SpriteRenderer weaponSprite;

    private void Start()
    {
        enemyAi = GetComponent<EnemyAi>();
        itemManager = ItemManagement.Instance;
        idWeapon = Random.Range(1, 4);
        if (itemManager != null)
        {
            Weapon currentWeapon = itemManager.weapons[idWeapon];
            shootCooldown = itemManager.weapons[idWeapon].cooldown;
            weaponSprite.sprite = itemManager.weapons[idWeapon].image;
        }
        targetPlayer = GameObject.FindGameObjectWithTag("Player");
        shootRange = enemyAi.radius;
    }

    private void Update()
    {
        if (enemyAi.currentState == States.Attacking && targetPlayer != null)
        {
            AimAndShoot();
        }
    }

    private void AimAndShoot()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.transform.position);
        if (distanceToPlayer <= shootRange)
        {
            Vector3 direction = (targetPlayer.transform.position - shootPoint.position).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion lookRotation = Quaternion.Euler(0f, 0f, angle);

            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * enemyAi.rotationSpeed);

            if (canShoot)
            {
                StartCoroutine(Shoot(direction));
            }
        }
    }

    private IEnumerator Shoot(Vector3 direction)
    {
        canShoot = false;
        RaycastHit2D hit = Physics2D.Raycast(shootPoint.position, direction, shootRange, playerLayer);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            Debug.Log("hitlo!");
        }
        else
        {
            Debug.Log("nie hitlo");
        }
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }
}
