using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class InteractionPointer : MonoBehaviour
{
    public float maxDistance = 5f; // Maksymalna odleg³oœæ Raycast
    public LayerMask interactableLayer; // Warstwa, któr¹ Raycast wykryje
    public LayerMask originalInteractableLayer;
    private GameObject highlightedItem; // Obiekt aktualnie podœwietlony
    private bool pickable;


    void Update()
    {
        Vector2 playerPosition = transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - playerPosition).normalized;


        RaycastHit2D hit = Physics2D.Raycast(playerPosition, direction, maxDistance, interactableLayer);

        if (hit.collider != null)
        {
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
                    int weaponId;
                    if (int.TryParse(highlightedItem.tag, out weaponId))
                    {
                        ItemManagement.Instance.SetCurrentWeapon(weaponId);
                        Destroy(highlightedItem.gameObject);
                    }

                }
            }
        }
    }


    void ApplyHighlight(GameObject item)
        {
            GameObject outline = new GameObject("Outline");
            outline.transform.position = item.transform.position;
            outline.transform.localScale = new Vector3(1.1f, 1.2f, 1f); // Skalowanie konturu
            outline.transform.SetParent(item.transform); // Ustaw kontur jako dziecko obiektu

            SpriteRenderer itemRenderer = item.GetComponent<SpriteRenderer>();
            if (itemRenderer != null)
            {
                SpriteRenderer outlineRenderer = outline.AddComponent<SpriteRenderer>();
                outlineRenderer.sprite = itemRenderer.sprite; // U¿ycie tego samego sprite'a

                // Przypisz materia³ z jasn¹ emisj¹
                Material outlineMaterial = Resources.Load<Material>("WhiteOutlineMaterial");
                outlineMaterial.SetColor("_EmissionColor", Color.white * 2); // Zwiêkszona jasnoœæ
                outlineRenderer.material = outlineMaterial;

                outlineRenderer.sortingOrder = itemRenderer.sortingOrder - 1; // Kontur za obiektem
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
        // Jezli warstwa istnieje (>= 0)
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

