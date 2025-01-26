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
    [SerializeField] private float x=1f;
    [SerializeField] private float y=1f;


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
            GameObject outline = new GameObject("Outline");
            outline.transform.position = item.transform.position;
            outline.transform.localScale = new Vector3(1.1f*x, 1.2f*y, 1f); // Skalowanie konturu
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
    }
}
