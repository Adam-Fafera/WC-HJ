using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class InteractionPointer : MonoBehaviour
{
    public float maxDistance = 5f; // Maksymalna odleglosc Raycast
    public LayerMask interactableLayer; // Warstwa, ktora Raycast wykryje
    public LayerMask originalInteractableLayer;
    private GameObject highlightedItem; // Obiekt aktualnie podswietlony
    private bool pickable;

    private void Awake()
    {
        originalInteractableLayer = interactableLayer;
    }

    void Update()
    {

        Vector2 playerPosition = transform.parent.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - playerPosition).normalized;

        // Wysylamy Raycast od gracza w kierunku kursora, do maksymalnej odleg�o�ci i na warstwie interactableLayer
        RaycastHit2D hit = Physics2D.Raycast(playerPosition, direction, maxDistance, interactableLayer);
        

        if (hit.collider != null)
        {
            // Jezeli trafiliamy w inny obiekt niz poprzednio, zresetuj podswietlenie starego
            if (highlightedItem != hit.collider.gameObject)
            {
                if (highlightedItem != null)
                {
                    ResetHighlight(highlightedItem);

                }
                highlightedItem = hit.collider.gameObject;
                ApplyHighlight(highlightedItem);
                pickable = true;
                
            }
        }
        else
        {
            // Jezeli Raycast nic nie trafi, a cos wczesniej bylo podswietlone, resetujemy
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
                if (highlightedItem.CompareTag("Barrel"))
                {
                    BarrelScript barrel = highlightedItem.GetComponent<BarrelScript>();
                    if (barrel != null)
                    {
                        barrel.TogglePickup();
                    }
                }
                else
                {
                    if (hit.collider != null)
                    {
                        Debug.Log($"Hit Object: {hit.collider.gameObject.name}, Layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)}");
                    }
                    else
                    {
                        Debug.Log("Raycast did not hit any object.");
                    }
                    int weaponId;
                    if (int.TryParse(highlightedItem.tag, out weaponId))
                    {
                        ItemManagement.Instance.SetCurrentWeapon(weaponId);
                    }
                    
                }
            }
        }
    }

    // Tworzy obiekt "Outline" jako dziecko obiektu, ktory chcemy podswietlic
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
            outlineRenderer.sprite = itemRenderer.sprite; // Uzycie tego samego sprite'a

            Material outlineMaterial = Resources.Load<Material>("WhiteOutlineMaterial");
            if (outlineMaterial != null)
            {
                outlineMaterial.SetColor("_EmissionColor", Color.white * 2); // Zwiekszona jasnosc
                outlineRenderer.material = outlineMaterial;
            }
            else
            {
                Debug.LogWarning("Nie znaleziono materia�u 'WhiteOutlineMaterial' w Resources!");
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

    public void SetRaycastToOnlyCarriedBarrel()
    {
        // Zapisz ID warstwy:
        int carriedBarrelLayer = LayerMask.NameToLayer("Barrel");
        // Jesli warstwa istnieje (>= 0)
        if (carriedBarrelLayer >= 0)
        {
            // Maska = 1 << carriedBarrelLayer
            LayerMask onlyCarriedMask = 1 << carriedBarrelLayer;
            interactableLayer = onlyCarriedMask;
        }
        ResetHighlight(highlightedItem);
    }


    public void RestoreOriginalRaycastLayer()
    {
        interactableLayer = originalInteractableLayer;
    }

}
