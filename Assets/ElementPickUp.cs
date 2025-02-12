using UnityEngine;
using UnityEngine.UIElements;

public class ElementPickUp : MonoBehaviour
{
    private InventoryController inventory;
    public ElementType elementType;

    
    void Start()
    {
        // Find the inventory controller in the scene
        inventory = FindFirstObjectByType<InventoryController>();

        if (inventory == null)
        {
            Debug.LogError("No InventoryController found in the scene!");
        }
        
    }
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered is the player
        if (other.CompareTag("Player"))
        {
            // Add to inventory
            AddToInventory();
            // Remove element from scene
            Destroy(gameObject);
        }
    }
    private void AddToInventory()
    {
        if (inventory != null)
        {
            inventory.AddItemToInventory(elementType);
        }
    }

}
