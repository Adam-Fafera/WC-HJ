using System.Collections;
using UnityEngine;

public class AiShooting : MonoBehaviour
{
    [SerializeField] float aimTime;
    [SerializeField] private float pistolSpread;
    [SerializeField] private float shotgunSpread;
    [SerializeField] private float rifleSpread;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private SpriteRenderer weaponSprite;


    private GameObject targetPlayer;
    private EnemyAi enemyAi;
    private ItemManagement itemManager;
    private float shootRange;
    private int idWeapon;
    private float shootCooldown;
    private bool canShoot = true;
    private bool isAiming = false;
    public ParticleSystem bulletTrail;
    public ParticleSystem sparks;


    private void Start()
    {
        enemyAi = GetComponent<EnemyAi>();
        itemManager = ItemManagement.Instance;
        idWeapon = Random.Range(1, 5);
        targetPlayer = GameObject.FindGameObjectWithTag("Player");
        shootRange = enemyAi.radius / 2;
        Weapon currentWeapon = itemManager.weapons[idWeapon];
        shootCooldown = itemManager.weapons[idWeapon].cooldown;
        weaponSprite.sprite = itemManager.weapons[idWeapon].image;
        lineRenderer.startColor = new Color(1f, 1f, 1f, 0.5f);
        lineRenderer.endColor = new Color(1f, 1f, 1f, 0.5f);
        lineRenderer.startWidth = 0.1f;
        switch (idWeapon)
        {
            case 1:
            case 2:
                {
                    lineRenderer.endWidth = pistolSpread;
                    break;
                }
            case 3:
                {
                    lineRenderer.endWidth = shotgunSpread;
                    break;
                }
            case 4:
                {
                    lineRenderer.endWidth = rifleSpread;
                    break;
                }
            default:
                {
                    break;
                }
        }
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }


    public void AimAndShoot()
    {
        if (!isAiming)
        {
            Vector3 direction = (targetPlayer.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion lookRotation = Quaternion.Euler(0f, 0f, angle);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * enemyAi.rotationSpeed);
        }

        float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.transform.position);
        if (distanceToPlayer <= shootRange && canShoot)
        {
            StartCoroutine(AimAndShootSequence());
        }
    }

    private IEnumerator AimAndShootSequence()
    {
        canShoot = false;
        isAiming = true;

        Vector3 direction = transform.right; // transform.right wskazuje "w prawo" w przestrzeni lokalnej obiektu

        lineRenderer.enabled = true;

        Vector3 lineEndPoint = shootPoint.position + direction * shootRange;
        lineRenderer.SetPosition(0, shootPoint.position);
        lineRenderer.SetPosition(1, lineEndPoint);

        float elapsedTime = 0f;
        while (elapsedTime < aimTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / aimTime;
            Color currentColor = Color.Lerp(new Color(1f, 1f, 1f, 0.5f), new Color(0.5f, 0.5f, 0.5f, 0.5f), t);
            lineRenderer.startColor = currentColor;
            lineRenderer.endColor = currentColor;
            yield return null;
        }

        RaycastHit2D hit = Physics2D.Raycast(shootPoint.position, direction, shootRange, playerLayer);

        bulletTrail.Play();
        sparks.Play();

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            SceneHandler.Instance.DisplayGameOverScreen();
        }

        lineRenderer.enabled = false;
        isAiming = false;

        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

}
