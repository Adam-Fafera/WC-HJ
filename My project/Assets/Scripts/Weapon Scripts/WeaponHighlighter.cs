using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class InteractionPointer : MonoBehaviour
{
    public float maxDistance = 5f; // Maksymalna odleg³oœæ Raycast
    public LayerMask interactableLayer; // Warstwa, któr¹ Raycast wykryje
    private GameObject highlightedItem; // Obiekt aktualnie podœwietlony
    private int itemId;
    private bool pickable;


    void Update()
    {
        Vector2 playerPosition = transform.parent.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - playerPosition).normalized;

        // Wysy³amy Raycast od gracza w kierunku kursora, do maksymalnej odleg³oœci i na warstwie interactableLayer
        RaycastHit2D hit = Physics2D.Raycast(playerPosition, direction, maxDistance, interactableLayer);

        if (hit.collider != null)
        {
            // Je¿eli trafiliœmy w inny obiekt ni¿ poprzednio, zresetuj podœwietlenie starego
            if (highlightedItem != hit.collider.gameObject)
            {
                if (highlightedItem != null)
                {
                    ResetHighlight(highlightedItem);
                }
                // Ustawiamy nowy obiekt jako podœwietlony
                highlightedItem = hit.collider.gameObject;
                pickable = true;

                ApplyHighlight(highlightedItem);
            }
        }
        else
        {
            // Je¿eli Raycast nic nie trafi³, a coœ wczeœniej by³o podœwietlone, resetujemy
            if (highlightedItem != null)
            {
                ResetHighlight(highlightedItem);
                highlightedItem = null;
                pickable = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (highlightedItem != null && pickable)
            {
                // Rozró¿niamy obiekt po tagu
                if (highlightedItem.CompareTag("Barrel"))
                {
                    BarrelScript barrel = highlightedItem.GetComponent<BarrelScript>();
                    if (barrel != null)
                    {
                        barrel.TogglePickup();
                        ResetHighlight(highlightedItem);
                    }
                    
                }
                else
                {
                    int weaponId;
                    if (int.TryParse(highlightedItem.tag, out weaponId))
                    {
                        ItemManagement.Instance.SetCurrentWeapon(weaponId);
                    }
                    
                }
            }
        }
    }

    // Tworzy obiekt "Outline" jako dziecko obiektu, który chcemy podœwietliæ
    void ApplyHighlight(GameObject item)
    {
        GameObject outline = new GameObject("Outline");
        outline.transform.position = item.transform.position;
        outline.transform.localScale = new Vector3(1.1f, 1.2f, 1f); // Skalowanie konturu
        outline.transform.SetParent(item.transform);               // Kontur jako dziecko obiektu

        SpriteRenderer itemRenderer = item.GetComponent<SpriteRenderer>();
        if (itemRenderer != null)
        {
            SpriteRenderer outlineRenderer = outline.AddComponent<SpriteRenderer>();
            outlineRenderer.sprite = itemRenderer.sprite; // U¿ycie tego samego sprite'a

            Material outlineMaterial = Resources.Load<Material>("WhiteOutlineMaterial");
            if (outlineMaterial != null)
            {
                outlineMaterial.SetColor("_EmissionColor", Color.white * 2); // Zwiêkszona jasnoœæ
                outlineRenderer.material = outlineMaterial;
            }
            else
            {
                Debug.LogWarning("Nie znaleziono materia³u 'WhiteOutlineMaterial' w Resources!");
            }

            outlineRenderer.sortingOrder = itemRenderer.sortingOrder - 1;
        }
    }

    void ResetHighlight(GameObject item)
    {
        Transform outline = item.transform.Find("Outline");
        if (outline != null)
        {
            Destroy(outline.gameObject);
        }
    }
}