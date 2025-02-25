using UnityEngine;
using UnityEngine.UIElements;

public class ElementPickUp : MonoBehaviour
{
    private InventoryController inventory;
    public ElementType elementType;

    public BossBehavior bossBehavior; // Reference for boss spawning;

    
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
            ElementSpawner.activeOrbs--;

            // Check if there are any orbs left on the screen for boss spawn trigger
            if (ElementSpawner.activeOrbs == 0)
            {
                Debug.Log("activeOrbs is 0! Spawning boss...");
                //bossBehavior.SpawnBoss();
                if (bossBehavior != null)
                {
                    Debug.Log("Boss reference found, starting spawn");
                    StartCoroutine(bossBehavior.SpawnBoss());
                }
                else
                {
                    Debug.Log("Boss reference missing!");
                }
            }
            else
            {
                Debug.Log("activeOrbs remaining: " + ElementSpawner.activeOrbs);
            }
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
