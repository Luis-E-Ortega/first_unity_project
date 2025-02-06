using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel;
    public bool inv_showing = false;
    public GameObject slotPrefab;

    public Sprite waterSprite;
    public Sprite fireSprite;
    public Sprite earthSprite;
    public Sprite windSprite;

    private void Start()
    {
        Debug.Log("Start method called");  
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
            Debug.Log("I key was pressed");
            Debug.Log("Panel active state: " + inventoryPanel.activeSelf);
            inv_showing = !inv_showing;
            inventoryPanel.SetActive(inv_showing);
        }
    }
    // Leaving AddItem as a reference for code or later use
    public void AddItem()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GridLayoutGroup grid = inventoryPanel.GetComponent<GridLayoutGroup>();
        
            if (grid == null)
            {
                Debug.LogError("No GridLayoutGroup found on inventoryPanel!");
                return;
            }

            GameObject newSlot = Instantiate(slotPrefab, inventoryPanel.transform);

            Debug.Log("Added new slot: " + newSlot.name);

            foreach(Transform child in inventoryPanel.transform)
            {
                Debug.Log("Found a grid cell: " + child.name);
            }
        }
    }

    public void AddItemToInventory(string elementType)
    {
        GridLayoutGroup grid = inventoryPanel.GetComponent<GridLayoutGroup>();
        GameObject newSlot = Instantiate(slotPrefab, inventoryPanel.transform);
        Image slotImage = newSlot.GetComponent<Image>();

        switch(elementType)
        {
            case "water":
                slotImage.sprite = waterSprite;
                break;
            case "fire":
                slotImage.sprite = fireSprite;
                break;
            case "earth":
                slotImage.sprite = earthSprite;
                break;
            case "wind":
                slotImage.sprite = windSprite;
                break;
        }
        Debug.Log("Added new slot: " + newSlot.name);

        if (grid == null)
        {
            Debug.LogError("No GridLayoutGroup found on inventoryPanel");
            return;
        }
    }

}
