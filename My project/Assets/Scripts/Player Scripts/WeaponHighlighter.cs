using UnityEngine;

public class WeaponHighlighter : MonoBehaviour
{
    public float detectionRange = 5f;
    private Transform closestWeapon;
    private SpriteRenderer previousRenderer; 
    private Color originalColor;
    public float colorChange = 0.4f;

    void Update()
    {
        HighlightWeaponUnderMouse();
    }

    void HighlightWeaponUnderMouse()
    {
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero);

        if (hit.collider != null && IsWeapon(hit.transform))
        {
            float distanceToPlayer = Vector2.Distance(transform.position, hit.transform.position); // sprawdza czy gracz jest w zasiêgu
            if (distanceToPlayer <= detectionRange)
            {
                if (hit.transform != closestWeapon)
                {
                    if (previousRenderer != null)
                    {
                        previousRenderer.color = originalColor;
                    }

                    closestWeapon = hit.transform;

                    SpriteRenderer renderer = closestWeapon.GetComponent<SpriteRenderer>();
                    if (renderer != null)
                    {
                        originalColor = renderer.color;
                        renderer.color = originalColor * colorChange;
                        previousRenderer = renderer;
                    }
                }
            }
            else
            {
                if (previousRenderer != null) //wy³¹cza podœwietlenie po oddaleniu 
                {
                    previousRenderer.color = originalColor;
                    previousRenderer = null;
                    closestWeapon = null;
                }
            }
        }
        else
        {
            if (previousRenderer != null) //wy³¹cza po zdjêciu myszki 
            {
                previousRenderer.color = originalColor;
                previousRenderer = null;
                closestWeapon = null;
            }
        }
    }
    private bool IsWeapon(Transform obj)
    {
        for (int i = 1; i <= 7; i++)
        {
            if (obj.CompareTag(i.ToString()))
            {
                return true;
            }
        }
        return false;
    }
}
