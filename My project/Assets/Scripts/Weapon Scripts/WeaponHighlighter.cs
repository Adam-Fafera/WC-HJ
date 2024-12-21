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
                itemId = int.Parse(highlightedItem.gameObject.tag);
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
        if (Input.GetKey(KeyCode.E))
        {
            if (pickable == true)
            {
                ItemManagement.Instance.SetCurrentWeapon(itemId); // zmiana itemu (funkcja) w itemManagement
            }
        }

        void ApplyHighlight(GameObject item)
        {
            SpriteRenderer spriteRenderer = item.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.blue;
            }
        }

        void ResetHighlight(GameObject item)
        {
            SpriteRenderer spriteRenderer = item.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.white;
            }
        }
    }
}
