using System.Collections.Generic;
using Cainos.PixelArtTopDown_Basic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    public bool inv_showing = false;
    public GameObject slotPrefab;

    public Sprite waterSprite;
    public Sprite fireSprite;
    public Sprite earthSprite;
    public Sprite windSprite;

    [SerializeField] private ElementData elementData;
    private Dictionary<ElementType, ElementData.ElementProperties> elementProperties;

    private void Awake()
    {
        elementProperties = elementData.GetElementProperties();
    }


    private Dictionary<ElementType, int> elementCounts = new Dictionary<ElementType, int>();

    private void Start()
    { 
        inventoryPanel.SetActive(inv_showing);
    }
    private void Update()
    {  
        OpenInventory();
    }
    public void OpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inv_showing = !inv_showing;
            inventoryPanel.SetActive(inv_showing);
        }
    }

    public void AddItemToInventory(ElementType elementType)
    {
        GridLayoutGroup grid = inventoryPanel.GetComponent<GridLayoutGroup>();
        if (grid == null)
        {
            Debug.LogError("No GridLayoutGroup found on inventoryPanel");
            return;
        }
        GameObject newSlot = Instantiate(slotPrefab, inventoryPanel.transform);
        Image slotImage = newSlot.GetComponent<Image>();

        switch(elementType)
        {
            case ElementType.Water:
                slotImage.sprite = waterSprite;
                break;
            case ElementType.Fire:
                slotImage.sprite = fireSprite;
                break;
            case ElementType.Earth:
                slotImage.sprite = earthSprite;
                break;
            case ElementType.Wind:
                slotImage.sprite = windSprite;
                break;
        }
        if (elementCounts.ContainsKey(elementType))
            elementCounts[elementType]++; // Increment element count for that type if it exists
        else
            elementCounts[elementType] = 1; // Add a new element if it doesn't exist
    }
    public bool HasElement(ElementType type)
    {
        return elementCounts.ContainsKey(type) && elementCounts[type] > 0;
    }
    
    public void UseElement(ElementType type)
    {
        if (HasElement(type))
        {
            elementCounts[type]--;

            // Get the grid layout and its children
            GridLayoutGroup grid = inventoryPanel.GetComponent<GridLayoutGroup>();
            Transform gridTransform = grid.transform;

            // Find the last matching element added
            for (int i = gridTransform.childCount -1; i >= 0; i--)
            {
                Transform child = gridTransform.GetChild(i);
                Image image = child.GetComponent<Image>();

                // Check if this is the correct element to remove
                if (IsMatchingElementSprite(image.sprite, type))
                {
                    Destroy(child.gameObject);
                    break; // Exit after removing one sprite
                }
            }
        }
    }

    private bool IsMatchingElementSprite(Sprite sprite, ElementType type)
    {
        return  (type == ElementType.Water && sprite == waterSprite) ||
                (type == ElementType.Fire && sprite == fireSprite) ||
                (type == ElementType.Earth && sprite == earthSprite) ||
                (type == ElementType.Wind && sprite == windSprite);
    }

}
